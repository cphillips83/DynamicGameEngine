using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.Core
{
    public interface IAbstractFactory<T>
    {
        /// <summary>
        /// The factory type.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Creates a new object.
        /// </summary>
        /// <param name="name">Name of the object to create</param>
        /// <returns>
        /// An object created by the factory. The type of the object depends on
        /// the factory.
        /// </returns>
        T CreateInstance(string name);

        /// <summary>
        /// Creates a new object.
        /// </summary>
        /// <param name="name">Name of the object to create</param>
        /// <param name="parms">List of Name/Value pairs to initialize the object with</param>
        /// <returns>
        /// An object created by the factory. The type of the object depends on
        /// the factory.
        /// </returns>
        //T CreateInstance( string name, NameValuePairList parms );
        /// <summary>
        /// Destroys an object which was created by this factory.
        /// </summary>
        /// <param name="obj">the object to destroy</param>
        void DestroyInstance(ref T obj);
    }
}
