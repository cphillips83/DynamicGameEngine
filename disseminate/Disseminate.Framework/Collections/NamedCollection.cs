using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.Collections
{
    public class NamedCollection<T> : Dictionary<string, T>
    {
        #region Constants

        private const int InitialCapacity = 60;

        #endregion Constants

        #region Readonly & Static Fields

        protected static int nextUniqueKeyCounter;

        protected string typeName;

        #endregion Readonly & Static Fields

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        public NamedCollection()
            : this(InitialCapacity)
        {
        }

        public NamedCollection(int initialCap)
            : base(initialCap)
        {
            this.typeName = typeof(T).Name;
        }

        #endregion Constructors

        #region Instance Methods

        /// <summary>
        ///	Adds an unnamed object to the <see cref="AxiomCollection{T}"/> and names it manually.
        /// </summary>
        /// <param name="item">The object to add.</param>
        virtual public void Add(T item)
        {
            Add(typeName + (nextUniqueKeyCounter++), item);
        }

        /// <summary>
        /// Adds multiple items from a specified source collection
        /// </summary>
        /// <param name="from"></param>
        virtual public void AddRange(IDictionary<string, T> source)
        {
            foreach (KeyValuePair<string, T> entry in source)
            {
                this.Add(entry.Key, entry.Value);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="AxiomCollection{T}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the <see cref="AxiomCollection{T}"/> values.</returns>
        virtual new public IEnumerator GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        public T this[int index]
        {
            get
            {
                foreach (T item in Values)
                {
                    if (index == 0)
                    {
                        return item;
                    }
                    index--;
                }
                return default(T);
            }
        }

        #endregion Instance Methods
    }
}
