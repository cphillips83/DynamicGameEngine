using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate
{
    public interface IEntitySystem
    {
        void init(EntityManager em);
        void update();
        void destroy();
    }
}
