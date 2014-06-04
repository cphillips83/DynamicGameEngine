using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zSprite.Json
{
    public class JsonNumber : JsonValue
    {
        public float Number { get; set; }
        public JsonNumber(float number)
        {
            this.Number = number;
        }

        public override void Write(StringBuilder sb)
        {
            sb.Append(Number);
        }

        public override JsonTypes Type { get { return JsonTypes.Number; } }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Write(sb);
            return sb.ToString();
        }
    }

}
