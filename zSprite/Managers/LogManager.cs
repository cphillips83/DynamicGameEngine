using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zSprite.Managers
{
    public sealed class LogManager : AbstractManager
    {
        internal LogManager()
        {

        }

        public void assert(bool cond, string message)
        {
            if (cond)
                throw new Exception(message);
        }

        public void assert(bool cond, string message, params object[] args)
        {
            assert(cond, string.Format(message, args));
        }


        public void debug( string message)
        {

        }

        public void debug(string message, params object[] args)
        {
            debug(string.Format(message, args));
        }

    }
}
