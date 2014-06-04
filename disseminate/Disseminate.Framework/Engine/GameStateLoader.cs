using Disseminate.Systems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Disseminate.Engine
{
    public abstract class GameStateLoader : IGameState
    {
        protected IGameEngine _gameEngine;
        protected IProcessor _loader;
        protected Queue<IProcessor> _loaders = new Queue<IProcessor>();
        protected Stopwatch _stopwatch = new Stopwatch();
        protected int _totalsteps = 0;
        protected int _currentstep = 0;

        protected Time time;

        public void init(IGameEngine engine)
        {
            _gameEngine = engine;
            time = _gameEngine.get<Time>();
            oninit();

            _loaders.Enqueue(null);
            _loader = _loaders.Dequeue();
        }

        protected abstract void oninit();

        protected void addLoader(IProcessor loader)
        {
            _totalsteps++;
            _loaders.Enqueue(loader);
            loader.start();
        }

        public void update()
        {
            _stopwatch.Reset();
            _stopwatch.Start();
            
            if(_loader != null)
            //while (_loader != null && _stopwatch.Elapsed.TotalSeconds < time.maximumDelta / 2f)
            {
                if (_loader.step())
                    _loader = _loaders.Dequeue();
            }
            
            _stopwatch.Stop();

            if (_loader == null)
                oncomplete();
        }

        protected abstract void oncomplete();

        public void render()
        {
            if (_loader != null)
            {
                var progress = _loader.progress;
                var overall = (float)_currentstep / (float)_totalsteps + (progress / _totalsteps);
                onrender(_currentstep, _totalsteps, progress, overall, _loader.state);
            }
        }

        protected abstract void onrender(int current, int total, float progress, float overallProgress, string step);

        public void Dispose()
        {
            ondispose();
        }

        protected abstract void ondispose();
    }
}
