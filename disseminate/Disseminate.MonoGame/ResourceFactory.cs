using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Core;
using XnaContentManager = Microsoft.Xna.Framework.Content.ContentManager;

namespace Disseminate.MonoGame
{
    public class ResourceFactory : AbstractResourceFactory
    {
        protected XnaContentManager _content;

        public ResourceFactory(string root )
            : base(root)
        {
            //_content = xcm;
        }
    }
}
