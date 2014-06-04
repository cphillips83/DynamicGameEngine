using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite;

namespace GameName1.Space
{

    public class Ship1 : Script
    {
        private GameObject turret1;
        private GameObject turret2;
        private GameObject hardpoint;
        //private int trackingTargetId = -1;

        private void init()
        {
            turret1 = this.gameObject.createChild();
            turret2 = this.gameObject.createChild();
            hardpoint = this.gameObject.createChild();

            setupTurret(turret1, new Vector2(-25, 25));
            setupTurret(turret2, new Vector2(25, 25));
            //hardpoint.transform().Position = new Vector2(0, -25);
        }

        private void setupTurret(GameObject go, Vector2 pos)
        {
            var _transform = go.transform2();
            _transform.Position = pos;

            var track = go.createScript<Track>();
            
            var direction = go.createScript<ShowDirection>();
            direction.length = 100f;
            direction.color = Color.Red;
        }
    }


    //mount type
    // hard point
    // turret

    //attack type
    // laser
    // missile
    // canon
    public class WeaponSystem : Script
    {

        public void addturrent(Vector2 position, float speed)
        {

        }

    }
}
