using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Disseminate.Math;
using Disseminate.Core;
using Disseminate.Json;

namespace Disseminate
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Entity2
    {
        //[FieldOffset(0)]
        //public ushort id;
        [FieldOffset(0)]
        public ushort prev;
        [FieldOffset(2)]
        public ushort next;
        [FieldOffset(4)]
        public ushort firstchild;
        [FieldOffset(6)]
        public ushort parent;

        public override string ToString()
        {
            return string.Format("{{prev: {0}, next: {1}, first:{2}, parent:{3}}}", prev, next, firstchild, parent);
        }

        public static Entity2 empty { get { return new Entity2() { prev = 0, next = 0, firstchild = 0, parent = 0 }; } }
    }

    public struct EntityFlags2
    {
        public const ulong _inUse = 1 << 0;

        public ulong flags;

        public void setFlags(bool isSet, ulong options)
        {
            if (isSet)
                flags |= options;
            else
                flags &= (ulong.MaxValue ^ options);
        }

        public bool inUse { get { return (flags & _inUse) == _inUse; } set { setFlags(value, _inUse); } }
    }

    //[StructLayout(LayoutKind.Explicit)]
    //public struct ComponentMask
    //{
    //    public fixed long data[2];
    //}

    public interface IComponentData
    {
        void init();
        void clear(ushort id);
        bool has(ushort id);
        void set(ushort id, object data);
    }

    public class ComponentData<T> : IComponentData
        where T : struct
    {
        private int _dataIndex;

        public T[] data;// = new T[ushort.MaxValue];
        //public uint[] mappings;// = new uint[ushort.MaxValue / 32];
        public ushort[] mappings;// = new uint[ushort.MaxValue / 32];
        public ushort[] freeData;

        public void init()
        {
            data = new T[256];
            mappings = new ushort[ushort.MaxValue];
            freeData = new ushort[ushort.MaxValue];

            for (var i = 0; i < freeData.Length; i++)
                freeData[i] = (ushort)i;

            //_dataIndex = 1;
        }

        public void set(ushort id, object data)
        {
            set(id, (T)data);
        }

        public void set(ushort id, T t)
        {
            var index = mappings[id];
            if (mappings[id] > 0)
            {
                data[index] = t;
            }
            else
            {
                _dataIndex++;
                if (_dataIndex == ushort.MaxValue)
                    throw new Exception("no more data slots free");

                if (_dataIndex == data.Length)
                    Array.Resize(ref data, data.Length * 2);

                index = freeData[_dataIndex];
                mappings[id] = index;
                data[index] = t;
            }
        }

        public bool has(ushort id)
        {
            //var mask = id % 32;
            //var index = (id - mask) / 32;

            //return (mappings[index] & ((uint)1 << mask)) > 0;

            return mappings[id] != 0;
        }

        public T get(ushort id)
        {
            return data[mappings[id]];
        }

        public void clear(ushort id)
        {
            var index = mappings[id];

            if (index > 0)
            {
                data[index] = data[0];
                mappings[id] = 0;

                freeData[_dataIndex] = index;
                _dataIndex--;
            }

            //var mask = id % 32;
            //var index = (id - mask) / 32;

            //data[id] = new T();

            //mappings[index] = mappings[index] & (uint.MaxValue ^ ((uint)1 << mask));
        }
    }

    public class EntityManager2
    {
        public EntityFlags2[] _entityFlags = new EntityFlags2[ushort.MaxValue];
        public Entity2[] _entities = new Entity2[ushort.MaxValue];
        private ushort[] _freeEntities = new ushort[ushort.MaxValue];
        private int _freeIndex = ushort.MaxValue - 1;

        //public ushort[] _rootEntities = new ushort[ushort.MaxValue];
        //public int _rootIndex = 0;
        public int totalEntities { get { return ushort.MaxValue - _freeIndex + 1; } }

        public Dictionary<Type, IComponentData> data = new Dictionary<Type, IComponentData>();

        public void init()
        {
            for (var i = 0; i < _freeEntities.Length; i++)
                _freeEntities[ushort.MaxValue - i - 1] = (ushort)i;

        }

        public ushort create()
        {
            return create(0);
            //if (_freeIndex == 0)
            //    throw new Exception("No more entities free!");

            //_freeIndex--;
            //var id = _freeEntities[_freeIndex];

            //if (_entityFlags[id].inUse)
            //    throw new Exception("entity id already in use");

            //_rootEntities[_rootIndex++] = id;
            //_entityFlags[id].inUse = true;
            //Console.WriteLine("create {0} {1}", _freeEntities[_freeIndex], _entities[_freeEntities[_freeIndex]]);
            //return id;
        }

        public ushort create(ushort parent)
        {
            if (_freeIndex == 0)
                throw new Exception("No more entities free!");

            _freeIndex--;
            var id = _freeEntities[_freeIndex];

            if (_entityFlags[id].inUse)
                throw new Exception("entity id already in use");

            _entityFlags[id].inUse = true;
            addChild(parent, id);
            Console.WriteLine("create child {0} - {1} {2}", parent, id, _entities[id]);
            return id;
        }

        private void addChild(ushort parent, ushort child)
        {
            if (_entities[parent].firstchild != 0)
            {
                _entities[child].next = _entities[parent].firstchild;
                _entities[_entities[parent].firstchild].prev = child;
            }

            _entities[parent].firstchild = child;
            _entities[child].parent = parent;
            _entities[child].prev = 0;
        }

        private void removeChild(ushort id)
        {
            Console.WriteLine("removing {0} {1}", id, _entities[id]);
            if (_entities[id].firstchild != 0)
                removeChild(_entities[id].firstchild);

            var prev = _entities[id].prev;
            var next = _entities[id].next;
            var parent = _entities[id].parent;

            if (prev == 0)
            {
                //have to update the parents first child
                //if (parent != 0)
                {
                    if (next == 0)
                        _entities[_entities[id].parent].firstchild = 0;
                    else
                        _entities[_entities[id].parent].firstchild = next;
                }
            }
            else
            {
                _entities[prev].next = next;
            }

            if (next != 0)
            {
                _entities[next].prev = prev;
            }


            //if (prev == 0 && next != 0 && parent != 0)
            //{
            //    //move the next node as the first
            //    _entities[_entities[id].parent].firstchild = next;
            //    _entities[next].prev = 0;
            //}
            //else
            //{
            //    //if(prev != 0)
            //}
            ////else if (prev != 0 && next == 0)
            ////{
            ////    //remove from the last node
            ////    _entities[_entities[id].prev].next = 0;
            ////}
            ////else if (prev == 0 && next == 0 && parent != 0)
            ////{
            ////    //only child
            ////    _entities[_entities[id].parent].firstchild = 0;
            ////}

            //if (prev == 0)
            //{

            //}

            //if (parent == 0)
            //{
            //    for (var i = 0; i < ushort.MaxValue; i++)
            //    {
            //        if (_rootEntities[i] == id)
            //        {
            //            _rootEntities[i] = _rootEntities[_rootIndex - 1];
            //            _rootIndex--;
            //            Console.WriteLine("removing root {0} {1}", id, _entities[id]);
            //            break;
            //        }
            //    }
            //}

            _entities[id] = Entity2.empty;
        }

        public void destroy(ushort id)
        {
            if (id == 0)
                throw new Exception("can not delete the root node");

            if (!_entityFlags[id].inUse)
                throw new Exception("entity id was not in use");

            Console.WriteLine("destroying {0} {1}", id, _entities[id]);
            var firstchild = _entities[id].firstchild;
            while (_entities[id].firstchild != 0)
                destroy(_entities[id].firstchild);

            _entityFlags[id].inUse = false;
            removeChild(id);
            foreach (var d in data)
                d.Value.clear(id);
            //_entities[id] = Entity2.empty;
            _freeEntities[_freeIndex] = id;
            _freeIndex++;
        }

        public void set<T>(ushort id, T t)
            where T : struct
        {
            var type = typeof(T);
            IComponentData icdata;
            ComponentData<T> cdata;
            if (!data.TryGetValue(type, out icdata))
            {
                cdata = new ComponentData<T>();
                cdata.init();
                icdata = cdata;
                data.Add(type, cdata);
            }
            else
            {
                cdata = (ComponentData<T>)icdata;
            }

            cdata.set(id, t);
            //position.setData(id, p);
        }


        public ushort clear<T>(ushort id)
            where T : struct
        {
            var type = typeof(T);
            IComponentData icdata;
            if (data.TryGetValue(type, out icdata))
                icdata.clear(id);

            return id;
        }

        public bool has<T>(ushort id)
            where T : struct
        {
            var type = typeof(T);
            IComponentData icdata;
            if (!data.TryGetValue(type, out icdata))
                return false;

            return icdata.has(id);
        }

        public T get<T>(ushort id)
            where T : struct
        {
            var type = typeof(T);
            IComponentData icdata;
            if (!data.TryGetValue(type, out icdata))
                return default(T);

            var cdata = (ComponentData<T>)icdata;
            return cdata.get(id);
        }
    }


    public class Program
    {
        public static void addpositionchange(ReadOnlyCollection<int> ids)
        {
            foreach (var id in ids)
                Console.WriteLine("----------" + id);
        }

        public static void speedtest()
        {
            var sw = new Stopwatch();
            var k = 0;
            sw.Start();
            for (var i = 0; i < 1000000; i++)
            {
                k++;
            }
            sw.Stop();
            Console.WriteLine(string.Format("{0} - {1}", k, sw.Elapsed));

        }

        public static void Main()
        {

            speedtest();
            speedtest();
            speedtest();
            speedtest();
            speedtest();

            var em = new EntityManager2();
            em.init();

            var jp = new PrefabFactory();
            jp.process(em, JsonReader.ParseObject("{\"position\": { \"translation\" : {\"x\": 2, \"y\":3}}}"));
            jp.process(em, JsonReader.ParseObject("{\"position\": { \"translation\" : {\"x\": 4, \"y\":1}}}"));
            jp.process(em, JsonReader.ParseObject("{\"position\": { \"translation\" : {\"x\": 1, \"y\":4}}}"));
            jp.process(em, JsonReader.ParseObject("{\"position\": { \"translation\" : {\"x\": 8, \"y\":9}}}"));

            printtree<Position>(em, 0, 0);
            Console.Read();
            return;

            var id = em.create();
            var child1 = em.create(id);
            var child2 = em.create(id);
            em.destroy(id);

            //em.destroy(id);
            id = em.create();
            //            em.destroy(1);
            id = em.create();
            id = em.create(id);
            id = em.create(2);
            id = em.create(2);
            id = em.create(2);
            id = em.create();
            printtree(em, 0);
            em.destroy(1);
            id = em.create();

            em.set(id, new Position() { translation = new Vector2(16f, 16f) });
            em.set(id, new Position() { translation = new Vector2(16f, 16f) });
            Console.WriteLine("hasData: {0}", em.has<Position>(id));
            Console.WriteLine("getData: {0}", em.get<Position>(id));
            //Console.WriteLine("clearData: {0}", em.deleteData(id));
            Console.WriteLine("hasData: {0}", em.has<Position>(id));
            Console.WriteLine("getData: {0}", em.get<Position>(id));
            //Console.WriteLine("clearData: {0}", em.deleteData(id));

            id = em.create(id);
            em.set(id, new Position() { translation = new Vector2(16f, 16f) });
            Console.WriteLine("hasData: {0}", em.has<Position>(id));
            Console.WriteLine("getData: {0}", em.get<Position>(id));
            //Console.WriteLine("clearData: {0}", em.deleteData(id));
            Console.WriteLine("hasData: {0}", em.has<Position>(id));
            Console.WriteLine("getData: {0}", em.get<Position>(id));
            //Console.WriteLine("clearData: {0}", em.deleteData(id));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            em.destroy(1);
            //run(em, 0, Vector2.Zero, 0);
            id = em.create();
            em.set(id, new Position() { translation = new Vector2(16f, 16f) });
            id = em.create();
            em.set(id, new Position() { translation = new Vector2(16f, 16f) });
            em.destroy(1);
            id = em.create();
            em.set(id, new Position() { translation = new Vector2(16f, 16f) });
            em.destroy(1);
            em.destroy(2);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            var r = new Random();
            //            var testing = new List<ushort>(new ushort[4]);
            for (var k = 0; k < 16; k++)
            {
                for (var i = 0; i < 256; i++)
                {
                    var add = r.Next(0, em.totalEntities - 1) + 1;
                    var parent = r.Next(0, 5) == 0 ? 0 : get(em, (ushort)0, ref add);

                    var _id = (ushort)0;
                    //if (parent == 0)
                    //    _id = em.create();
                    //else
                    _id = em.create((ushort)parent);


                    if (r.Next(2) == 0)
                        em.set(_id, new Position() { translation = new Vector2(r.NextDouble() * 16f, r.NextDouble() * 16f) });
                }

                var loops = r.Next(4, 16);
                for (var d = 0; d < loops; d++)
                {
                    var delete = r.Next(1, em.totalEntities / 2);
                    var _delete = delete;
                    var index = get(em, (ushort)0, ref _delete);

                    if (index != 0)
                        em.destroy(index);
                    else
                    {
                        // continue;
                    }
                    //testing.RemoveAt(delete);
                }
            }
            var total = 0;
            var sw = new Stopwatch();
            sw.Start();
            total = run(ref em._entities);
            sw.Stop();
            Console.WriteLine("{0} - {1} - {2}", em.totalEntities, sw.Elapsed, total);
            sw.Reset();
            sw.Start();
            total = run(ref em._entities);
            sw.Stop();
            Console.WriteLine("{0} - {1} - {2}", em.totalEntities, sw.Elapsed, total);
            sw.Reset();
            sw.Start();
            total = run(ref em._entities);
            sw.Stop();
            Console.WriteLine("{0} - {1} - {2}", em.totalEntities, sw.Elapsed, total);
            sw.Reset();
            sw.Start();
            total = run(ref em._entities);
            sw.Stop();
            Console.WriteLine("{0} - {1} - {2}", em.totalEntities, sw.Elapsed, total);
            sw.Reset();

            //printtree(em, 0);
            Console.Read();
        }

        public static ushort get(EntityManager2 em, ushort index, ref int iteraton)
        {
            //if (index == 0)
            //{
            //    for (var i = 0; i < em._rootIndex; i++)
            //    {
            //        iteraton--;
            //        if (iteraton == 0)
            //            return em._rootEntities[i];

            //        var id = get(em, em._rootEntities[i], ref iteraton);
            //        if (id != 0)
            //            return id;
            //    }
            //}
            //else
            {
                var next = em._entities[index].firstchild;
                while (next != 0)
                {
                    iteraton--;
                    if (iteraton == 0)
                        return next;

                    var id = get(em, next, ref iteraton);
                    if (id != 0)
                        return id;

                    next = em._entities[next].next;
                }
            }

            return 0;
        }

        static ushort[] stack = new ushort[ushort.MaxValue];
        //var point = new ushort[ushort.MaxValue];
        public static int run(ref Entity2[] entities)
        //public static int run(EntityManager2 em)
        {
            //if (index == 0)
            //{
            //    for (var i = 0; i < em._rootIndex; i++)
            //    {
            //        if (em.position.hasData(em._rootEntities[i]))
            //        {
            //            var np = em.position.data[em.position.mappings[em._rootEntities[i]]];
            //            Console.WriteLine("{0} - {1}", em._rootEntities[i], (p + np.translation).ToString());
            //            run(em, em._rootEntities[i], p + np.translation, depth + 1);
            //        }
            //        else
            //        {
            //            Console.WriteLine("{0} - {1}", em._rootEntities[i], p.ToString());
            //            run(em, em._rootEntities[i], p, depth + 1);
            //        }
            //    }
            //}
            //else
            //var processing = new Stack<ushort>();
            //var stack = new ushort[ushort.MaxValue];
            //var point = new ushort[ushort.MaxValue];
            //var entities = em._entities;
            //var p = Vector2.Zero;
            var i = (uint)0;
            var total = 0;
            stack[i++] = 0;
            //for (i = 0; i < 8192; i++)
            //{
            //    stack[i] = i;
            //}

            //return 8192;
            //unsafe
            //{
            //    ushort* pStack = stackalloc ushort[ushort.MaxValue];
            //    *pStack = 0;
            //    pStack++;
            //    fixed (Entity2* pSrc = entities)
            //    {
            //        while (i > 0)
            //        {
            //            total++;
            //            pStack--;
            //            i--;
            //            var id = *pStack;
            //            var next = pSrc[id].firstchild;
            //            while (next != 0)
            //            {
            //                *pStack = next;
            //                pStack++;
            //                i++;
            //                next = pSrc[next].next;
            //            }
            //        }
            //    }
            //}

            //unsafe
            //{
            //    fixed (Entity2* pSrc = entities)
            //    fixed (ushort* baseStack = stack)
            //    {
            //        ushort* pStack = baseStack;

            //        *pStack = 0;
            //        pStack++;

            //        while (baseStack != pStack)
            //        {
            //            total++;

            //            pStack--;
            //            Entity2* e = (pSrc + *pStack);

            //            var next = e->firstchild;
            //            while (next != 0)
            //            {
            //                *pStack = next;
            //                pStack++;
            //                //stack[i++] = next;
            //                e = (pSrc + next);
            //                next = e->next;
            //            }
            //        }
            //    }
            //}


            stack[i++] = 0;

            while (i > 0 && i < ushort.MaxValue)
            {
                total++;

                var next = entities[stack[--i]].firstchild;
                while (next != 0)
                {
                    stack[i++] = next;
                    next = entities[next].next;
                }
            }

            return total;
        }

        public static void printtree(EntityManager2 em, int index)
        {
            //if (index == 0)
            //{
            //    for (var i = 0; i < em._rootIndex; i++)
            //    {
            //        printtree(em, em._rootEntities[i]);
            //    }
            //}
            //else
            {
                Console.Write(index + " - ");

                var next = em._entities[index].firstchild;
                while (next != 0)
                {
                    Console.Write(next + ", ");
                    next = em._entities[next].next;
                }

                Console.WriteLine();

                next = em._entities[index].firstchild;
                while (next != 0)
                {
                    printtree(em, next);
                    next = em._entities[next].next;
                }
            }
        }

        public static void printtree<T>(EntityManager2 em, ushort index, int depth)
            where T : struct
        {
            {
                Console.Write(new string('.', depth) + index);
                if (em.has<T>(index))
                    Console.Write(" - " + em.get<T>(index).ToString());

                Console.WriteLine();

                var next = em._entities[index].firstchild;
                while (next != 0)
                {
                    printtree<T>(em, next, depth + 1);
                    next = em._entities[next].next;
                }
            }
        }


        public static void Main2()
        {
            //var em = new EntityManager();
            //em.added += em_added;
            //em.removed += em_removed;
            //em.changed += em_changed;
            //em.addComponentChange<Position>(addpositionchange);

            //var ms = new Movement();
            //ms.init(em);

            //var id = em.create();
            //var p = em.add(id, new Position());
            //p.x = 10;
            //p.y = 15;

            //Console.WriteLine("added entity and component..");
            //em.update();
            //ms.update();

            //em.remove(id, p);

            //Console.WriteLine("removed component..");
            //em.update();
            //ms.update();

            //Console.WriteLine("added component back..");
            //em.add(id, p);
            //em.update();
            //ms.update();

            //Console.WriteLine("destroying entity..");
            //em.destroy(id);
            //em.update();
            //ms.update();

            ////em.add(id, p);

            //Console.Read();
            //return;

        }

        static void em_changed(int obj)
        {
            Console.WriteLine("changed entity {0}", obj);
        }

        static void em_removed(int obj)
        {
            Console.WriteLine("removed entity {0}", obj);
        }

        static void em_added(int obj)
        {
            Console.WriteLine("added entity {0}", obj);
        }
    }
}
