using Disseminate.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.Engine
{
    public interface IGameState : IDisposable
    {
        void init(IGameEngine engine);
        void update();
        void render();
    }
}
