using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.SRPG.Scripts
{
    public class Player : Script
    {
        private TurnManager turnManager;
        private Core core;
        private int turnId = -1;
        private bool canMove = true;
        private bool canAttack = true;


        private void init()
        {
            turnManager = Root.instance.RootObject.getScript<TurnManager>();
            core = this.gameObject.getScript<Core>();

            turnId = turnManager.registerAction(this.gameObject, core.speed);
        }

        private void executeTurn()
        {
            //show command menu
            if (canMove && gui.buttonold(AxisAlignedBox.FromRect(10, 10, 100, 20), "attack"))
            {
                doAttack();
            }
            else if (canAttack && gui.buttonold(AxisAlignedBox.FromRect(10, 40, 100, 20), "move"))
            {
                doMove();
            }
            else if (gui.buttonold(AxisAlignedBox.FromRect(10, 40, 100, 20), "done"))
            {
                canMove = true;
                canAttack = true;
                endTurn();
            }

        }

        private void doAttack()
        {

        }

        private void doMove()
        {

        }

        private void endTurn()
        {
            turnId = turnManager.registerAction(this.gameObject, core.speed);
            turnManager.endTurn();
        }

        private void render()
        {

        }
    }
}
