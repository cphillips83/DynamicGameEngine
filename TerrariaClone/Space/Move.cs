using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Space
{
    public class Move : Script
    {
        private Transform _transform;
        private float speed = 150f;

        private void init()
        {
            _transform = this.gameObject.transform2();
        }

        private void render()
        {
            if (_transform != null)
            {
                var font = Root.instance.resources.findFont("fonts/arial.fnt");

                var p = Camera.mainCamera.screenToWorld(Root.instance.input.MousePosition);
                var r = _transform.GetRotationTo(p, TransformSpace.Local);
                Root.instance.graphics.DrawText(0, font, 1f, new Vector2(40, 20), r.ToString(), Color.White);

                //var p = Camera.mainCamera.screenToWorld(Root.instance.input.MousePosition);
                //Root.instance.graphics.DrawText(0, font, 1f, 40, 40, string.Format("X: {0}, Y: {1}", p.X, p.Y), Color.White);
            }
        }

        private void update()
        {
            if (_transform != null)
            {
                //var p = Camera.mainCamera.screenToWorld(Root.instance.input.MousePosition);
                //_transform.LookAt(p, TransformSpace.World);

                if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                    _transform.Translate(Transform.right * Root.instance.time.deltaTime * speed, TransformSpace.World);
                if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                    _transform.Translate(Transform.left * Root.instance.time.deltaTime * speed, TransformSpace.World);
                if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
                    _transform.Translate(Transform.forward * Root.instance.time.deltaTime * speed, TransformSpace.World);
                if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
                    _transform.Translate(Transform.backward * Root.instance.time.deltaTime * speed, TransformSpace.World);
            }
        }
    }
}