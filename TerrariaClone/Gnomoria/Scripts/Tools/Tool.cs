using GameName1.Gnomoria.Scripts.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Gnomoria.Scripts.Tools
{
    public abstract class Tool : Script
    {
        protected WorldManager world;
        protected WorldRenderer renderer;
        protected ToolManager tm;

        public string toolName;

        public Tool(string name)
        {
            this.toolName = name;
        }

        private void init()
        {
            world = rootObject.getScript<WorldManager>();
            renderer = rootObject.getScript<WorldRenderer>();
            tm = rootObject.find("tools").getScript<ToolManager>();
            tm.registerTool(this);
            oninit();
        }

        private void update()
        {
            if (tm.currentTool == toolName)
                onupdate();
        }

        private void render()
        {
            if (tm.currentTool == toolName)
                onrender();
        }

        protected virtual void oninit()
        {

        }

        protected virtual void onupdate()
        {

        }

        protected virtual void onrender()
        {

        }

        public virtual void renderFloor(MapTile tile, int x, int y, int z)
        {
            if (tile.hasFloor)
                renderer.RenderObject(tile.floorType, x, y, z, 1f);


        }

        public virtual void renderPlaceable(MapTile tile, int x, int y, int z)
        {
            if (tile.hasPlaceable)
            {
                var po = world.placableObjects[tile.placedId];
                if (po != null)
                    po.render(renderer, tile, x, y, z, 1f);
            }
        }

        public virtual void doGui()
        {

        }

        public virtual void startRendering() { }
        public virtual void endRendering() { }
    }
}
