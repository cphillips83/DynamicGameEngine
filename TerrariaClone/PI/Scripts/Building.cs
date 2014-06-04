using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.PI.Scripts
{
    public class Building : Script
    {
        public Sprite sprite;

        //private bool isBuilding = true;
        private float buildTimeRemaining = 5;
        private float buildTimer = 5;
        
        public bool isBuilding { get { return buildTimeRemaining > 0; } }

        public void setBuildTime(float time)
        {
            buildTimer = time;
            buildTimeRemaining = time;
        }

        private void update()
        {
            if (isBuilding)
                updateBuild();
            else
                updateBuilt();
        }

        protected virtual void updateBuild()
        {
            var time = Root.instance.time;
            buildTimeRemaining -= time.deltaTime;
            if (buildTimeRemaining <= 0f)
                built();
        }

        protected virtual void updateBuilt()
        {

        }

        protected virtual void built()
        {

        }

        private void render()
        {
            if (buildTimeRemaining > 0)
                renderBuild();
            else
                renderBuilt();
        }

        protected virtual void renderBuild()
        {
            var g = Root.instance.graphics;
            var p = transform.DerivedPosition;
            var s = new Vector2(p.X - 50, p.Y);
            var e = new Vector2(p.X + 50, p.Y);
            var l = 1f - buildTimeRemaining / buildTimer;
            g.Draw(null, AxisAlignedBox.FromRect(s.X, s.Y - 3, 100, 6), Color.Red);
            g.Draw(null, AxisAlignedBox.FromRect(s.X, s.Y - 3, 100 * l, 6), Color.Green);
        }

        protected virtual void renderBuilt()
        {

        }

        private void ongui()
        {

        }
    }

}
