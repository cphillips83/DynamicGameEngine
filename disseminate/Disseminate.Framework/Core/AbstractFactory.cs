using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.Core
{
    public abstract class AbstractFactory<T> : IAbstractFactory<T>
        where T: class
    {
        /// <summary>
        /// The factory type.
        /// </summary>
        virtual public string Type { get { return typeof(T).Name; } protected set { throw new NotImplementedException(); } }

        /// <summary>
        /// Creates a new object.
        /// </summary>
        /// <param name="name">Name of the object to create</param>
        /// <returns>
        /// An object created by the factory. The type of the object depends on
        /// the factory.
        /// </returns>
        public abstract T CreateInstance(string name);
    
        /// <summary>
        /// Destroys an object which was created by this factory.
        /// </summary>
        /// <param name="obj">the object to destroy</param>
        public abstract void DestroyInstance(ref T obj);

    }
}
