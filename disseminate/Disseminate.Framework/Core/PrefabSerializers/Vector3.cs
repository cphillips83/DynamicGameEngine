using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Json;
using Disseminate.Math;

namespace Disseminate.Core.PrefabSerializers
{
    public class Vecto3Serializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            var data = (Vector3)o;
            var jo = new JsonObject();
            jo["x"] = data.X;
            jo["y"] = data.Y;
            jo["z"] = data.Z;

            return jo;
        }

        public object Deserialize(JsonValue jv)
        {
            var data = new Vector3();
            if (jv.Type == JsonTypes.Object)
            {
                var jo = (JsonObject)jv;
                var jx = jo["x"];
                var jy = jo["y"];
                var jz = jo["z"];

                if (jx.Type == JsonTypes.Number)
                    data.X = (float)jx;

                if (jy.Type == JsonTypes.Number)
                    data.Y = (float)jy;

                if (jy.Type == JsonTypes.Number)
                    data.Z = (float)jz;

            }
            return data;
        }
    }

    public class Vecto3ArraySerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            if (o == null)
                return new JsonNull();

            var data = (Vector3[])o;
            var ja = new JsonArray();

            for (var i = 0; i < data.Length; i++)
                ja.Add(new JsonObject(new string[] { "x", "y", "z" }, new object[] { data[i].X, data[i].Y, data[i].Z }));

            return ja;
        }

        public object Deserialize(JsonValue _jv)
        {
            if (_jv.Type == JsonTypes.Null)
                return null;

            var ja = (JsonArray)_jv;
            var data = new Vector3[ja.Count];
            for (var i = 0; i < data.Length; i++)
            {
                var jv = ja[i];
                if (jv.Type == JsonTypes.Object)
                {
                    var jo = (JsonObject)jv;
                    var jx = jo["x"];
                    var jy = jo["y"];
                    var jz = jo["z"];

                    if (jx.Type == JsonTypes.Number)
                        data[i].X = (float)jx;

                    if (jy.Type == JsonTypes.Number)
                        data[i].Y = (float)jy;

                    if (jy.Type == JsonTypes.Number)
                        data[i].Z = (float)jz;
                }
            }
            return data;
        }
    }

}
