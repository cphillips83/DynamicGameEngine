using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Json;
using Disseminate.Math;

namespace Disseminate.Core.PrefabSerializers
{
    public class Vecto4Serializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            var data = (Vector4)o;
            var jo = new JsonObject();
            jo["x"] = data.x;
            jo["y"] = data.y;
            jo["z"] = data.z;
            jo["w"] = data.w;

            return jo;
        }

        public object Deserialize(JsonValue jv)
        {
            var data = new Vector4();
            if (jv.Type == JsonTypes.Object)
            {
                var jo = (JsonObject)jv;
                var jx = jo["x"];
                var jy = jo["y"];
                var jz = jo["z"];
                var jw = jo["w"];

                if (jx.Type == JsonTypes.Number)
                    data.x = (float)jx;

                if (jy.Type == JsonTypes.Number)
                    data.y = (float)jy;

                if (jy.Type == JsonTypes.Number)
                    data.z = (float)jz;

                if (jy.Type == JsonTypes.Number)
                    data.w = (float)jw;

            }
            return data;
        }
    }

    public class Vecto4ArraySerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            if (o == null)
                return new JsonNull();

            var data = (Vector4[])o;
            var ja = new JsonArray();

            for (var i = 0; i < data.Length; i++)
                ja.Add(new JsonObject(new string[] { "x", "y", "z", "w" }, new object[] { data[i].x, data[i].y, data[i].z, data[i].w }));

            return ja;
        }

        public object Deserialize(JsonValue _jv)
        {
            if (_jv.Type == JsonTypes.Null)
                return null;

            var ja = (JsonArray)_jv;
            var data = new Vector4[ja.Count];
            for (var i = 0; i < data.Length; i++)
            {
                var jv = ja[i];
                if (jv.Type == JsonTypes.Object)
                {
                    var jo = (JsonObject)jv;
                    var jx = jo["x"];
                    var jy = jo["y"];
                    var jz = jo["z"];
                    var jw = jo["w"];

                    if (jx.Type == JsonTypes.Number)
                        data[i].x = (float)jx;

                    if (jy.Type == JsonTypes.Number)
                        data[i].y = (float)jy;

                    if (jy.Type == JsonTypes.Number)
                        data[i].z = (float)jz;

                    if (jy.Type == JsonTypes.Number)
                        data[i].w = (float)jw;
                }
            }
            return data;
        }
    }

}
