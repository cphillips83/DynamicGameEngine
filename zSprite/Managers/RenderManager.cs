using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite.Collections;
using zSprite.Resources;
using System.Diagnostics;

namespace zSprite
{
    public enum RenderQueueSortMode : int
    {
        Material = 0,
        PreserverOrder = 1,
        ReverseOrder = 2,
        Depth = 3,

        //NegativeZ,
        Y = 4,

        //NegativeY,
        //X,
        //NegativeX,
        YThenDepth = 5,

        DepthThenY = 6,
        //Texture,
    }
}

namespace zSprite.Managers
{
    #region DrawItem

    internal struct DrawItem : IRadixKey
    {
        //public long id;
        public int key;
        public float depth;
        public float rotation;
        public Vector2 origin;
        public Vector2 position;
        public Vector2 scale;
        public Color color;
        public Rectangle scissorRect;
        public AxisAlignedBox sourceRectangle;
        public Material material;
        public SpriteEffects effect;
        public bool applyScissor;

        public int Key { get { return key; } }
        //public TextureRef texture;
    }

    #endregion DrawItem

    #region Sorting helpers

    internal static class SortHelpers
    {
        internal static IEnumerable<DrawItem> byDepth(this IEnumerable<DrawItem> items)
        {
            return items.OrderBy(x => x.depth);
        }

        internal static IEnumerable<DrawItem> byDepthDesc(this IEnumerable<DrawItem> items)
        {
            return items.OrderByDescending(x => x.depth);
        }

        internal static IEnumerable<IGrouping<Material, DrawItem>> byMaterial(this IEnumerable<DrawItem> items)
        {
            return items.GroupBy(x => x.material);
        }

        internal static IEnumerable<DrawItem> byTexture(this IEnumerable<DrawItem> items)
        {
            return items.OrderBy(x => x.material.texture);
        }

        internal static IEnumerable<DrawItem> byY(this IEnumerable<DrawItem> items)
        {
            return items.OrderBy(x => x.position.Y);
        }

        internal static IEnumerable<DrawItem> byYDesc(this IEnumerable<DrawItem> items)
        {
            return items.OrderByDescending(x => x.position.Y);
        }
    }

    #endregion Sorting helpers

    public sealed class RenderManager : AbstractManager
    {
        internal int drawCallsThisFrame = 0;
        internal GraphicsDevice graphicsDevice;

        internal int spritesSubmittedThisFrame = 0;
        private MonoRenderer[] queues = new MonoRenderer[20];
        private Stack<int> renderQueueStack = new Stack<int>();
        private Stopwatch timer = new Stopwatch();
        public long lastFrameRender = 0;
        public long totalFrameRender = 0;

        public int currentRenderQueue = 9;

        internal RenderManager()
        {
        }

        public void pushRenderQueue(int queue)
        {
            renderQueueStack.Push(currentRenderQueue);
            currentRenderQueue = queue;
        }

        public void popRenderQueue()
        {
            currentRenderQueue = renderQueueStack.Pop();
        }

        public void setSortOrder(int queue, RenderQueueSortMode sortMode)
        {
            queues[queue].sortMode = sortMode;
        }

        public int drawCalls { get; internal set; }

        public bool scissorEnabled { get; private set; }

        public Rectangle scissorRect { get; private set; }

        public int spritesSubmitted { get; internal set; }

        public void beginScissor(AxisAlignedBox rect)
        {
            beginScissor(rect.ToRect());
        }

        public void beginScissor(Rectangle rect)
        {
            rect.Width = rect.Width / 2;
            scissorEnabled = true;
            scissorRect = rect;
        }

