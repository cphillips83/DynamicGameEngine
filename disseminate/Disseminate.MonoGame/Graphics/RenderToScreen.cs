using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Graphics;

namespace Disseminate.MonoGame.Graphics
{
    public class RenderToScreen : IRenderTarget
    {
        public void enable()
        {
            MonoGL.instance._device.SetRenderTarget(null);
        }

    }
}
