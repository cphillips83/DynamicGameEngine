using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Space
{
    public class Map : Script
    {
        private Settings _settings;
        private PlayerController _player;
        private TargetableObjects _targets;

        private void init()
        {
            _settings = rootObject.getScript<Settings>();
            _player = rootObject.getScriptWithChildren<PlayerController>();
            _targets = rootObject.getScript<TargetableObjects>();
        }

        private void ongui()
        {
            var aabb = AxisAlignedBox.FromDimensions(new Vector2(0, 0), new Vector2(256, 256));

            //var p = _player.transform.DerivedPosition;
            var worldAABB = mainCamera.worldBounds;
            var p0 = worldAABB.Minimum;
            var p1 = worldAABB.Maximum;
            var scale = 128f / _settings.SectorHalfSize;


            p0.X = Utility.Clamp(p0.X, _settings.SectorHalfSize, -_settings.SectorHalfSize) * scale;
            p0.Y = Utility.Clamp(p0.Y, _settings.SectorHalfSize, -_settings.SectorHalfSize) * scale;
            p1.X = Utility.Clamp(p1.X, _settings.SectorHalfSize, -_settings.SectorHalfSize) * scale;
            p1.Y = Utility.Clamp(p1.Y, _settings.SectorHalfSize, -_settings.SectorHalfSize) * scale;

            worldAABB.SetExtents(p0, p1);
            worldAABB.Center = worldAABB.Center;

            aabb.Center += transform.DerivedPosition;
            worldAABB.Center += transform.DerivedPosition;

            graphics.Draw(aabb, new Color(Color.Black, 0.5f));
            graphics.DrawRect(aabb, new Color(Color.Green, 0.5f));

            var aliens = _targets.get(Vector2.Zero, _settings.SectorSize, TargetFilters.Aliens);
            foreach (var alien in aliens)
            {
                var p = alien.transform.DerivedPosition * scale;
                graphics.DrawRect(AxisAlignedBox.FromDimensions(p + transform.DerivedPosition, Vector2.One * 2), new Color(Color.Red, 0.5f));
            }

            graphics.DrawRect(worldAABB, new Color(Color.White, 0.5f));

        }
    }
}
