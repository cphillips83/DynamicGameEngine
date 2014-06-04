using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.Json
{
    public class JsonNumber : JsonValue
    {
        public double Number { get; set; }
        public JsonNumber(double number)
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
