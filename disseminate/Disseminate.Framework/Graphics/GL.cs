using Disseminate.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.Graphics
{
    public abstract partial class GL
    {
        private bool _begin = false;

        protected Stack<GLState> _stateStack = new Stack<GLState>();
        protected Stack<AxisAlignedBox2> _clipStack = new Stack<AxisAlignedBox2>();
        protected Stack<IEffect> _effectStack = new Stack<IEffect>();

        protected GLState _state;
        protected AxisAlignedBox2 _clipping;
        protected IEffect _effect;

        protected int _renderableIndex = 0;
        protected GLSortMode _sortMode = GLSortMode.Material;
        protected GLRenderable[] _renderableItems = new GLRenderable[1024];
        protected IRenderTarget _target;

        public Real currentRotation { get { return _state.rotation; } }
        public Vector2 currentPosition { get { return _state.position; } }

        protected virtual void verifyBegin()
        {
            if (!_begin)
                throw new InvalidOperationException("You must call begin first.");
        }

        protected virtual void draw(GLRenderable item)
        {
            verifyBegin();
            if (_renderableIndex == _renderableItems.Length)
                Array.Resize(ref _renderableItems, _renderableItems.Length * 3 / 2);

            if (item.material == null)
                item.material = defaultMaterial;

            if (item.texture == null)
                item.texture = defaultTexture;

            //HACK: item.key = item.material.getSortKey(_sortMode);
            item.key = item.material.id;
            _renderableItems[_renderableIndex++] = item;
        }

        protected internal abstract GLMaterial defaultMaterial { get; }
        protected internal abstract GLTexture2D defaultTexture { get; }

        //primitives
        public virtual void begin(IRenderTarget target, GLSortMode mode)
        {
            if (_begin)
                throw new InvalidOperationException("You must call end before you can call begin again.");

            if (_stateStack.Count > 0)
                throw new InvalidOperationException("State stack was not empty.");

            if (_clipStack.Count > 0)
                throw new InvalidOperationException("Clipping stack was not empty.");

            if (_effectStack.Count > 0)
                throw new InvalidOperationException("Effect stack was not empty.");

            _target = target;
            _begin = true;
            _sortMode = mode;
            _renderableIndex = 0;
            _clipping = AxisAlignedBox2.Null;
            _effect = null;
            //_currentMatrix = Matrix3.Identity;
            _state = GLState.empty;

            _target.enable();
            //check other settings
        }

        public void end()
        {
            verifyBegin();

            if (_stateStack.Count > 0)
                throw new InvalidOperationException("State stack was not empty.");

            if (_clipStack.Count > 0)
                throw new InvalidOperationException("Clipping stack was not empty.");

            if (_effectStack.Count > 0)
                throw new InvalidOperationException("Effect stack was not empty.");

            _begin = false;
            if (_sortMode != GLSortMode.None)
                Utility.RadixSort(_renderableItems, _renderableIndex);

            onend(_renderableItems, _renderableIndex);
        }

        protected abstract void onend(GLRenderable[] items, int length);

        public void quad(Vector2 size) { quad(-size / 2f, size / 2f, new Vector2(0.5f, 0.5f), _state.rotation); }
        public void quad(AxisAlignedBox2 a) { quad(a.minVector, a.maxVector, new Vector2(0.5f, 0.5f), _state.rotation); }
        public void quad(Vector2 size, Real rotation) { quad(-size / 2f, size / 2f, new Vector2(0.5f, 0.5f), rotation); }
        public void quad(Vector2 min, Vector2 max, Real rotation) { quad(min, max, new Vector2(0.5f, 0.5f), rotation); }
        public void quad(AxisAlignedBox2 a, Real rotation) { quad(a.minVector, a.maxVector, new Vector2(0.5f, 0.5f), rotation); }
        public void quad(AxisAlignedBox2 a, Vector2 pivot, Real rotation) { quad(a.minVector, a.maxVector, pivot, rotation); }
        public void quad(Vector2 min, Vector2 max) { quad(min, max, new Vector2(0.5f, 0.5f), _state.rotation); }

        public virtual void quad(Vector2 min, Vector2 max, Vector2 pivot, Real rotation)
        {
            var item = new GLRenderable();
            item.color = _state.color;
            item.depth = _state.depth;
            item.material = _state.material;
            item.texture = _state.texture;
            item.pivot = pivot;
            item.scale = (max - min) * _state.scale;
            item.position = (_state.position + min) * _state.scale;
            item.rotation = rotation;
            item.scissorRect = _clipping;
            item.applyScissor = !_clipping.IsNull;
            item.sourceRectangle = _state.source;
            item.effect = _effect;
            item.type = GLRenderableType.Quad;
            item.key = 0;

            draw(item);
        }

        public void line(Vector2 p0) { line(_state.position, p0, 1f, _state.rotation); }
        public void line(Vector2 p0, Real width) { line(_state.position, p0, width, _state.rotation); }
        public void line(Vector2 p0, Vector2 p1) { line(p0, p1, 1f, _state.rotation); }
        public void line(Vector2 p0, Vector2 p1, Real width) { line(p0, p1, width, _state.rotation); }
        public void line(Vector2 start, Vector2 end, Real width, Real rotation)
        {
            if (rotation != 0)
            {
                start = start.Rotate(Vector2.Zero, rotation);
                end = end.Rotate(Vector2.Zero, rotation);
            }

            var diff = end - start;
            //var srcRect = new AxisAlignedBox(start.X, start.Y - width * 0.5f, start.X + diff.Length(), start.Y + width * 0.5f);

            var srcRect = new AxisAlignedBox2(start.X, start.Y, start.X + width, start.Y + diff.Length);

            diff.Normalize();

            var _rotation = Utility.ATan2(diff.Y, diff.X) - Utility.PIOverTwo;
            //Draw(renderQueue, material, srcRect, AxisAlignedBox2.Null, color, rotation, new Vector2(0.5f, 0), SpriteEffects.None, depth);

            var item = new GLRenderable();
            item.color = _state.color;
            item.depth = _state.depth;
            item.material = _state.material;
            item.pivot = new Vector2(0.5f, 0);
            item.position = (_state.position + start) * _state.scale;
            item.rotation = _rotation;
            item.scale = srcRect.Size * _state.scale;
            item.scissorRect = _clipping;
            item.applyScissor = !_clipping.IsNull;
            item.sourceRectangle = _state.source;
            item.effect = _effect;
            item.type = GLRenderableType.Line;
            item.key = 0;

            draw(item);
        }

        //public void rect(Vector2 size) { rect(Vector2.Zero, size, 1f, _state.rotation); }
        public void rect(AxisAlignedBox2 a) { rect(a.minVector, a.maxVector, 1f, _state.rotation); }
        //public void rect(Vector2 size, Real width) { rect(Vector2.Zero, size, width, _state.rotation); }
        public void rect(AxisAlignedBox2 a, Real width) { rect(a.minVector, a.maxVector, width, _state.rotation); }
        public void rect(Vector2 min, Vector2 max) { rect(min, max, 1f, _state.rotation); }
        public void rect(Vector2 min, Vector2 max, Real width) { rect(min, max, width, _state.rotation); }
        public virtual void rect(Vector2 min, Vector2 max, Real width, Real rotation)
        {
            min = new Vector2(System.Math.Floor(min.X), System.Math.Floor(min.Y));
            max = new Vector2(System.Math.Ceiling(max.X), System.Math.Ceiling(max.Y));

            var points = new Vector2[] { min, new Vector2(max.X, min.Y), max, new Vector2(min.X, max.Y) };
            if (rotation != 0)
            {
                for (var i = 0; i < 4; i++)
                    points[i] = points[i].Rotate(Vector2.Zero, rotation);
            }

            for (var i = 0; i < 4; i++)
                line(points[i], points[(i + 1) % 4], width, 0f);
        }

        //transforms
        public void position(Real x, Real y) { position(new Vector2(x, y)); }
        public virtual void position(Vector2 point)
        {
            verifyBegin();
            _state.position = point;
        }

        public virtual void rotation(Real amount)
        {
            verifyBegin();
            _state.rotation = Utility.WrapAngle(amount);
        }

        public virtual void rotate(Real amount)
        {
            verifyBegin();
            _state.rotation = Utility.WrapAngle(_state.rotation + amount);
            //_state.rotation = _state.rotation + amount;
        }

        public void translate(Real x, Real y) { translate(new Vector2(x, y)); }
        public virtual void translate(Vector2 p)
        {
            verifyBegin();
            p = p.Rotate(Vector2.Zero, _state.rotation);
            _state.position += p;
        }

        public void scale(Real amount) { scale(new Vector2(amount, amount)); }
        public virtual void scale(Vector2 amount)
        {
            verifyBegin();
            _state.scale = amount;
        }

        public virtual void depth(Real amount)
        {
            _state.depth = amount;
        }

        public virtual void push()
        {
            verifyBegin();
            _stateStack.Push(_state);
        }
        //public void push(Matrix4 matrix) { }
        public virtual void pop()
        {
            verifyBegin();

            if (_stateStack.Count == 0)
                throw new InvalidOperationException("State stack was empty.");

            _state = _stateStack.Pop();
        }

        //coloring
        public void color(Real r, Real g, Real b) { color(new Color(r, g, b, 1f)); }
        public void color(Real r, Real g, Real b, Real a) { color(new Color(r, g, b, a)); }
        public void color(Vector3 _color) { color(new Color(_color.X, _color.Y, _color.Z, 1f)); }
        public void color(Vector4 _color) { color(new Color(_color.x, _color.y, _color.z, _color.w)); }
        public virtual void color(Color _color)
        {
            verifyBegin();
            _state.color = _color;
        }

        public virtual void material(GLMaterial material)
        {
            verifyBegin();
            _state.material = material;
        }

        public virtual void texture(GLTexture2D texture)
        {
            verifyBegin();
            _state.texture = texture;
        }

        public void source(Vector2 min, Vector2 max) { source(new AxisAlignedBox2(min, max)); }
        public virtual void source(AxisAlignedBox2 src)
        {
            verifyBegin();
            _state.source = src;
        }

        public void beginclip(Vector2 size) { beginclip(new AxisAlignedBox2(Vector2.Zero, size)); }
        public void beginclip(Vector2 min, Vector2 max) { beginclip(new AxisAlignedBox2(min, max)); }
        public virtual void beginclip(AxisAlignedBox2 clipregion)
        {
            verifyBegin();
            _clipStack.Push(_clipping);
            _clipping = clipregion;
        }

        public virtual void endclip()
        {
            verifyBegin();

            if (_clipStack.Count == 0)
                throw new InvalidOperationException("Clipping stack was already empty.");

            _clipping = _clipStack.Pop();
        }

        public virtual void begineffect(IEffect effect)
        {
            verifyBegin();
            _effectStack.Push(_effect);
            _effect = effect;
        }

        public virtual void endeffect()
        {
            verifyBegin();

            if (_effectStack.Count == 0)
                throw new InvalidOperationException("Effect stack was already empty.");

            _effect = _effectStack.Pop();
        }
        //buffer

        public void clear(Color color)
        {
            verifyBegin();
            onclear(color);
        }

        protected abstract void onclear(Color color);

    }
}
