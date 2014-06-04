using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.Engine
{
    public interface IProcessor
    {
        string state { get; }
        float progress { get; }
        void start();
        bool step();
        //void stop();
    }


        
}
