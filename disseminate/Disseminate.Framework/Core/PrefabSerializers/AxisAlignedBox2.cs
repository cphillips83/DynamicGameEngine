using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Json;
using Disseminate.Math;

namespace Disseminate.Core.PrefabSerializers
{
    public class AxisAlignedBox3Serializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            var data = (AxisAlignedBox3)o;

            if (data.IsNull)
                return new JsonNull();

            return new JsonObject(
                new string[] { "min", "max" },
                new object[] { 
                    new JsonObject( new string[] { "x", "y", "z"}, new object[] { data.Minimum.X, data.Minimum.Y, data.Minimum.Z}),
                    new JsonObject( new string[] { "x", "y", "z"}, new object[] { data.Maximum.X, data.Maximum.Y, data.Maximum.Z})});
        }

        public object Deserialize(JsonValue jv)
        {
            var data = AxisAlignedBox3.Null;
            if (jv.Type == JsonTypes.Object)
            {
                var jo = (JsonObject)jv;
                var min = jo["min"];
                var max = jo["max"];

                if (min.Type == JsonTypes.Object && max.Type == JsonTypes.Object)
                {
                    var jmin = (JsonObject)min;
                    var jmax = (JsonObject)max;

                    var vmin = new Vector3();
                    var vmax = new Vector3();

                    var jminx = jmin["x"];
                    var jminy = jmin["y"];
                    var jminz = jmin["z"];
                    if (jminx.Type == JsonTypes.Number)
                        vmin.X = (float)jminx;

                    if (jminy.Type == JsonTypes.Number)
                        vmin.Y = (float)jminy;

                    if (jminz.Type == JsonTypes.Number)
                        vmin.Z = (float)jminz;

                    var jmaxx = jmax["x"];
                    var jmaxy = jmax["y"];
                    var jmaxz = jmax["z"];
                    if (jmaxx.Type == JsonTypes.Number)
                        vmax.X = (float)jmaxx;

                    if (jmaxy.Type == JsonTypes.Number)
                        vmax.Y = (float)jmaxy;

                    if (jmaxz.Type == JsonTypes.Number)
                        vmax.Z = (float)jmaxz;

                    data.SetExtents(vmin, vmax);
                }
            }
            return data;
        }
    }

    public class AxisAlignedBox3ArraySerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            if (o == null)
                return new JsonNull();

            var data = (AxisAlignedBox3[])o;
            var ja = new JsonArray();

            for (var i = 0; i < data.Length; i++)
            {
                if (data[i].IsNull)
                    ja.Add(new JsonNull());
                else
                {
                    ja.Add(new JsonObject(
                        new string[] { "min", "max" },
                        new object[] { 
                            new JsonObject( new string[] { "x", "y", "z"}, new object[] { data[i].Minimum.X, data[i].Minimum.Y, data[i].Minimum.Z}),
                            new JsonObject( new string[] { "x", "y", "z"}, new object[] { data[i].Maximum.X, data[i].Maximum.Y, data[i].Maximum.Z})})
                        );
                }
            }
            return ja;
        }

        public object Deserialize(JsonValue _jv)
        {
            if (_jv.Type == JsonTypes.Null)
                return null;

            var ja = (JsonArray)_jv;
            var data = new AxisAlignedBox3[ja.Count];
            for (var i = 0; i < data.Length; i++)
            {
                var jv = ja[i];
                if (jv.Type == JsonTypes.Object)
                {
                    var jo = (JsonObject)jv;
                    var min = jo["min"];
                    var max = jo["max"];

                    if (min.Type == JsonTypes.Object && max.Type == JsonTypes.Object)
                    {
                        var jmin = (JsonObject)min;
                        var jmax = (JsonObject)max;

                        var vmin = new Vector3();
                        var vmax = new Vector3();

                        var jminx = jmin["x"];
                        var jminy = jmin["y"];
                        var jminz = jmin["z"];
                        if (jminx.Type == JsonTypes.Number)
                            vmin.X = (float)jminx;

                        if (jminy.Type == JsonTypes.Number)
                            vmin.Y = (float)jminy;

                        if (jminz.Type == JsonTypes.Number)
                            vmin.Z = (float)jminz;

                        var jmaxx = jmax["x"];
                        var jmaxy = jmax["y"];
                        var jmaxz = jmax["z"];
                        if (jmaxx.Type == JsonTypes.Number)
                            vmax.X = (float)jmaxx;

                        if (jmaxy.Type == JsonTypes.Number)
                            vmax.Y = (float)jmaxy;

                        if (jmaxz.Type == JsonTypes.Number)
                            vmax.Z = (float)jmaxz;

                        data[i].SetExtents(vmin, vmax);
                    }
                }
            }
            return data;
        }
    }

}
