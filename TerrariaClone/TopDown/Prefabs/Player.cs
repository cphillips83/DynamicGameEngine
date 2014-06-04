using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameName1.TopDown.Components;
using GameName1.TopDown.Controllers;
using zSprite;

namespace GameName1.TopDown
{
    public static class Player
    {
        public static GameObject prefabPlayer(this GameObject parent)
        {
            var go = parent.createChild("player");
            go.createScript<VelocityComponent>();
            go.createScript<Camera>();
            go.createScript<PlayerController>();
            go.createScript<MovementComponent>();
            go.createScript<MovementAnimation>();

            return go;
        }
    }
}
