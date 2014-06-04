using Disseminate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disseminate.Systems
{
    public interface ITime : ISystem
    {
        float total { get; }
        float delta { get; }
        float fps { get; }        
        //int ticks{ get; }
    }

    public class Time : ITime
    {
        protected double _accumulator = 0.0;

        public float total { get; private set; }
        public double totalD { get; private set; }

        public float delta { get; private set; }
        public double deltaD { get; private set; }

        //public int ticks { get; private set; }
        //public float fixedTime { get; private set; }
        //public double fixedTimeD { get; private set; }

        //public float fixedDeltaTime { get { return (float)fixedDeltaTimeD; } set { fixedDeltaTimeD = value; } }
        //public double fixedDeltaTimeD { get; set; }

        public float maximumDelta { get { return (float)maximumDeltaD; } set { maximumDeltaD = value; } }
        public double maximumDeltaD { get; set; }

        public float smoothedTimeDelta { get; private set; }
        public double smoothedTimeDeltaD { get; private set; }

        public double timeScale { get; set; }

        //public float fixedFPS { get { return 1f / fixedDeltaTime; } set { fixedDeltaTimeD = 1.0 / value; } }
        public float fps { get { return 1f / delta; } }

        public int frameCount { get; private set; }

        public double realtimeSinceStartup { get; private set; }
        public double realtimeSinceStartupD { get; private set; }

        
        protected double timeStepsTotal = 0;
        protected Queue<double> timeSteps = new Queue<double>();

        //public void setGameTime(float 

        //public void setTicks(int ticks)
        //{
        //    this.ticks = ticks;  
        //}
        public Time()
        {
            timeScale = 1;
            var fixedDeltaTimeD = 1.0 / 60.0;
            maximumDeltaD = fixedDeltaTimeD * 5.0;
        }

        public void init()
        {
            //fixedDeltaTimeD = 1.0 / 60.0;
            //maximumDeltaTimeD = fixedDeltaTimeD * 5.0;
            timeScale = 1;
        }


        public void update(double newTime)
        {
            double frameTime = newTime - realtimeSinceStartupD;

            // note: max frame time to avoid spiral of death          
            if (frameTime > maximumDeltaD * timeScale)
                frameTime = maximumDeltaD * timeScale;

            step(newTime, frameTime);
            //_accumulator += frameTime;

            //if (_accumulator >= fixedDeltaTimeD)
            //{
            //    while (_accumulator >= fixedDeltaTimeD)
            //    {
            //        _accumulator -= fixedDeltaTimeD;


            //    }
            //}
        }

        private void step(double newTime, double step)
        {
            var dtd = step * timeScale;
            var dt = (float)(step * timeScale);

            realtimeSinceStartup = (float)newTime;
            realtimeSinceStartupD = newTime;

            total += dt;
            totalD += dtd;
            delta = dt;
            deltaD = dtd;

            if (timeSteps.Count == 0)
            {
                for (var i = 0; i < 5; i++)
                {
                    timeSteps.Enqueue(step);
                    timeStepsTotal += dtd;
                }
            }

            var oldStep = timeSteps.Dequeue();
            timeStepsTotal -= oldStep;
            timeStepsTotal += step;
            timeSteps.Enqueue(step);

            smoothedTimeDeltaD = timeStepsTotal / timeSteps.Count;

            frameCount++;
        }

        //public void updateFixed(double step)
        //{
        //    var dt = (float)(step * timeScale);
        //    var dtd = timeScale != 1 ? step * timeScale : step;

        //    deltaTime = dt;
        //    deltaTimeD = dtd;
        //    //fixedTime += dt;
        //    //fixedTimeD += dtd;
        //}

        public void Dispose()
        {

        }
    }

}
