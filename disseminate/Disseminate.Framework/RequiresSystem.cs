using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate
{
    public abstract class RequiresSystem
    {
        private EntityManager _entityManager;
        protected HashSet<int> _entitiesMap = new HashSet<int>();
        protected HashSet<int> _changed = new HashSet<int>();
        protected HashSet<int> _added = new HashSet<int>();
        protected HashSet<int> _removed = new HashSet<int>();

        protected List<RequireNode> _nodes = new List<RequireNode>();

        protected Action<int> _update;

        protected struct RequireNode
        {
            public int id;
            public Action callback;

            public override int GetHashCode()
            {
                return id;
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;

                if (obj is RequireNode)
                    return ((RequireNode)obj).id == this.id;

                return false;
            }
        }

        protected void require<T0>(Action<T0> callback)
            where T0 : IComponent
        {
            if (_update != null)
                throw new Exception("update was already set");

            _update = new Action<int>((id) =>
            {
                Console.WriteLine("validating entity " + id);

                var shouldRemove = _removed.Contains(id);
                var shouldAdd = _added.Contains(id);

                var t0 = _entityManager.get<T0>(id);
                if (!shouldRemove && !shouldAdd)
                {
                    var exists = _entitiesMap.Contains(id);
                    if (t0 == null && exists)
                        shouldRemove = true;
                    else if (t0 != null && !exists)
                        shouldAdd = true;
                }

                if (shouldRemove)
                {
                    var index = _nodes.IndexOf(new RequireNode() { id = id });
                    _nodes[index] = _nodes[_nodes.Count - 1];
                    _nodes.RemoveAt(_nodes.Count - 1);
                    _entitiesMap.Remove(id);
                    //remove
                }
                else if (shouldAdd)
                {
                    _entitiesMap.Add(id);
                    var node = new RequireNode();
                    node.id = id;
                    node.callback = new Action(() =>
                    {
                        callback(t0);
                    });
                    _nodes.Add(node);
                    //add
                }
            });
        }

        protected void require<T0, T1>(Action<T0, T1> callback)
            where T0 : IComponent
            where T1 : IComponent
        {
            if (_update != null)
                throw new Exception("update was already set");

            _update = new Action<int>((id) =>
            {
                Console.WriteLine("validating entity " + id);
                var shouldRemove = _removed.Contains(id);
                var shouldAdd = _added.Contains(id);

                T0 t0; T1 t1;
                var has = _entityManager.get<T0, T1>(id, out t0, out t1);
                if (!shouldRemove && !shouldAdd)
                {
                    var exists = _entitiesMap.Contains(id);
                    if (!has && exists)
                        shouldRemove = true;
                    else if (has && !exists)
                        shouldAdd = true;
                }

                if (shouldRemove)
                {
                    var index = _nodes.IndexOf(new RequireNode() { id = id });
                    _nodes[index] = _nodes[_nodes.Count - 1];
                    _nodes.RemoveAt(_nodes.Count - 1);
                    _entitiesMap.Remove(id);
                    //remove
                }
                else if (shouldAdd)
                {
                    _entitiesMap.Add(id);
                    var node = new RequireNode();
                    node.id = id;
                    node.callback = new Action(() =>
                    {
                        callback(t0, t1);
                    });
                    _nodes.Add(node);
                    //add
                }
            });
        }


        private void em_changed(int id)
        {
            _changed.Add(id);
        }

        private void em_removed(int id)
        {
            if (_removed.Add(id))
                _changed.Add(id);
        }

        private void em_added(int id)
        {
            if (_added.Add(id))
                _changed.Add(id);
        }

        public void init(EntityManager em)
        {
            _entityManager = em;
            _entityManager.added += em_added;
            _entityManager.removed += em_removed;
            _entityManager.changed += em_changed;
            oninit();
        }

        public void update()
        {
            if (_update != null)
            {
                foreach (var id in _changed)
                    _update(id);
            }

            _changed.Clear();
            _removed.Clear();
            _added.Clear();

            onbeforeupdate();

            for (var i = 0; i < _nodes.Count; i++)
                _nodes[i].callback();

            onupdate();
        }

        public void destroy()
        {
            ondestroy();

            _entityManager.added -= em_added;
            _entityManager.removed -= em_removed;
            _entityManager.changed -= em_changed;

            _nodes.Clear();
            _changed.Clear();
            _added.Clear();
            _removed.Clear();
            _entitiesMap.Clear();
        }

        protected virtual void oninit() { }
        protected virtual void onbeforeupdate() { }
        protected virtual void onupdate() { }
        protected virtual void ondestroy() { }

    }
}
