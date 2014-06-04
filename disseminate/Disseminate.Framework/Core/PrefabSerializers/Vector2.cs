using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Json;
using Disseminate.Math;

namespace Disseminate.Core.PrefabSerializers
{
    public class Vecto2Serializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            var data = (Vector2)o;
            var jo = new JsonObject();
            jo["x"] = data.X;
            jo["y"] = data.Y;

            return jo;
        }

        public object Deserialize(JsonValue jv)
        {
            var data = new Vector2();
            if (jv.Type == JsonTypes.Object)
            {
                var jo = (JsonObject)jv;
                var jx = jo["x"];
                var jy = jo["y"];

                if (jx.Type == JsonTypes.Number)
                    data.X = (float)jx;

                if (jy.Type == JsonTypes.Number)
                    data.Y = (float)jy;

            }
            return data;
        }
    }

    public class Vecto2ArraySerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            if (o == null)
                return new JsonNull();

            var data = (Vector2[])o;
            var ja = new JsonArray();

            for (var i = 0; i < data.Length; i++)
                ja.Add(new JsonObject(new string[] { "x", "y" }, new object[] { data[i].X, data[i].Y }));

            return ja;
        }

        public object Deserialize(JsonValue _jv)
        {
            if (_jv.Type == JsonTypes.Null)
                return null;

            var ja = (JsonArray)_jv;
            var data = new Vector2[ja.Count];
            for (var i = 0; i < data.Length; i++)
            {
                var jv = ja[i];
                if (jv.Type == JsonTypes.Object)
                {
                    var jo = (JsonObject)jv;
                    var jx = jo["x"];
                    var jy = jo["y"];

                    if (jx.Type == JsonTypes.Number)
                        data[i].X = (float)jx;

                    if (jy.Type == JsonTypes.Number)
                        data[i].Y = (float)jy;
                }
            }
            return data;
        }
    }

}
