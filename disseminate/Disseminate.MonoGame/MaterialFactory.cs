using Disseminate.Core;
using Disseminate.MonoGame.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.MonoGame
{
    public class MaterialFactory : AbstractMaterialFactory<Material>
    {
        public MaterialFactory(ResourceFactory rf)
            : base(rf)
        {

        }

        protected override Material createMaterial(ushort id, string name)
        {
            return new Material(id, name);
        }
    }
}
