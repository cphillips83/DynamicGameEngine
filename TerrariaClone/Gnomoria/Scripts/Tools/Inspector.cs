using GameName1.Gnomoria.Scripts.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Gnomoria.Scripts.Tools
{
    public class Inspector : Tool
    {
        public Inspector()
            : base("inspector")
        {

        }

        //private Vector3i tilePosition;
        // private bool rendergui = false;
        private ScreenToTileRay ray;
        private WorldSprite floorSelection;
        private WorldSprite wallSelection;

        protected override void oninit()
        {
            floorSelection = renderer.spriteSheet["floorselection"];
            wallSelection = renderer.spriteSheet["wallselection"];
        }

        public override void startRendering()
        {
            ray = world.findMouseFloorTile();
            
            //var mp = input.MousePosition;
            //var wp = mainCamera.screenToWorld(mp);
            //ray = renderer.screenToTile(wp);
        }

        public override void renderFloor(MapTile tile, int x, int y, int z)
        {
            base.renderFloor(tile, x, y, z);
            if (ray.hit && ray.objectType == ScreenToTileRayObject.Floor && ray.iso == new Vector3i(x, y, z))
                renderer.queueRender(floorSelection, x, y, z, Color.Yellow);
        }

        public override void renderPlaceable(MapTile tile, int x, int y, int z)
        {
            base.renderPlaceable(tile, x, y, z);
            if (ray.hit && ray.objectType == ScreenToTileRayObject.Placable && ray.iso == new Vector3i(x, y, z))
                renderer.queueRender(wallSelection, x, y, z, Color.Yellow);
        }

        public override void doGui()
        {
            var mp = input.MousePosition;
            var wp = mainCamera.screenToWorld(mp);
            var p = world.worldToIso(wp);
            var mouse = input.MousePosition;
            var gp = gui.screenToGUI(mouse);

            if (ray.hit)
            {
                //var wp = world.isoToWorld(mp.X, mp.Y, world.currentMapDepth) + new Vector2(0, world.currentMapDepth * (world.blockSizeOver2 + world.floorSizeOver2));
                var content = new GUIContent(string.Format("mx:{4}, my:{5}, x:{0}, y:{1}, z:{2}, type:{3}", ray.iso.x, ray.iso.y, ray.iso.z, ray.objectType, p.X, p.Y));
                var size = gui.skin.box.CalcSize(content, 100);
                gui.box(AxisAlignedBox.FromRect(gp, size), content);
            }
            else
            {
                var content = new GUIContent(string.Format("mx:{0}, my:{1}", p.X, p.Y));
                var size = gui.skin.box.CalcSize(content, 100);
                gui.box(AxisAlignedBox.FromRect(gp, size), content);           
            }
        }
    }
}
