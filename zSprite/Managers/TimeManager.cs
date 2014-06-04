using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zSprite.Managers
{
    public sealed class TimeManager : AbstractManager
    {
        public float time { get; private set; }
        public double timeD { get; private set; }

        public float deltaTime { get; private set; }
        public double deltaTimeD { get; private set; }

        public float fixedTime { get; private set; }
        public double fixedTimeD { get; private set; }

        public float fixedDeltaTime { get { return (float)fixedDeltaTimeD; } set { fixedDeltaTimeD = value; } }
        public double fixedDeltaTimeD { get; set; }

        public float maximumDeltaTime { get { return (float)maximumDeltaTimeD; } set { maximumDeltaTimeD = value; } }
        public double maximumDeltaTimeD { get; set; }

        public float smoothedTimeDelta { get; private set; }
        public double smoothedTimeDeltaD { get; private set; }

        public double timeScale { get; set; }

        public float fixedFPS { get { return 1f / fixedDeltaTime; } set { fixedDeltaTimeD = 1.0 / value; } }

        public int frameCount { get; private set; }

        public double realtimeSinceStartup { get; private set; }
        public double realtimeSinceStartupD { get; private set; }

        private double timeStepsTotal = 0;
        private Queue<double> timeSteps = new Queue<double>();

        internal TimeManager()
        {

        }

        internal void init()
        {
            fixedDeltaTimeD = 1.0 / 60.0;
            maximumDeltaTimeD = fixedDeltaTimeD * 5.0;
            timeScale = 1;
        }

        internal void update(double newTime, double step)
        {
            var dtd = step * timeScale;
            var dt = (float)(step * timeScale);

            realtimeSinceStartup = (float)newTime;
            realtimeSinceStartupD = newTime;

            time += dt;
            timeD += dtd;
            deltaTime = dt;
            deltaTimeD = dtd;

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

        internal void updateFixed(double step)
        {
            var dt = (float)(step * timeScale);
            var dtd = timeScale != 1 ? step * timeScale : step;

            deltaTime = dt;
            deltaTimeD = dtd;
            fixedTime += dt;
            fixedTimeD += dtd;
        }
    }
}
