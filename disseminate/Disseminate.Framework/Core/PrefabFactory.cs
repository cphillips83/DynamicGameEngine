using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Json;
using Disseminate.Math;
using System.Reflection;
using System.Reflection.Emit;
using Disseminate.Core.PrefabSerializers;
using System.IO;

namespace Disseminate.Core
{
    public interface ISerializer
    {
        JsonValue Serialize(object o);
        object Deserialize(JsonValue jo);
    }

    public class PrefabFactory : Factory<PrefabFactory>
    {
        #region TypeSerializer
        private class TypeSerializer
        {
            public Type type;
            public FieldInfo[] fields;
            //public ISerializer[] fields;
            public Action<PrefabFactory, TypeSerializer, JsonObject> load;

            public TypeSerializer(Type type)
            {
                this.type = type;
                fields = type.GetFields(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance).ToArray();
            }

            public void build()
            {
                var loader = typeof(PrefabFactory).GetMethod("loadFields", BindingFlags.NonPublic | BindingFlags.Instance);
                var callback = typeof(PrefabFactory).GetMethod("LoadType", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(type);
                //var actionType = typeof(Action<,,>).MakeGenericType(typeof(JsonProcessor), typeof(JsonObject));
                //var invoke = actionType.GetMethod("Invoke");

                DynamicMethod dynam =
                    new DynamicMethod(
                    ""
                    , typeof(void)
                    , new Type[] { typeof(PrefabFactory), typeof(TypeSerializer), typeof(JsonObject) }
                    , typeof(TypeSerializer)
                    , true);

                ILGenerator il = dynam.GetILGenerator();
                var dl = il.DeclareLocal(typeof(object));
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldarg_2);
                il.Emit(OpCodes.Ldloca_S, dl);
                il.Emit(OpCodes.Box, type);
                il.Emit(OpCodes.Call, loader);
                //il.Emit(OpCodes.Pop);
                il.Emit(OpCodes.Unbox_Any, type);
                il.Emit(OpCodes.Call, callback);
                il.Emit(OpCodes.Ret);
                load = (Action<PrefabFactory, TypeSerializer, JsonObject>)dynam.CreateDelegate(typeof(Action<PrefabFactory, TypeSerializer, JsonObject>));
            }

        }
        #endregion TypeSerializer

        private EntityManager2 currentEM;
        private ushort currentId;

        private Dictionary<Type, ISerializer> serializers = new Dictionary<Type, ISerializer>();
        private Dictionary<string, TypeSerializer> typeData = new Dictionary<string, TypeSerializer>();

        public PrefabFactory()
        {
            RegisterSerializer<bool>(new BoolSerializer());
            RegisterSerializer<bool[]>(new BoolArraySerializer());
            RegisterSerializer<byte>(new ByteSerializer());
            RegisterSerializer<byte[]>(new ByteArraySerializer());
            RegisterSerializer<sbyte>(new SByteSerializer());
            RegisterSerializer<sbyte[]>(new SByteArraySerializer());
            RegisterSerializer<short>(new ShortSerializer());
            RegisterSerializer<short[]>(new ShortArraySerializer());
            RegisterSerializer<ushort>(new UShortSerializer());
            RegisterSerializer<ushort[]>(new UShortArraySerializer());
            RegisterSerializer<int>(new IntSerializer());
            RegisterSerializer<int[]>(new IntArraySerializer());
            RegisterSerializer<uint>(new UIntSerializer());
            RegisterSerializer<uint[]>(new UIntArraySerializer());
            RegisterSerializer<long>(new LongSerializer());
            RegisterSerializer<long[]>(new LongArraySerializer());
            RegisterSerializer<ulong>(new ULongSerializer());
            RegisterSerializer<ulong[]>(new ULongArraySerializer());
            RegisterSerializer<float>(new FloatSerializer());
            RegisterSerializer<float[]>(new FloatArraySerializer());
            RegisterSerializer<double>(new DoubleSerializer());
            RegisterSerializer<double[]>(new DoubleArraySerializer());

            RegisterSerializer<Vector2>(new Vecto2Serializer());
            RegisterSerializer<Vector2[]>(new Vecto2ArraySerializer());
            RegisterSerializer<Vector3>(new Vecto3Serializer());
            RegisterSerializer<Vector3[]>(new Vecto3ArraySerializer());
            RegisterSerializer<Vector4>(new Vecto4Serializer());
            RegisterSerializer<Vector4[]>(new Vecto4ArraySerializer());

            RegisterSerializer<AxisAlignedBox2>(new AxisAlignedBox2Serializer());
            RegisterSerializer<AxisAlignedBox2[]>(new AxisAlignedBox2ArraySerializer());
            RegisterSerializer<AxisAlignedBox3>(new AxisAlignedBox3Serializer());
            RegisterSerializer<AxisAlignedBox3[]>(new AxisAlignedBox3ArraySerializer());

        }

        protected override PrefabFactory getInstance()
        {
            return this;
        }

        public void RegisterSerializer<T>(ISerializer s)
        {
            serializers.Add(typeof(T), s);
        }

        public void process(EntityManager2 em, string file)
        {
            currentEM = em;
            currentId = em.create();

            using (var sr = new StreamReader(file))
            {
                var jo = JsonReader.ParseObject(sr.ReadToEnd());
                process(em, jo);
            }

            currentId = 0;
            currentEM = null;
        }

        public void process(EntityManager2 em, JsonObject jo)
        {
            currentEM = em;
            currentId = em.create();

            foreach (var kvp in jo)
                loadType(kvp.Key, kvp.Value);

            currentId = 0;
            currentEM = null;
        }

        private void loadType(string name, JsonValue v)
        {
            TypeSerializer ts;
            if (!typeData.TryGetValue(name, out ts))
                ts = build(name);

            ts.load(this, ts, (JsonObject)v);
        }

        private object loadFields(TypeSerializer ts, JsonObject jo, object p)
        {
            foreach (var fieldType in ts.fields)
            {
                var jv = jo[fieldType.Name];
                ISerializer serializer;
                if (!serializers.TryGetValue(fieldType.FieldType, out serializer))
                    throw new Exception(string.Format("{0} unsupported type", fieldType.FieldType));

                fieldType.SetValue(p, serializer.Deserialize(jv));
            }

            return p;
        }

        private TypeSerializer build(string name)
        {
            var type = Type.GetType(name, false, true);
            if (type == null)
                throw new Exception(string.Format("{0} invalid type"));

            var ts = new TypeSerializer(type);
            ts.build();
            typeData.Add(name, ts);
            return ts;
        }

        private void LoadType<T>(T t)
            where T : struct
        {
            currentEM.set(currentId, t);
        }

    }
}
