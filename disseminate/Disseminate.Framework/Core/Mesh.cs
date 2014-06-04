using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Graphics;
using Disseminate.Math;

namespace Disseminate.Core
{
    public interface IMesh
    {
        void render(GL gl);
    }

    public class Mesh : IMesh
    {
        public AxisAlignedBox2 source = AxisAlignedBox2.Null;
        public AxisAlignedBox2 destination = AxisAlignedBox2.Null;
        //public Real rotation = 0;

        public void render(GL gl)
        {
            ////var savedRotation = 0f;
            //if (rotation != 0)
            //{
            //    //savedRotation = gl.currentRotation;
            //    gl.rotate(rotation);
            //}
            if (!destination.IsNull)
            {
                gl.source(source);
                gl.quad(destination);
            }

            //if (rotation != 0) gl.rotate(-rotation);
        }
    }

    public class MeshComponent : IComponent
    {
        public GLMaterial material;
        public Mesh mesh;
        public Color color = Color.White;
    }

    public class MeshFactory
    {

    }

    public abstract class MeshSystem : RequiresSystem
    {
        private GL gl;
        protected override void oninit()
        {
            base.oninit();

            require<PositionComponent, MeshComponent>(render);
            gl = createGL();
        }

        public abstract GL createGL();

        private void render(PositionComponent p, MeshComponent m)
        {
            //gl.rotate(p.rotation);
            //gl.translate(p.translation);


        }
    }
}
