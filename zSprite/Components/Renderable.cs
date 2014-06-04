using Disseminate.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite.Resources;

namespace zSprite.Components
{
    public class Renderable : IComponent
    {
        public string materialName { get; set; }
        public Material material { get; set; }
        
    }
}
