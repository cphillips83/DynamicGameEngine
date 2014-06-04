using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite.Collections;
using zSprite.Resources;
using System.Diagnostics;


namespace zSprite.Managers
{
    public class MonoRenderer
    {
        private bool applyingScissor = false;
        private SpriteBatch batch;
        private Matrix currentMatrix = Matrix.Identity;
        private float depthRange = 0f;
        internal Material materialState;
        private float maxDepth = 0f;
        private float minDepth = 0f;
        //private BlendState blendState = new BlendState();
        //private DepthStencilState depthState = new DepthStencilState();
        //private RasterizerState rasterizerState = new RasterizerState();
        //private SamplerState samplerState = new SamplerState();
        //private Effect effect = null;
        private int renderableIndex = 0;
        private int spritesSubmittedThisBatch = 0;
        private GraphicsDevice _graphicsDevice;

        //private List<DrawItem> renderableItems = new List<DrawItem>();
        private DrawItem[] renderableItems = new DrawItem[1024];

        public RenderQueueSortMode sortMode = RenderQueueSortMode.Material;

        internal MonoRenderer(GraphicsDevice device)
        {
            _graphicsDevice = device;
            batch = new SpriteBatch(device);
        }

        private IEnumerable<DrawItem> activeItems
        {
            get
            {
                for (var i = 0; i < renderableIndex; i++)
                    yield return renderableItems[i];
            }
        }

        private IEnumerable<DrawItem> reverseActiveItems
        {
            get
            {
                for (int i = renderableIndex - 1; i >= 0; i--)
                    yield return renderableItems[i];
            }
        }

        internal void DrawInternal(Material material,
                Vector2 position,
                Vector2 scale,
                AxisAlignedBox sourceRectangle,
                Color color,
                float rotation,
                Vector2 origin,
                SpriteEffects effect,
                float depth)
        {

            var item = new DrawItem();
            item.material = material ?? Root.instance.resources.defaultMaterial;
            //item.texture = texture ?? Root.instance.resources.basewhite;
            item.position = position;
            item.scale = scale;
            item.sourceRectangle = sourceRectangle;
            item.color = color;
            item.rotation = rotation;
            item.origin = origin;
            item.effect = effect;
            item.depth = depth;
            item.applyScissor = Root.instance.graphics.scissorEnabled;
            item.scissorRect = Root.instance.graphics.scissorRect;

            minDepth = Math.Min(item.depth, minDepth);
            maxDepth = Math.Max(item.depth, maxDepth);

            switch (sortMode)
            {
                case RenderQueueSortMode.Material:
                    item.key = item.material.id;
                    break;
                case RenderQueueSortMode.ReverseOrder:
                case RenderQueueSortMode.PreserverOrder:
                    item.key = renderableIndex;
                    break;
            }

            if (renderableItems.Length == renderableIndex)
            {
                Array.Resize(ref renderableItems, renderableItems.Length * 2 / 2);
                //var tempItems = new DrawItem[(renderableItems.Length * 3) / 2];
                //Array.Copy(renderableItems, tempItems, renderableItems.Length);
                //renderableItems = tempItems;
            }

            renderableItems[renderableIndex++] = item;
        }

        internal void Render(Matrix m, RenderTarget2D target, Viewport viewport)
        {
            if (renderableIndex == 0)
                return;

            _graphicsDevice.SetRenderTarget(target);
            _graphicsDevice.Viewport = viewport;
            //_graphicsDevice.Clear(clear);

            currentMatrix = m;
            depthRange = maxDepth - minDepth;

            //var items = new DrawItem[renderableIndex];
            //for (var i = 0; i < renderableIndex; i++)
            //    items[i] = renderableItems[i];

            switch (sortMode)
            {
                case RenderQueueSortMode.Material:
                    Utility.RadixSort(renderableItems, renderableIndex);
                    renderItem(0, renderableIndex);
                    //renderItem(activeItems);
                    //foreach (var group in activeItems.byMaterial())
                    //    renderItem(group.byTexture());
                    break;
                case RenderQueueSortMode.PreserverOrder:
                    renderItem(0, renderableIndex);
                    break;
                case RenderQueueSortMode.ReverseOrder:
                    renderItem(reverseActiveItems);
                    break;
                default:
                    throw new Exception("not implmented");
                case RenderQueueSortMode.Depth:
                    foreach (var groupz in activeItems.GroupBy(x => x.depth).OrderBy(x => x.Key))
                        foreach (var group in groupz.GroupBy(x => x.material))
                            renderItem(group);
                    break;
                //case RenderQueueSortMode.PreserverOrder:
                //    var item = new DrawItem[1];
                //    for (var i = 0; i < renderableIndex; i++)
                //    {
                //        item[0] = items[i];
                //        renderItem(item[0].material, item, depthRange, m);
                //    }
                //    break;

                //case RenderQueueSortMode.Y:

                //    foreach (var groupy in items.GroupBy(x => x.position.Y).OrderBy(x => x.Key))
                //        foreach (var group in groupy.GroupBy(x => x.material))
                //            renderItem(group.Key, group.OrderBy(x => x.texture.id), depthRange, m);
                //    break;

                //case RenderQueueSortMode.YThenDepth:
                //    foreach (var groupy in items.GroupBy(x => x.position.Y).OrderBy(x => x.Key))
                //        foreach (var groupz in groupy.GroupBy(x => x.depth).OrderBy(x => x.Key))
                //            foreach (var group in groupz.GroupBy(x => x.material))
                //                renderItem(group.Key, group.OrderBy(x => x.texture.id), depthRange, m);
                //    break;

                //case RenderQueueSortMode.Depth:
                //    foreach (var groupz in items.GroupBy(x => x.depth).OrderBy(x => x.Key))
                //        foreach (var group in groupz.GroupBy(x => x.material))
                //            renderItem(group.Key, group.OrderBy(x => x.texture.id), depthRange, m);
                //    break;

                //case RenderQueueSortMode.DepthThenY:
                //    foreach (var groupz in items.GroupBy(x => x.depth).OrderBy(x => x.Key))
                //        foreach (var groupy in groupz.GroupBy(x => x.position.Y).OrderBy(x => x.Key))
                //            foreach (var group in groupy.GroupBy(x => x.material))
                //                renderItem(group.Key, group.OrderBy(x => x.texture.id), depthRange, m);
                //    break;
            }

            renderableIndex = 0;
            minDepth = 0f;
            maxDepth = 0f;
            spritesSubmittedThisBatch = 0;
            //crap.Clear();
        }

