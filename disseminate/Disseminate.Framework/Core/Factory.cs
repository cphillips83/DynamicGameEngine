using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.Core
{
    public abstract class Factory<T>
        where T: new()
    {
        protected static T _instance;
        public static T instance { get { return _instance; } }

        protected abstract T getInstance();

        public void update()
        {
            _instance = getInstance();
            onupdate();
        }

        protected virtual void onupdate()
        {

        }
    }
}
