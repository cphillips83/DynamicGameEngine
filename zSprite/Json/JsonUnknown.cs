using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zSprite.Json
{
    public abstract class JsonUnknown : JsonValue
    {
        public override JsonTypes Type { get { return JsonTypes.Unknown; } }

    }
}