        private void renderItem(int start, int length)
        {
            for (var i = start; i < start + length; i++)
            //foreach (var item in items)
            {
                var item = renderableItems[i];
                //if (item.material.textureName == "content\\fonts\\arial_0.png")
                {
                    Root.instance.graphics.spritesSubmittedThisFrame++;
                    updateBatchState(item);

                    var texture = item.material.texture ?? Root.instance.resources.basewhite.texture;

                    Rectangle srcRectangle;
                    if (item.sourceRectangle.IsNull)
                        srcRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
                    else
                        srcRectangle = item.sourceRectangle.ToRect();

                    var origin = (new Vector2(srcRectangle.Width, srcRectangle.Height) * item.origin);// +new Vector2(srcRectangle.Value.X, srcRectangle.Value.Y);

                    var p = item.position;
                    var s = item.scale;
                    var destRectangle = p.ToRectangle(s);

                    var depth = 0f;
                    if (depthRange > 0f)
                        depth = (item.depth - minDepth) / depthRange;

                    batch.Draw(texture, destRectangle, srcRectangle, item.color, item.rotation, origin, item.effect, 1f - depth);
                    spritesSubmittedThisBatch++;
                }
            }

            endBatchState();
        }

        private void renderItem(IEnumerable<DrawItem> items)
        {
            foreach (var item in items)
            {
                if (item.material.textureName == "content\\fonts\\arial_0.png")
                {
                    Root.instance.graphics.spritesSubmittedThisFrame++;
                    updateBatchState(item);

                    var texture = item.material.texture ?? Root.instance.resources.basewhite.texture;

                    Rectangle srcRectangle;
                    if (item.sourceRectangle.IsNull)
                        srcRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
                    else
                        srcRectangle = item.sourceRectangle.ToRect();

                    var origin = (new Vector2(srcRectangle.Width, srcRectangle.Height) * item.origin);// +new Vector2(srcRectangle.Value.X, srcRectangle.Value.Y);

                    var p = item.position;
                    var s = item.scale;
                    var destRectangle = p.ToRectangle(s);

                    var depth = 0f;
                    if (depthRange > 0f)
                        depth = (item.depth - minDepth) / depthRange;

                    batch.Draw(texture, destRectangle, srcRectangle, item.color, item.rotation, origin, item.effect, 1f - depth);
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
                    batch.GraphicsDevice.ScissorRectangle = new Rectangle(0, 0, 800, 480);
                    applyingScissor = false;
                }

                Root.instance.graphics.drawCallsThisFrame++;
                materialState = null;
            }
        }

        //private HashSet<string> crap = new HashSet<string>();
        internal void setBatchState(DrawItem item)
        {
            //if (materialState != null && materialState.textureName == item.material.textureName)
            //    materialState = item.material;

            endBatchState();


            //if (!crap.Add(item.material.textureName))
            //    materialState = item.material;

            materialState = item.material;
            var blendState = item.material.BlendState;
            var depthState = item.material.DepthStencilState;
            var rasterizerState = item.applyScissor ? item.material.ScissorRasterizerState : item.material.RasterizerState;
            var samplerState = item.material.SamplerState;

            if (item.applyScissor)
            {
                batch.GraphicsDevice.ScissorRectangle = item.scissorRect;
                applyingScissor = true;
            }

            batch.Begin(SpriteSortMode.Deferred, blendState, samplerState, depthState, rasterizerState, materialState.effect, currentMatrix);
        }

        internal void updateBatchState(DrawItem item)
        {
            if (materialState == null)
                setBatchState(item);
            else if (materialState != item.material)
                setBatchState(item);
            else if (item.applyScissor != applyingScissor)
                setBatchState(item);
            else if (item.applyScissor && applyingScissor && item.scissorRect != batch.GraphicsDevice.ScissorRectangle)
                setBatchState(item);
            else if ((spritesSubmittedThisBatch % 2048) == 0)
                setBatchState(item);
        }
    }

}
