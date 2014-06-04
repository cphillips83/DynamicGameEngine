using Disseminate.Graphics;
using Disseminate.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.Core
{
    public class CameraComponent : IComponent
    {
        public Color color = Color.Transparent;
        public long cullingmask = 0;
        public AxisAlignedBox2 normalizedViewport = new AxisAlignedBox2(Vector2.Zero, Vector2.One);
        public int depth = 0;
    }
}
