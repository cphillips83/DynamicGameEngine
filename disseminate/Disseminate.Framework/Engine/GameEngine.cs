using Disseminate.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Disseminate.Core;

namespace Disseminate.Engine
{
    public interface IGameEngine : IDisposable
    {
        bool isRunning { get; }
        //TimeSystem time { get; }

        void init();
        void start(IGameState startState);
        void update(double newTime);
        void render();
        void stop();
        
        void switchState(IGameState state);

        T get<T>() where T : class, ISystem;
        T put<T>(T t) where T : class, ISystem;
        void unload<T>(T t) where T : class, ISystem;
    }
    
}
