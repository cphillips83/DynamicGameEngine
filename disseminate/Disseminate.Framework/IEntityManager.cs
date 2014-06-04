using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate
{
    public interface IEntityManager
    {
        int count { get; }
        int create();
        int create(params IComponent[] components);
        void destroy(int id);
       
        T add<T>(int id, T t) where T: IComponent;
        void add(int id, params IComponent[] components);

        void remove<T>(int id, T t) where T : IComponent;
        void remove(int id, params IComponent[] components);

        void update();

        IEnumerable<int> allEntities { get; }        
    }
}
