using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Disseminate
{
    //public abstract class AbstractSystem<T0>
    //    where T0: IComponent
    //{
    //    protected EntityManager _entityManager;

    //    protected HashSet<int> _changes = new HashSet<int>();
    //    protected Dictionary<int, int> _entitiesMap = new Dictionary<int, int>();

    //    protected int _count = 0;
    //    protected int[] _entities = new int[64];
    //    protected T0[] _t0 = new T0[64];

    //    public AbstractSystem(EntityManager em)
    //    {
    //        _entityManager = em;
    //        _entityManager.added += em_added;
    //        _entityManager.removed += em_removed;
    //        _entityManager.changed += em_changed;
    //    }

    //    private void em_changed(int obj)
    //    {
    //        _changes.Add(obj);
    //    }

    //    private void em_removed(int obj)
    //    {
    //        _changes.Add(obj);
    //    }

    //    private void em_added(int obj)
    //    {
    //        _changes.Add(obj);
    //    }

    //    public void process()
    //    {
    //        foreach (var id in _changes)
    //            verify(id);

    //        _changes.Clear();
    //        onprocess();
    //    }

    //    private void verify(int id)
    //    {
    //        var t0 = _entityManager.get<T0>(id);
    //        var index = 0;
    //        var exists = _entitiesMap.TryGetValue(id, out index);

    //        if (t0 == null && exists)
    //        {
    //            _t0[index] = _t0[_count - 1];

    //            _count--;

    //            onremoved(id);
    //        }
    //        else if (t0 != null && !exists)
    //        {
    //            //
    //            onadded(id);
    //        }
    //    }

    //    protected virtual void onadded(int id) { }
    //    protected virtual void onremoved(int id) { }
    //    protected abstract void onprocess();
    //}

    //public class Movement : RequiresSystem
    //{
    //    protected override void oninit()
    //    {
    //        require<Position>((p) =>
    //        {
    //            Console.WriteLine(string.Format("processed: {0}:{1}", p.x, p.y));
    //        });
    //    }
    //}

    //public class Position : IComponent
    //{
    //    public float x = 0;
    //    public float y = 0;
    //}




    public interface IComponent
    {

    }

    public class Compositor<T>
        where T : IComponent
    {
        public Compositor(EntityManager em)
        {

        }
    }
}