        public void Draw(Material material, Vector2 position, AxisAlignedBox sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
        {
            queues[currentRenderQueue].DrawInternal(material,
                position,
                scale,
                sourceRectangle,
                color,
                rotation,
                origin,
                effect,
                depth);
        }

        public void Draw(Material material, Vector2 position, AxisAlignedBox sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, float depth)
        {
            queues[currentRenderQueue].DrawInternal(material,
                position,
                (new Vector2(scale)),
                sourceRectangle,
                color,
                rotation,
                origin,
                effect,
                depth);
        }

        public void Draw(Material material, AxisAlignedBox destinationRectangle, AxisAlignedBox sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effect, float depth)
        {
            queues[currentRenderQueue].DrawInternal(material,
                  (new Vector2(destinationRectangle.X0, destinationRectangle.Y0)),
                  (new Vector2(destinationRectangle.Width, destinationRectangle.Height)),
                  sourceRectangle,
                  color,
                  rotation,
                  origin,
                  effect,
                  depth);
        }

        public void Draw(Material material, Vector2 position, AxisAlignedBox sourceRectangle, Color color)
        {
            Draw(currentRenderQueue, material, position, sourceRectangle, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void Draw(Material material, AxisAlignedBox destinationRectangle, AxisAlignedBox sourceRectangle, Color color)
        {
            Draw(currentRenderQueue, material, destinationRectangle, sourceRectangle, color, 0, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void Draw(Material material, AxisAlignedBox destinationRectangle, AxisAlignedBox sourceRectangle, Color color, float depth)
        {
            Draw(currentRenderQueue, material, destinationRectangle, sourceRectangle, color, 0, Vector2.Zero, SpriteEffects.None, depth);
        }

        public void Draw(Material material, Vector2 position, Color color)
        {
            Draw(currentRenderQueue, material, position, AxisAlignedBox.Null, color);
        }

        public void Draw(Material material, Vector2 position, float rotation, Vector2 scale, Color color)
        {
            Draw(currentRenderQueue, material, position, AxisAlignedBox.Null, color, rotation, Vector2.One / 2f, scale, SpriteEffects.None, 0f);
        }

        public void Draw(Material material, AxisAlignedBox rectangle, Color color)
        {
            Draw(currentRenderQueue, material, rectangle, AxisAlignedBox.Null, color);
        }

        //public void Draw(Material material, AxisAlignedBox source, AxisAlignedBox rectangle, Color color)
        //{
        //    Draw(currentRenderQueue, material, rectangle, source, color);
        //}

        public void Draw(Material material, AxisAlignedBox rectangle, Vector2 origin, Color color)
        {
            Draw(currentRenderQueue, material, rectangle, AxisAlignedBox.Null, color, 0, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void Draw(Material material, AxisAlignedBox rectangle, Color color, float depth)
        {
            Draw(currentRenderQueue, material, rectangle, AxisAlignedBox.Null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        }

        public void Draw(Material material, AxisAlignedBox rectangle, Color color, SpriteEffects effects, float depth)
        {
            Draw(currentRenderQueue, material, rectangle, AxisAlignedBox.Null, color, 0f, Vector2.Zero, effects, depth);
        }

        public void Draw(int renderQueue, Material material, Vector2 position, AxisAlignedBox sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
        {
            queues[renderQueue].DrawInternal(material,
                position,
                scale,
                sourceRectangle,
                color,
                rotation,
                origin,
                effect,
                depth);
        }

        public void Draw(int renderQueue, Material material, Vector2 position, AxisAlignedBox sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, float depth)
        {
            queues[renderQueue].DrawInternal(material,
                position,
                (new Vector2(scale)),
                sourceRectangle,
                color,
                rotation,
                origin,
                effect,
                depth);
        }

        public void Draw(int renderQueue, Material material, AxisAlignedBox destinationRectangle, AxisAlignedBox sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effect, float depth)
        {
            queues[renderQueue].DrawInternal(material,
                  (new Vector2(destinationRectangle.X0, destinationRectangle.Y0)),
                  (new Vector2(destinationRectangle.Width, destinationRectangle.Height)),
                  sourceRectangle,
                  color,
                  rotation,
                  origin,
                  effect,
                  depth);
        }

        public void Draw(int renderQueue, Material material, Vector2 position, AxisAlignedBox sourceRectangle, Color color)
        {
            Draw(renderQueue, material, position, sourceRectangle, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void Draw(int renderQueue, Material material, AxisAlignedBox destinationRectangle, AxisAlignedBox sourceRectangle, Color color)
        {
            Draw(renderQueue, material, destinationRectangle, sourceRectangle, color, 0, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void Draw(int renderQueue, Material material, AxisAlignedBox destinationRectangle, AxisAlignedBox sourceRectangle, Color color, float depth)
        {
            Draw(renderQueue, material, destinationRectangle, sourceRectangle, color, 0, Vector2.Zero, SpriteEffects.None, depth);
        }

        public void Draw(int renderQueue, Material material, Vector2 position, Color color)
        {
            Draw(renderQueue, material, position, AxisAlignedBox.Null, color);
        }

        public void Draw(int renderQueue, Material material, AxisAlignedBox rectangle, Color color)
        {
            Draw(renderQueue, material, rectangle, AxisAlignedBox.Null, color);
        }

        public void Draw(int renderQueue, Material material, AxisAlignedBox rectangle, Color color, float depth)
        {
            Draw(renderQueue, material, rectangle, AxisAlignedBox.Null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        }

        public void Draw(int renderQueue, AxisAlignedBox rectangle, Color color, float depth)
        {
            Draw(renderQueue, null, rectangle, AxisAlignedBox.Null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        }

        public void Draw(AxisAlignedBox rectangle, Color color)
        {
            Draw(currentRenderQueue, null, rectangle, AxisAlignedBox.Null, color, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public void Draw(AxisAlignedBox rectangle, Color color, float depth)
        {
            Draw(currentRenderQueue, null, rectangle, AxisAlignedBox.Null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        }

        public void Draw(int renderQueue, Material material, AxisAlignedBox rectangle, Color color, SpriteEffects effects, float depth)
        {
            Draw(renderQueue, material, rectangle, AxisAlignedBox.Null, color, 0f, Vector2.Zero, effects, depth);
        }

        public void DrawCircle(Material material, Vector2 center, float radius, int segments, Color color)
        {
            DrawCircle(currentRenderQueue, material, center, radius, segments, color);
        }

        public void DrawLine(Vector2 start, Vector2 end, Color color)
        {
            DrawLine(currentRenderQueue, null, start, end, color, 1f, 0);
        }

        public void DrawLine(Material material, Vector2 start, Vector2 end, Color color)
        {
            DrawLine(currentRenderQueue, material, start, end, color, 1f, 0);
        }

        public void DrawLine(Material material, Vector2 start, Vector2 end, Color color, float depth)
        {
            DrawLine(material, start, end, color, depth);
        }

        public void DrawLine(Vector2 start, Vector2 end, Color color, float depth)
        {
            DrawLine(null, start, end, color, depth);
        }

        public void DrawRect(Material material, AxisAlignedBox rect, Color color)
        {
            DrawRect(currentRenderQueue, material, rect.minVector, rect.maxVector, color, 0f);
        }

        public void DrawRect(Material material, AxisAlignedBox rect, Color color, float depth)
        {
            DrawRect(currentRenderQueue, material, rect.minVector, rect.maxVector, color, depth);
        }

        public void DrawRect(Material material, Vector2 p0, Vector2 p1, Color color)
        {
            DrawRect(currentRenderQueue, material, p0, p1, color, 0f);
        }

        public void DrawRect(Material material, Vector2 p0, Vector2 p1, Color color, float depth)
        {
            DrawRect(currentRenderQueue, material, p0, p1, color, depth);
        }

        public void DrawShape(Material material, Shape shape, Color color)
        {
            DrawShape(currentRenderQueue, material, shape, color);
        }

        public void DrawText(BmFont font, float scale, Vector2 pos, string text, Color color)
        {
            DrawText(currentRenderQueue, font, scale, pos, text, color, 0f);
        }

        public void DrawText(BmFont font, float scale, Vector2 pos, string text, Color color, float depth)
        {
            font.DrawText(currentRenderQueue, pos, scale, text, color, depth);
        }

        public void DrawCircle(int renderQueue, Material material, Vector2 center, float radius, int segments, Color color)
        {
            var step = MathHelper.TwoPi / segments;

            var lp = new Vector2(Utility.Cos(0), Utility.Sin(0)) * radius + center;
            var p = Vector2.Zero;
            for (var i = 1; i <= segments; i++)
            {
                var current = step * i;
                p.X = Utility.Cos(current) * radius + center.X;
                p.Y = Utility.Sin(current) * radius + center.Y;

                DrawLine(renderQueue, material, lp, p, color);

                lp = p;
            }
        }

        public void DrawLine(int renderQueue, Material material, Vector2 start, Vector2 end, Color color)
        {
            DrawLine(renderQueue, material, start, end, color, 1f, 0);
        }

        public void DrawLine(int renderQueue, Material material, Vector2 start, Vector2 end, Color color, float width, float depth)
        {
            //end.Y = -end.Y;
            //start.Y = -start.Y;
            var diff = end - start;
            //var srcRect = new AxisAlignedBox(start.X, start.Y - width * 0.5f, start.X + diff.Length(), start.Y + width * 0.5f);
            var srcRect = new AxisAlignedBox(start.X, start.Y, start.X + width, start.Y + diff.Length());

            diff.Normalize();

            var rotation = (float)Math.Atan2(diff.Y, diff.X) - MathHelper.PiOver2;
            Draw(renderQueue, material, srcRect, AxisAlignedBox.Null, color, rotation, new Vector2(0.5f, 0), SpriteEffects.None, depth);
        }

        public void DrawRect(int renderQueue, Material material, AxisAlignedBox rect, Color color)
        {
            DrawRect(renderQueue, material, rect.minVector, rect.maxVector, color, 0f);
        }

        public void DrawRect(int renderQueue, AxisAlignedBox rect, Color color)
        {
            DrawRect(renderQueue, null, rect.minVector, rect.maxVector, color, 0f);
        }

        public void DrawRect(AxisAlignedBox rect, Color color)
        {
            DrawRect(currentRenderQueue, null, rect.minVector, rect.maxVector, color, 0f);
        }

        public void DrawRect(int renderQueue, Material material, AxisAlignedBox rect, Color color, float depth)
        {
            DrawRect(renderQueue, material, rect.minVector, rect.maxVector, color, depth);
        }

        public void DrawRect(int renderQueue, Material material, Vector2 p0, Vector2 p1, Color color)
        {
            DrawRect(renderQueue, material, p0, p1, color, 0f);
        }

        public void DrawRect(int renderQueue, Vector2 p0, Vector2 p1, Color color)
        {
            DrawRect(renderQueue, null, p0, p1, color, 0f);
        }

        public void DrawRect(int renderQueue, Material material, Vector2 p0, Vector2 p1, Color color, float depth)
        {
            var points = new Vector2[4];
            points[0] = p0.Floor();
            points[2] = p1.Floor();
            points[1] = new Vector2(points[2].X, points[0].Y);
            points[3] = new Vector2(points[0].X, points[2].Y);

            for (var i = 0; i < points.Length; i++)
                DrawLine(renderQueue, material, points[i], points[(i + 1) % points.Length], color, 1f, depth);
        }

        public void DrawShape(int renderQueue, Material material, Shape shape, Color color)
        {
            var points = shape.derivedVertices;
            for (var i = 0; i < points.Length; i++)
                DrawLine(renderQueue, material, points[i], points[(i + 1) % points.Length], color);
        }

        public void DrawText(int renderQueue, BmFont font, float scale, Vector2 pos, string text, Color color)
        {
            DrawText(renderQueue, font, scale, pos, text, color, 0f);
        }

        public void DrawText(int renderQueue, BmFont font, float scale, Vector2 pos, string text, Color color, float depth)
        {
            font.DrawText(renderQueue, pos, scale, text, color, depth);
        }

        public void endScissor()
        {
            scissorRect = new Rectangle();
            scissorEnabled = false;
        }

        public void render(Matrix m, RenderTarget2D target, Viewport viewport)
        {
            lastFrameRender = 0;
            timer.Reset();

            timer.Start();
            for (var i = 0; i < queues.Length; i++)
                queues[i].Render(m, target, viewport);
            timer.Stop();
            lastFrameRender = timer.ElapsedMilliseconds;
            totalFrameRender += lastFrameRender;
        }

        internal void init(GraphicsDevice device)
        {
            for (var i = 0; i < queues.Length; i++)
                queues[i] = new MonoRenderer(device);

            graphicsDevice = device;
        }

    }
}