using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.Collections;

namespace GameName1.SRPG.Scripts
{
    public class TurnManager : Script
    {
        private static int turnId = 0;

        private interface IAction
        {
            int getExecuteTime();
            void Execute();
        }

        private struct Turn : IAction
        {
            public int id ;
            public int turn;
            public int speed;
            public GameObject gameObject;

            public int getExecuteTime()
            {
                return turn + speed;
            }

            public void Execute()
            {
                gameObject.sendMessage("executeTurn");
            }
        }

        private PriorityQueue<int, IAction> actionList = new PriorityQueue<int, IAction>();
        private int currentTurn = 0;

        public int CurrentTurn { get { return currentTurn; } }

        //private List<Turn> actions = new List<Turn>();
        //private List<Turn> characters = new List<Turn>();

        //private IEnumerable<Turn> allTurns()
        //{
        //    foreach (var a in actions)
        //        yield return a;

        //    foreach (var c in characters)
        //        yield return c;
        //}

        public int registerAction(GameObject go, int speed)
        {

            //var currentTime = Root.instance.time.time;
            //var actionTime = currentTime + speed;
          
            var nextid = turnId++;
            var turn = new Turn() { id = nextid, gameObject = go, speed = speed, turn = currentTurn };
            actionList.Enqueue(turn.getExecuteTime(), turn);

            return nextid;
        }

        public void cancelAction(int id)
        {

        }

        public void endTurn()
        {
            actionList.Dequeue();
        }

        private void update()
        {
            if (actionList.Count > 0)
            {
                var item = actionList.Peek();

                currentTurn = item.Key;
                item.Value.Execute();
            }
        }

        private void render()
        {

            //show next 5 turns in the top right
        }
    }
}
