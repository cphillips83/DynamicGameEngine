using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.Json
{
    public class JsonNull : JsonValue
    {
        public override void Write(StringBuilder sb)
        {
            sb.Append("null");
        }

        public override JsonTypes Type { get { return JsonTypes.Null; } }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Write(sb);
            return sb.ToString();
        }
        
        private static JsonNull _null = new JsonNull();
        public static JsonNull Null { get { return _null; } }
    }

}
