using GameName1.Roguelike.Systems.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.AI.Pathfinding;

namespace GameName1.Roguelike.Scripts
{

    public class SimpleAI : Script
    {
        private enum aistate
        {
            idle,
            moving,
            findingmove,
            computingmove,
            fail,
        }

        private static Random rand = new Random();
        private aistate state = aistate.idle;
        private List<int> pathing = new List<int>();
        private float idleTimer = 0f;
        private AStar pathfinder = null;
        private Map map = null;

        private float attackDelay = 2f;
        private float attackTimer = 0f;
        private Weapon weapon;
        private void init()
        {
            map = Root.instance.RootObject.getScript<Map>();
            pathfinder = new AStar(map);
            //var fiber = new Fiber(
            weapon = new Systems.Items.Projectile();
            weapon.texture = "content/textures/ship.png";
            weapon.flags = Physics.QUERY_PLAYER;
            weapon.duration = 3;
            
        }

        private void fixedupdate()
        {
            switch (state)
            {
                case aistate.idle:
                    idleTimer -= Root.instance.time.deltaTime;

                    if (idleTimer <= 0f)
                        state = aistate.findingmove;

                    break;
                case aistate.findingmove:
                    var checks = 25;
                    while (checks-- > 0)
                    {
                        var x = rand.Next(0, map.width);
                        var y = rand.Next(0, map.height);

                        var tile = map.getTile(x, y);
                        if (tile.Movable)
                        {
                            //x *= Map.tileSize * Game.WORLD_SCALE;
                            //y *= Map.tileSize * Game.WORLD_SCALE;
                            var p0 = map.worldToMap(gameObject.transform.DerivedPosition);
                            var p1 = new Point(x, y);
                            pathfinder.init((p0.X << 16) + p0.Y, (p1.X << 16) + p1.Y);
                            state = aistate.computingmove;
                            break;
                        }
                    }
                    break;
                case aistate.computingmove:
                    if (pathfinder.step(50))
                    {
                        if (pathfinder.foundPath)
                        {
                            pathing = pathfinder.getPath();
                            state = aistate.moving;
                            attackTimer = attackDelay;
                        }
                        else
                        {
                            state = aistate.findingmove;
                        }
                    }
                    break;
                case aistate.moving:
                    if (pathing.Count == 0)
                    {
                        state = aistate.idle;
                        idleTimer = (float)rand.NextDouble() * 4f + 1f;
                    }
                    else
                    {
                        attackTimer -= Root.instance.time.deltaTime;
                        if (attackTimer <= 0f)
                        {
                            attackTimer = attackDelay;
                            weapon.attack(gameObject, Root.instance.RootObject.find("player").transform.DerivedPosition);
                        }

                        var next = pathing[pathing.Count - 1];
                        var swp = gameObject.transform.DerivedPosition;
                        var dwp = map.mapToWorld(new Point(next >> 16, next & 0xffff));

                        var len = (dwp - swp);
                        if ((Map.tileSize * Game.WORLD_SCALE / 2) * (Map.tileSize * Game.WORLD_SCALE / 2) > len.LengthSquared())
                        {
                            pathing.RemoveAt(pathing.Count - 1);
                            break;
                        }

                        var dir = len.ToNormalized();
                        //swp += dir * 100 * Root.instance.time.deltaTime;
                        //gameObject.transform.DerivedPosition = swp;

                        gameObject.sendMessage("velocity", dir * 100 * Root.instance.time.deltaTime);
                    }
                    break;
                case aistate.fail:
                    break;
            }
        }

        private void lockmovement(float time)
        {
            if (idleTimer <= time || state != aistate.idle)
            {
                idleTimer = time;
                state = aistate.idle;
            }
        }

        private double lastImmunity = 0;
        private float immunityWindow = 0.25f;
        private void applydamage(GameObject attacker, float amt)
        {
            if (Root.instance.time.fixedTimeD > lastImmunity + immunityWindow)
            {
                lastImmunity = Root.instance.time.fixedTimeD;

                var go = Root.instance.RootObject.createChild(gameObject.name + " - scrollDamage");
                go.transform.DerivedPosition = gameObject.transform.DerivedPosition;

                var text = go.createScript<ScrollText>();
                text.color = Color.Red;
                text.fadeColor = new Color(Color.Red, 0f);
                text.text = amt.ToString();
                text.renderQueue = 10;
            }
        }
    }
}
