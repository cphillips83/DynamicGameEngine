using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Disseminate
{
    public class EntityComponentChange
    {
        public event Action<ReadOnlyCollection<int>> changed;

        private ReadOnlyCollection<int> _readonly;
        private List<int> _entities;

        public void add(int id)
        {
            if (_readonly == null)
            {
                _entities = new List<int>();
                _readonly = new ReadOnlyCollection<int>(_entities);
            }

            _entities.Add(id);
        }

        public void update()
        {
            if (_readonly != null)
            {
                if (changed != null)
                    changed(_readonly);

                _entities.Clear();
            }
        }
    }

    public class EntityManager : IEntityManager
    {
        public static int _nextid;

        public event Action<int> added;
        public event Action<int> removed;
        public event Action<int> changed;

        //public event Action<int, IComponent> componentadded;
        //public event Action<int, IComponent> componentremoved;

        //private List<int> _entities = new List<int>(1024);
        protected Dictionary<int, List<IComponent>> _entities = new Dictionary<int, List<IComponent>>();
        protected Dictionary<int, List<IComponent>> _newEntities = new Dictionary<int, List<IComponent>>();
        protected Dictionary<int, int> _childToParent = new Dictionary<int, int>();
        protected Dictionary<int, List<int>> _hierarchy = new Dictionary<int, List<int>>();
        //private List<int> _addedEntities = new List<int>(64);
        //private List<int> _removedEntities = new List<int>(64);
        //private List<int> _changedEntities = new List<int>(64);

        //private HashSet<int> _addedEntities = new HashSet<int>();
        protected List<int> _idCache = new List<int>();
        protected HashSet<int> _changedEntities = new HashSet<int>();
        protected HashSet<int> _removedEntities = new HashSet<int>();

        protected Dictionary<Type, EntityComponentChange> _changeEvents = new Dictionary<Type, EntityComponentChange>();

        public int create()
        {
            var id = _nextid++;
            _newEntities.Add(id, null);

            return id;
        }

        public int create(int parent)
        {
            var id = create();
            List<int> children;
            if (!_hierarchy.TryGetValue(parent, out children))
            {
                children = new List<int>();
                _hierarchy.Add(parent, children);
            }

            children.Add(id);
            _childToParent.Add(id, parent);
            return id;
        }

        public int create(params IComponent[] components)
        {
            var id = create();
            add(id, components);
            //_newEntities[id] = new List<IComponent>(components);

            return id;
        }

        public int create(int parent, params IComponent[] components)
        {
            var id = create(parent);
            add(id, components);
            //_newEntities[id] = new List<IComponent>(components);

            return id;
        }

        public void destroy(int id)
        {
            _removedEntities.Add(id);

            List<int> children;
            if (_hierarchy.TryGetValue(id, out children))
                for (var i = 0; i < children.Count; i++)
                    destroy(children[i]);
        }

        public T add<T>(int id, T t)
            where T : IComponent
        {
            var isNew = false;
            List<IComponent> components;
            if (!_entities.TryGetValue(id, out components))
            {
                if (!_newEntities.TryGetValue(id, out components))
                    throw new Exception("entity not found");

                isNew = true;
            }

            if (components == null)
            {
                components = new List<IComponent>();
                if (!isNew)
                    _entities[id] = components;
                else
                    _newEntities[id] = components;
            }

            if (components.Contains(t))
                throw new Exception("duplicate component");

            components.Add(t);
            componentChanged(typeof(T), id);

            if (!isNew)
                _changedEntities.Add(id);
            //if (changed != null && !_removedEntities.Contains(id) && !_addedEntities.Contains(id) && _changedEntities.Add(id))
            //if (changed != null)
            //    changed(id);

            return t;
        }

        public void add(int id, params IComponent[] _components)
        {
            var isNew = false;
            List<IComponent> components;
            if (!_entities.TryGetValue(id, out components))
            {
                if (!_newEntities.TryGetValue(id, out components))
                    throw new Exception("entity not found");

                isNew = true;
            }

            if (components == null)
            {
                components = new List<IComponent>();
                _entities[id] = components;
            }

            for (var i = 0; i < _components.Length; i++)
            {
                if (components.Contains(_components[i]))
                    throw new Exception("duplicate component");

                components.Add(_components[i]);
                componentChanged(components[i].GetType(), id);
            }

            if (!isNew)
                _changedEntities.Add(id);
            //if (changed != null && !_removedEntities.Contains(id) && !_addedEntities.Contains(id) && _changedEntities.Add(id))
            //if (changed != null)
            //    changed(id);

            //return t;
        }

        public void addComponentChange<T>(Action<ReadOnlyCollection<int>> listener)
            where T : IComponent
        {
            var type = typeof(T);
            EntityComponentChange ecc;
            if (!_changeEvents.TryGetValue(type, out ecc))
            {
                ecc = new EntityComponentChange();
                _changeEvents.Add(type, ecc);
            }
            ecc.changed += listener;
        }

        public void removeComponentChange<T>(Action<ReadOnlyCollection<int>> listener)
           where T : IComponent
        {
            var type = typeof(T);
            EntityComponentChange ecc;
            if (_changeEvents.TryGetValue(type, out ecc))
                ecc.changed -= listener;
        }

        private void componentChanged<T>(int id)
        {
            componentChanged(typeof(T), id);
        }

        private void componentChanged(Type type, int id)
        {
            EntityComponentChange ecc;
            if (_changeEvents.TryGetValue(type, out ecc))
                ecc.add(id);
        }

        public void remove<T>(int id, T t)
            where T : IComponent
        {
            var isNew = false;
            List<IComponent> components;
            if (!_entities.TryGetValue(id, out components))
            {
                if (!_newEntities.TryGetValue(id, out components))
                    throw new Exception("entity not found");

                isNew = true;
            }

            if (components == null || !components.Contains(t))
                throw new Exception("component not found");

            components.Remove(t);
            componentChanged(typeof(T), id);

            if (!isNew)
                _changedEntities.Add(id);

            //if (changed != null)
            //    changed(id);

            //return t;
        }

        public void remove(int id, params IComponent[] _components)
        {
            var isNew = false;
            List<IComponent> components;
            if (!_entities.TryGetValue(id, out components))
            {
                if (!_newEntities.TryGetValue(id, out components))
                    throw new Exception("entity not found");

                isNew = true;
            }

            for (var i = 0; i < _components.Length; i++)
            {
                if (components == null || !components.Contains(_components[i]))
                    throw new Exception("component not found");

                componentChanged(_components[i].GetType(), id);
                components.Remove(_components[i]);
            }

            if (!isNew)
                _changedEntities.Add(id);
        }

        public void update()
        {
            //var ids = new List<int>();
            foreach (var kvp in _newEntities)
            {
                _entities.Add(kvp.Key, kvp.Value);
                if (added != null)
                    _idCache.Add(kvp.Key);
            }

            _newEntities.Clear();

            if (added != null)
            {
                foreach (var id in _idCache)
                    added(id);

                _idCache.Clear();
            }

            foreach (var ecc in _changeEvents.Values)
                ecc.update();

            foreach (var id in _removedEntities)
            {
                _entities.Remove(id);
                if (removed != null)
                    _idCache.Add(id);
            }

            _removedEntities.Clear();

            if (removed != null)
            {
                foreach (var id in _idCache)
                {
                    var parent = 0;
                    if (_childToParent.TryGetValue(id, out parent))
                    {
                        //get parent list of ids and remove id
                        var children = _hierarchy[parent];
                        children.FastRemove(id);

                        //if children is empty, remove parent from hierarchy
                        if (children.Count == 0)
                            _hierarchy.Remove(parent);

                        //remove child to parent mapping
                        _childToParent.Remove(id);
                    }

                    removed(id);
                }

                _idCache.Clear();
            }

            if (changed != null)
            {
                _idCache.AddRange(_changedEntities);
                foreach (var id in _idCache)
                    changed(id);

                _idCache.Clear();
            }
        }

        private List<IComponent> getComponents(int id)
        {
            List<IComponent> components;
            if (!_entities.TryGetValue(id, out components))
            {
                if (!_newEntities.TryGetValue(id, out components))
                    return null;
            }

            return components;
        }

        public bool has<T0>(int id)
            where T0 : IComponent
        {
            var components = getComponents(id);
            if (components != null)
            {
                for (var i = 0; i < components.Count; i++)
                    if (components[i] is T0)
                        return true;
            }
            return false;
        }

        public bool hasany<T0, T1>(int id)
            where T0 : IComponent
            where T1 : IComponent
        {
            return has<T0>(id) || has<T1>(id);
        }

        public bool hasany<T0, T1, T2>(int id)
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
        {
            return has<T0>(id) || has<T1>(id) || has<T2>(id);
        }

        public bool hasall<T0, T1>(int id)
            where T0 : IComponent
            where T1 : IComponent
        {
            return has<T0>(id) && has<T1>(id);
        }

        public bool hasall<T0, T1, T2>(int id)
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
        {
            return has<T0>(id) && has<T1>(id) && has<T2>(id);
        }

        public T0 get<T0>(int id)
            where T0 : IComponent
        {
            var components = getComponents(id);
            if (components != null)
            {
                for (var i = 0; i < components.Count; i++)
                    if (components[i] is T0)
                        return (T0)components[i];
            }
            return default(T0);
        }

        public bool get<T0, T1>(int id, out T0 t0, out T1 t1)
            where T0 : IComponent
            where T1 : IComponent
        {
            t0 = default(T0);
            t1 = default(T1);

            var components = getComponents(id);
            if (components != null)
            {
                for (var i = 0; i < components.Count; i++)
                {
                    if (components[i] is T0)
                        t0 = (T0)components[i];
                    else if (components[i] is T1)
                        t1 = (T1)components[i];
                }
            }

            return t0 != null && t1 != null;
        }

        public bool get<T0, T1, T2>(int id, out T0 t0, out T1 t1, out T2 t2)
            where T0 : IComponent
            where T1 : IComponent
            where T2 : IComponent
        {
            t0 = default(T0);
            t1 = default(T1);
            t2 = default(T2);

            var components = getComponents(id);
            if (components != null)
            {
                for (var i = 0; i < components.Count; i++)
                {
                    if (components[i] is T0)
                        t0 = (T0)components[i];
                    else if (components[i] is T1)
                        t1 = (T1)components[i];
                    else if (components[i] is T2)
                        t2 = (T2)components[i];
                }
            }

            return t0 != null && t1 != null && t2 != null;
        }

        public int count { get { return _entities.Count; } }

        public IEnumerable<int> allEntities
        {
            get { return _entities.Keys; }
        }
    }
}
