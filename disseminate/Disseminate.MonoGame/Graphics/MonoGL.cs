using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Graphics;
using Disseminate.Math;
using Microsoft.Xna.Framework.Graphics;
using XnaMath = Microsoft.Xna.Framework;
using MGTexture = Microsoft.Xna.Framework.Graphics.Texture2D;
using XnaBlendState = Microsoft.Xna.Framework.Graphics.BlendState;
using XnaSamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;
using XnaRasterizerState = Microsoft.Xna.Framework.Graphics.RasterizerState;
using XnaDepthStencilState = Microsoft.Xna.Framework.Graphics.DepthStencilState;

namespace Disseminate.MonoGame.Graphics
{
    public class MonoGL : GL
    {
        internal static MonoGL instance { get; private set; }

        internal Texture2D baseWhite { get; private set; }
        internal Material _defaultMaterial;

        private XnaMath.Matrix currentMatrix = XnaMath.Matrix.Identity;

        internal GraphicsDevice _device;
        internal SpriteBatch batch;
        private float _maxDepth = 0f;
        internal float _minDepth = 0f;
        internal float depthRange = 1f;
        private int spritesSubmittedThisBatch = 0;
        private int spritesSubmittedThisFrame = 0;
        private int drawCallsThisFrame = 0;
        private bool applyingScissor;
        internal Material materialState;

        internal XnaBlendState blendState;
        internal XnaDepthStencilState depthState;
        internal XnaRasterizerState rasterizerState;
        internal XnaRasterizerState scissorRasterizerState;
        internal XnaSamplerState samplerState;
        internal Effect effect = null;

        public MonoGL(GraphicsDevice device)
        {
            _device = device;
            batch = new SpriteBatch(device);

            baseWhite = new Texture2D(0, "white", new MGTexture(device, 1, 1));
            baseWhite.SetData(new XnaMath.Color[] { XnaMath.Color.White });
            _defaultMaterial = new Material(0, "default"); 
        }

        protected override GLMaterial defaultMaterial { get { return _defaultMaterial; } }
        protected override GLTexture2D defaultTexture { get { return baseWhite; } }

        public override void begin(IRenderTarget target, GLSortMode mode)
        {
            instance = this;
            //Matrix4.Compose(new Vector3(-(int)position.X, -(int)position.Y, 0), Vector3.UnitScale, Quaternion.Zero);

            base.begin(target, mode);

            _maxDepth = 0f;
            _minDepth = 0f;
        }

        public override void depth(Math.Real amount)
        {
            base.depth(amount);
            _minDepth = Utility.Min(amount, _minDepth);
            _maxDepth = Utility.Max(amount, _maxDepth);
        }

        protected override void onclear(Color color)
        {
            _device.Clear(color.ToXnaColor());
        }

        protected override void onend(GLRenderable[] renderableItems, int length)
        {
            instance = this;

            var start = 0;
            var depthRange = 1f;
            if (_maxDepth != _minDepth)
                depthRange = (_maxDepth - _minDepth);

            for (var i = start; i < start + length; i++)
            {
                var item = renderableItems[i];
                {
                    spritesSubmittedThisFrame++;
                    updateBatchState(item);

                    var texture = item.texture ?? baseWhite;
                    texture.draw(item);

                    spritesSubmittedThisBatch++;
                }
            }

            endBatchState();
        }

        private void endBatchState()
        {
            spritesSubmittedThisBatch = 0;
            if (materialState != null)
            {
                batch.End();

                if (applyingScissor)
                {
                    batch.GraphicsDevice.ScissorRectangle = new XnaMath.Rectangle(0, 0, 800, 480);
                    applyingScissor = false;
                }

                drawCallsThisFrame++;
                materialState = null;
            }
        }

        //private HashSet<string> crap = new HashSet<string>();
        internal void setBatchState(GLRenderable item)
        {
            endBatchState();

            item.material.enable();

            var _rasterizer = rasterizerState;
            if (item.applyScissor)
            {
                _rasterizer = scissorRasterizerState;
                batch.GraphicsDevice.ScissorRectangle = new XnaMath.Rectangle((int)item.scissorRect.X0, (int)item.scissorRect.Y0, (int)item.scissorRect.Width, (int)item.scissorRect.Height);
                applyingScissor = true;
            }

            var width = _device.PresentationParameters.Bounds.Width;
            var height = _device.PresentationParameters.Bounds.Height;
            var position = Vector2.Zero;
            var orientation = 0f;
            var scale = Vector2.One;

            var viewMatrix =
               XnaMath.Matrix.CreateTranslation(new XnaMath.Vector3(-(int)position.X, -(int)position.Y, 0)) *
               XnaMath.Matrix.CreateRotationZ(orientation) *
               XnaMath.Matrix.CreateScale(scale.X, scale.Y, 1) *
               XnaMath.Matrix.CreateTranslation(new XnaMath.Vector3(width * 0.5f, height * 0.5f, 0));
                //Matrix4.CreateTranslation(new Vector3(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0));

            var viewMatrix2 =
               Matrix4.CreateTranslation(new Vector3(-(int)position.X, -(int)position.Y, 0)) *
               Matrix4.CreateRotationZ(orientation) *
               Matrix4.CreateScale(scale.X, scale.Y, 1) *
               Matrix4.CreateTranslation(new Vector3(width * 0.5f, height * 0.5f, 0));
            //Matrix4.CreateTranslation(new Vector3(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0));
            batch.Begin(SpriteSortMode.Deferred, blendState, samplerState, depthState, _rasterizer, effect, viewMatrix2.ToXnaMatrix());
        }

        private void updateBatchState(GLRenderable item)
        {
            if (materialState == null)
                setBatchState(item);
            else if (materialState != item.material)
                setBatchState(item);
            else if (item.applyScissor != applyingScissor)
                setBatchState(item);
            else if (item.applyScissor && applyingScissor && !checkScissorRect(item.scissorRect))
                setBatchState(item);
            else if ((spritesSubmittedThisBatch % 2048) == 0)
                setBatchState(item);

        }

        private bool checkScissorRect(AxisAlignedBox2 aabb)
        {
            var scissor = aabb.ToRect();
            return scissor == batch.GraphicsDevice.ScissorRectangle;
        }
    }
}
