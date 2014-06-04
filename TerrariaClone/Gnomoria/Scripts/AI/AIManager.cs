using GameName1.Gnomoria.Scripts.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using GameName1.Gnomoria.Scripts.GameObjects.Characters;

namespace GameName1.Gnomoria.Scripts.AI
{
    public class AIManager : Script
    {
        public List<ZombieWorker> workers = new List<ZombieWorker>();
        private WorldManager world;

        private void init()
        {
            world = Root.instance.RootObject.getScript<WorldManager>();
        }

        private void update()
        {
            foreach (var worker in workers.Where(x=>x.isIdle))
            {
                //do something
            }
        }

        public void addWorker(ZombieWorker worker)
        {
            workers.Add(worker);
        }
    }
}
