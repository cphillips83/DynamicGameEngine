using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.Core
{
    public class DisseminateException : Exception
    {
        public DisseminateException() : base() { }
        public DisseminateException(string msg) : base(msg) { }
        public DisseminateException(string msg, params object[] args) : base(string.Format(msg, args)) { }
    }

    public class AxiomException : Exception
    {
        public AxiomException() : base() { }
        public AxiomException(string msg) : base(msg) { }
        public AxiomException(string msg, params object[] args) : base(string.Format(msg, args)) { }
    }
}
