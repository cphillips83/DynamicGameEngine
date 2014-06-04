using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite;

namespace GameName1.Space
{
    public class LookAtMouse : Script
    {
        private Transform _transform;

        private void init()
        {
            _transform = this.gameObject.transform2();
        }

        private void update()
        {
            if (_transform != null)
            {
                var p = Camera.mainCamera.screenToWorld(Root.instance.input.MousePosition);
                _transform.LookAt(p, TransformSpace.World);

                //if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                //    _transform.Translate(Vector2.UnitX * Root.instance.time.deltaTime * 25, TransformSpace.Parent);
                //if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                //    _transform.Translate(-Vector2.UnitX * Root.instance.time.deltaTime * 25, TransformSpace.Parent);
            }
        }

        //private void render()
        //{
        //    if (_transform != null)
        //    {
        //        var font = Fonts.find("fonts/arial.fnt");

        //        //var mp = Root.instance.input.MousePosition;
        //        //Root.instance.graphics.DrawText(0, font, 1f, 40, 20, string.Format("X: {0}, Y: {1}", mp.X, mp.Y), Color.White);
                
        //        //var p = Camera.mainCamera.screenToWorld(Root.instance.input.MousePosition);
        //        //Root.instance.graphics.DrawText(0, font, 1f, 40, 40, string.Format("X: {0}, Y: {1}", p.X, p.Y), Color.White);
        //    }
        //}
    }

    public class LookAt : Script
    {
        private Transform _transform;

        public int gameObjectId = -1;

        private void init()
        {
            _transform = this.gameObject.transform2();
        }

        private void update()
        {
            if (_transform != null)
            {
                var go = Root.instance.find(gameObjectId);
                if (go != null)
                {
                    var _other = go.transform2();
                    if (_other != null)
                        _transform.LookAt(_other.DerivedPosition, TransformSpace.World);
                }
            }
        }
    }
}
