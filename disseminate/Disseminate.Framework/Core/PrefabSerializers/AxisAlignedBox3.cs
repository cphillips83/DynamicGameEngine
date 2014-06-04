using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Json;
using Disseminate.Math;

namespace Disseminate.Core.PrefabSerializers
{
    public class AxisAlignedBox2Serializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            var data = (AxisAlignedBox2)o;

            if (data.IsNull)
                return new JsonNull();

            return new JsonObject(
                new string[] { "min", "max" },
                new object[] { 
                    new JsonObject( new string[] { "x", "y"}, new object[] { data.Minimum.X, data.Minimum.Y}),
                    new JsonObject( new string[] { "x", "y"}, new object[] { data.Maximum.X, data.Maximum.Y})});
        }

        public object Deserialize(JsonValue jv)
        {
            var data = AxisAlignedBox2.Null;
            if (jv.Type == JsonTypes.Object)
            {
                var jo = (JsonObject)jv;
                var min = jo["min"];
                var max = jo["max"];

                if (min.Type == JsonTypes.Object && max.Type == JsonTypes.Object)
                {
                    var jmin = (JsonObject)min;
                    var jmax = (JsonObject)max;

                    var vmin = new Vector2();
                    var vmax = new Vector2();

                    var jminx = jmin["x"];
                    var jminy = jmin["y"];
                    if (jminx.Type == JsonTypes.Number)
                        vmin.X = (float)jminx;

                    if (jminy.Type == JsonTypes.Number)
                        vmin.Y = (float)jminy;

                    var jmaxx = jmax["x"];
                    var jmaxy = jmax["y"];
                    if (jmaxx.Type == JsonTypes.Number)
                        vmax.X = (float)jmaxx;

                    if (jmaxy.Type == JsonTypes.Number)
                        vmax.Y = (float)jmaxy;

                    data.SetExtents(vmin, vmax);
                }
            }
            return data;
        }
    }

    public class AxisAlignedBox2ArraySerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            if (o == null)
                return new JsonNull();

            var data = (AxisAlignedBox2[])o;
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
                            new JsonObject( new string[] { "x", "y"}, new object[] { data[i].Minimum.X, data[i].Minimum.Y}),
                            new JsonObject( new string[] { "x", "y"}, new object[] { data[i].Maximum.X, data[i].Maximum.Y})})
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
            var data = new AxisAlignedBox2[ja.Count];
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

                        var vmin = new Vector2();
                        var vmax = new Vector2();

                        var jminx = jmin["x"];
                        var jminy = jmin["y"];
                        if (jminx.Type == JsonTypes.Number)
                            vmin.X = (float)jminx;

                        if (jminy.Type == JsonTypes.Number)
                            vmin.Y = (float)jminy;

                        var jmaxx = jmax["x"];
                        var jmaxy = jmax["y"];
                        if (jmaxx.Type == JsonTypes.Number)
                            vmax.X = (float)jmaxx;

                        if (jmaxy.Type == JsonTypes.Number)
                            vmax.Y = (float)jmaxy;

                        data[i].SetExtents(vmin, vmax);
                    }
                }
            }
            return data;
        }
    }

}
