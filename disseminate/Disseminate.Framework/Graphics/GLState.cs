using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Math;

namespace Disseminate.Graphics
{
    public partial class GL
    {
        protected struct GLState
        {
            public Real depth, rotation;
            public Vector2 position, scale;
            public AxisAlignedBox2 source;
            public GLMaterial material;
            public GLTexture2D texture;
            public Color color;

            //public bool isClipping { get { return !clip.IsNull; } }

            public static GLState empty
            {
                get
                {
                    return new GLState()
                    {
                        position = Vector2.Zero,
                        rotation = 0,
                        //rotation = -Utility.PIOverTwo,
                        color = Color.White,
                        depth = 0f,
                        scale = Vector2.One,
                        material = null,
                        source = AxisAlignedBox2.Null
                    };
                }
            }
        }
    }
}
