using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Json;

namespace Disseminate.Core.PrefabSerializers
{
    #region Bool
    public class BoolSerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            return new JsonBool((bool)o);
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Bool)
                return ((JsonBool)jo).Value;

            return (byte)0;
        }
    }
    #endregion

    #region Byte
    public class ByteSerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            return new JsonNumber((byte)o);
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Number)
                return (byte)((JsonNumber)jo).Number;

            return (byte)0;
        }
    }
    #endregion

    #region SByte
    public class SByteSerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            return new JsonNumber((sbyte)o);
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Number)
                return (sbyte)((JsonNumber)jo).Number;

            return (sbyte)0;
        }
    }
    #endregion

    #region Short
    public class ShortSerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            return new JsonNumber((short)o);
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Number)
                return (short)((JsonNumber)jo).Number;

            return (short)0;
        }
    }
    #endregion

    #region UShort
    public class UShortSerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            return new JsonNumber((ushort)o);
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Number)
                return (ushort)((JsonNumber)jo).Number;

            return (ushort)0;
        }
    }
    #endregion

    #region Int
    public class IntSerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            return new JsonNumber((int)o);
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Number)
                return (int)((JsonNumber)jo).Number;

            return (int)0;
        }
    }
    #endregion

    #region UInt
    public class UIntSerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            return new JsonNumber((uint)o);
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Number)
                return (uint)((JsonNumber)jo).Number;

            return (uint)0;
        }
    }
    #endregion

    #region Long
    public class LongSerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            return new JsonNumber((long)o);
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Number)
                return (long)((JsonNumber)jo).Number;

            return (long)0;
        }
    }
    #endregion

    #region ULong
    public class ULongSerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            return new JsonNumber((ulong)o);
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Number)
                return (ulong)((JsonNumber)jo).Number;

            return (ulong)0;
        }
    }
    #endregion

    #region Float
    public class FloatSerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            return new JsonNumber((float)o);
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Number)
                return (float)((JsonNumber)jo).Number;

            return (float)0;
        }
    }
    #endregion

    #region Double
    public class DoubleSerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            return new JsonNumber((double)o);
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Number)
                return (double)((JsonNumber)jo).Number;

            return (double)0;
        }
    }
    #endregion

    #region Bool
    public class BoolArraySerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            if (o == null)
                return new JsonNull();

            var data = (bool[])o;
            var ja = new JsonArray();

            for (var i = 0; i < data.Length; i++)
                ja.Add(new JsonBool((bool)data[i]));

            return ja;
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Null)
                return null;

            var ja = (JsonArray)jo;
            var data = new bool[ja.Count];
            for (var i = 0; i < data.Length; i++)
                if (ja[i].Type == JsonTypes.Bool)
                    data[i] = ((JsonBool)ja[i]).Value;

            return data;
        }
    }
    #endregion

    #region Byte[]
    public class ByteArraySerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            if (o == null)
                return new JsonNull();

            var data = (byte[])o;
            var ja = new JsonArray();

            for (var i = 0; i < data.Length; i++)
                ja.Add(new JsonNumber(data[i]));

            return ja;
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Null)
                return null;

            var ja = (JsonArray)jo;
            var data = new byte[ja.Count];
            for (var i = 0; i < data.Length; i++)
                if (ja[i].Type == JsonTypes.Number)
                    data[i] = (byte)((JsonNumber)ja[i]).Number;

            return data;
        }
    }
    #endregion

    #region SByte[]
    public class SByteArraySerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            if (o == null)
                return new JsonNull();

            var data = (sbyte[])o;
            var ja = new JsonArray();

            for (var i = 0; i < data.Length; i++)
                ja.Add(new JsonNumber(data[i]));

            return ja;
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Null)
                return null;

            var ja = (JsonArray)jo;
            var data = new sbyte[ja.Count];
            for (var i = 0; i < data.Length; i++)
                if (ja[i].Type == JsonTypes.Number)
                    data[i] = (sbyte)((JsonNumber)ja[i]).Number;

            return data;
        }
    }
    #endregion

    #region Short[]
    public class ShortArraySerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            if (o == null)
                return new JsonNull();

            var data = (short[])o;
            var ja = new JsonArray();

            for (var i = 0; i < data.Length; i++)
                ja.Add(new JsonNumber(data[i]));

            return ja;
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Null)
                return null;

            var ja = (JsonArray)jo;
            var data = new short[ja.Count];
            for (var i = 0; i < data.Length; i++)
                if (ja[i].Type == JsonTypes.Number)
                    data[i] = (short)((JsonNumber)ja[i]).Number;

            return data;
        }
    }
    #endregion

    #region UShort[]
    public class UShortArraySerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            if (o == null)
                return new JsonNull();

            var data = (ushort[])o;
            var ja = new JsonArray();

            for (var i = 0; i < data.Length; i++)
                ja.Add(new JsonNumber(data[i]));

            return ja;
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Null)
                return null;

            var ja = (JsonArray)jo;
            var data = new ushort[ja.Count];
            for (var i = 0; i < data.Length; i++)
                if (ja[i].Type == JsonTypes.Number)
                    data[i] = (ushort)((JsonNumber)ja[i]).Number;

            return data;
        }
    }
    #endregion

    #region Int[]
    public class IntArraySerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            if (o == null)
                return new JsonNull();

            var data = (int[])o;
            var ja = new JsonArray();

            for (var i = 0; i < data.Length; i++)
                ja.Add(new JsonNumber(data[i]));

            return ja;
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Null)
                return null;

            var ja = (JsonArray)jo;
            var data = new int[ja.Count];
            for (var i = 0; i < data.Length; i++)
                if (ja[i].Type == JsonTypes.Number)
                    data[i] = (int)((JsonNumber)ja[i]).Number;

            return data;
        }
    }
    #endregion

    #region UInt[]
    public class UIntArraySerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            if (o == null)
                return new JsonNull();

            var data = (uint[])o;
            var ja = new JsonArray();

            for (var i = 0; i < data.Length; i++)
                ja.Add(new JsonNumber(data[i]));

            return ja;
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Null)
                return null;

            var ja = (JsonArray)jo;
            var data = new uint[ja.Count];
            for (var i = 0; i < data.Length; i++)
                if (ja[i].Type == JsonTypes.Number)
                    data[i] = (uint)((JsonNumber)ja[i]).Number;

            return data;
        }
    }
    #endregion

    #region Long[]
    public class LongArraySerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            if (o == null)
                return new JsonNull();

            var data = (long[])o;
            var ja = new JsonArray();

            for (var i = 0; i < data.Length; i++)
                ja.Add(new JsonNumber(data[i]));

            return ja;
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Null)
                return null;

            var ja = (JsonArray)jo;
            var data = new long[ja.Count];
            for (var i = 0; i < data.Length; i++)
                if (ja[i].Type == JsonTypes.Number)
                    data[i] = (long)((JsonNumber)ja[i]).Number;

            return data;
        }
    }
    #endregion

    #region ULong[]
    public class ULongArraySerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            if (o == null)
                return new JsonNull();

            var data = (ulong[])o;
            var ja = new JsonArray();

            for (var i = 0; i < data.Length; i++)
                ja.Add(new JsonNumber(data[i]));

            return ja;
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Null)
                return null;

            var ja = (JsonArray)jo;
            var data = new ulong[ja.Count];
            for (var i = 0; i < data.Length; i++)
                if (ja[i].Type == JsonTypes.Number)
                    data[i] = (ulong)((JsonNumber)ja[i]).Number;

            return data;
        }
    }
    #endregion

    #region Float[]
    public class FloatArraySerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            if (o == null)
                return new JsonNull();

            var data = (float[])o;
            var ja = new JsonArray();

            for (var i = 0; i < data.Length; i++)
                ja.Add(new JsonNumber(data[i]));

            return ja;
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Null)
                return null;

            var ja = (JsonArray)jo;
            var data = new float[ja.Count];
            for (var i = 0; i < data.Length; i++)
                if (ja[i].Type == JsonTypes.Number)
                    data[i] = (float)((JsonNumber)ja[i]).Number;

            return data;
        }
    }
    #endregion

    #region Double[]
    public class DoubleArraySerializer : ISerializer
    {
        public JsonValue Serialize(object o)
        {
            if (o == null)
                return new JsonNull();

            var data = (double[])o;
            var ja = new JsonArray();

            for (var i = 0; i < data.Length; i++)
                ja.Add(new JsonNumber(data[i]));

            return ja;
        }

        public object Deserialize(JsonValue jo)
        {
            if (jo.Type == JsonTypes.Null)
                return null;

            var ja = (JsonArray)jo;
            var data = new double[ja.Count];
            for (var i = 0; i < data.Length; i++)
                if (ja[i].Type == JsonTypes.Number)
                    data[i] = (double)((JsonNumber)ja[i]).Number;

            return data;
        }
    }
    #endregion
}
