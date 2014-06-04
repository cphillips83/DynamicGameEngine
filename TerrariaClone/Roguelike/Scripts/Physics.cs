using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Scripts
{

    public struct CollisionQuery
    {
        public int flag;
        public GameObject gameObject;
        public AxisAlignedBox aabb;

        public CollisionQuery(int flag, GameObject gameObject, AxisAlignedBox aabb)
        {
            this.flag = flag;
            this.gameObject = gameObject;
            this.aabb = aabb;
        }
    }

    public struct Collision
    {
        public int flag;
        public GameObject gameObject;
        public AxisAlignedBox aabb;

        public Collision(int flag, GameObject gameObject, AxisAlignedBox aabb)
        {
            this.flag = flag;
            this.gameObject = gameObject;
            this.aabb = aabb;
        }
    }

    public class Physics : Script
    {
        public const int QUERY_WORLD = 1;
        public const int QUERY_ENEMIES = 1 << 1;
        public const int QUERY_PLAYER = 1 << 2;

        public static bool CheckFlags(int flags, int query)
        {
            return (flags & query) == query;
        }

        public Vector2 _velocity = Vector2.Zero;
        public AxisAlignedBox shape = AxisAlignedBox.Null;

        private List<MinimumTranslationVector> collisions = new List<MinimumTranslationVector>();
        private List<Collision> broadphase = new List<Collision>();

        private void velocity(Vector2 velocity)
        {
            _velocity = velocity;
        }

        //private Vector2 checkCollision(Vector2 p)
        //{
        //    var attempts = 10;
        //    while (attempts-- > 0)
        //    {
        //        collisions.Clear();

        //        shape.Center = p;

        //        Root.instance.RootObject.broadcast("queryCollision", shape, collisions);

        //        if (collisions.Count > 0)
        //        {
        //            var mtv = collisions.OrderBy(x => x.overlap).First();
        //            if (mtv.intersects)
        //            {
        //                System.Diagnostics.Debug.WriteLine(mtv);
        //                return p + (float)(mtv.overlap) * mtv.smallest.normal;
        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //    return p;
        //}

        //private Vector2 processCollisions(Vector2 p, Vector2 d)
        //{
        //    var mtv = MinimumTranslationVector.Zero;
        //    var matched = AxisAlignedBox.Null;

        //    p += d;
        //    shape.Center = p;

        //    if (d != Vector2.Zero && broadphase.Count > 0)
        //    {

        //        var aabb = broadphase.OrderBy(x => (x.Center - p).LengthSquared()).First();
        //        {
        //            var testmtv = shape.collide(aabb);
        //            if (!mtv.intersects || testmtv.overlap < mtv.overlap)
        //            {
        //                //if (!matched.IsNull && mtv.overlap == testmtv.overlap &&
        //                //    (aabb.Center - p).LengthSquared() > (matched.Center - p).LengthSquared())
        //                //        continue;

        //                matched = aabb;
        //                mtv = testmtv;
        //            }
        //        }

        //        if (mtv.intersects)
        //        {
        //            System.Diagnostics.Debug.WriteLine(mtv);
        //            p += (float)mtv.overlap * mtv.smallest.normal;
        //        }
        //    }

        //    return p;
        //}

        private bool hasCollision(AxisAlignedBox check)
        {
            foreach (var c in broadphase)
                if (c.aabb.Intersects(check))
                    return true;

            return false;
        }

        private void fixedupdate()
        {
            if (!shape.IsNull)
            {
                var p = gameObject.transform.DerivedPosition;
                shape.Center = p;

                if (_velocity != Vector2.Zero)
                {
                    var m = _velocity * Root.instance.time.deltaTime;

                    var sweep = AxisAlignedBox.FromDimensions(shape.Center, shape.Size);
                    sweep.Center = p + m;
                    sweep.Merge(shape);

                    Root.instance.RootObject.broadcast("queryBroadphaseAABB", new CollisionQuery( QUERY_WORLD, this.gameObject, sweep), broadphase);

                    var dx = Math.Abs(m.X);
                    var dy = Math.Abs(m.Y);
                    var sx = Math.Sign(m.X);
                    var sy = Math.Sign(m.Y);
                    var px = new Vector2(sx, 0);
                    var py = new Vector2(0, sy);

                    while (dx > 0 || dy > 0)
                    {
                        if (dx > dy)
                        {
                            shape.Center = p + px;
                            var collided = hasCollision(shape);
                            if (collided && dy <= 0)
                                dx = 0;
                            if (!collided)
                                p.X += sx;
                            dx--;
                        }
                        else
                        {
                            shape.Center = p + py;
                            var collided = hasCollision(shape);
                            if (collided && dx <= 0)
                                dy = 0;
                            if (!collided)
                                p.Y += sy;
                            dy--;
                        }
                    }

                    shape.Center = p;
                    gameObject.transform.DerivedPosition = p;
                }
            }
            broadphase.Clear();
        }

        //private void fixedupdate1()
        //{
        //    if (!shape.IsNull && _velocity != Vector2.Zero)
        //    {
        //        broadphase.Clear();
        //        var m = _velocity * Root.instance.time.deltaTime;
        //        var p = gameObject.transform.DerivedPosition;
        //        shape.Center = p + m;

        //        Root.instance.RootObject.broadcast("queryBroadphase", shape, broadphase);

        //        if (Math.Abs(_velocity.X) > Math.Abs(_velocity.Y))
        //        {
        //            p = processCollisions(p, new Vector2(0, m.Y));
        //            p = processCollisions(p, new Vector2(m.X, 0));
        //        }
        //        else
        //        {
        //            p = processCollisions(p, new Vector2(m.X, 0));
        //            p = processCollisions(p, new Vector2(0, m.Y));
        //        }
        //        shape.Center = p;

        //        gameObject.transform.DerivedPosition = p;
        //    }
        //    else if (_velocity != Vector2.Zero)
        //    {
        //        gameObject.transform.DerivedPosition += _velocity * Root.instance.time.deltaTime;
        //    }
        //}

        //private void fixedupdate4()
        //{
        //    if (!shape.IsNull && _velocity != Vector2.Zero)
        //    {
        //        broadphase.Clear();
        //        Root.instance.RootObject.broadcast("queryBroadphase", shape, broadphase);

        //        var m = _velocity * Root.instance.time.deltaTime;
        //        var p = gameObject.transform.DerivedPosition;

        //        //if (Math.Abs(_velocity.X) > Math.Abs(_velocity.Y))
        //        {
        //            p = processCollisions(p, new Vector2(m.X, 0));
        //            p = processCollisions(p, new Vector2(0, m.Y));
        //        }
        //        //else
        //        //{
        //        //    p = processCollisions(p, new Vector2(0, m.Y));
        //        //    p = processCollisions(p, new Vector2(m.X, 0));
        //        //}

        //        gameObject.transform.DerivedPosition = p;
        //    }
        //    else if (_velocity != Vector2.Zero)
        //    {
        //        gameObject.transform.DerivedPosition += _velocity * Root.instance.time.deltaTime;
        //    }
        //}

        //private void fixedupdate2()
        //{
        //    //if (!shape.IsNull)
        //    //    shape.Center = gameObject.transform.DerivedPosition;
        //    //if (!shape.IsNull && _velocity != Vector2.Zero)
        //    //{
        //    //    var m = _velocity * Root.instance.time.deltaTime;
        //    //    var p = gameObject.transform.DerivedPosition;

        //    //    if (Math.Abs(_velocity.X) > Math.Abs(_velocity.Y))
        //    //    {
        //    //        p = checkCollision(p + new Vector2(0, m.Y));
        //    //        p = checkCollision(p + new Vector2(m.X, 0));
        //    //    }
        //    //    else
        //    //    {
        //    //        p = checkCollision(p + new Vector2(m.X, 0));
        //    //        p = checkCollision(p + new Vector2(0, m.Y));
        //    //    }

        //    //    gameObject.transform.DerivedPosition = p;
        //    //}
        //    //else if (_velocity != Vector2.Zero)
        //    {
        //        gameObject.transform.DerivedPosition += _velocity * Root.instance.time.deltaTime;
        //    }
        //}

        private void render()
        {
            //var line = new LineSegment(new Vector2(100, 100), new Vector2(300, 150));
            //var closest = line.closest(gameObject.transform.DerivedPosition);

            //Root.instance.graphics.DrawLine(3, null, null, line.p0, line.p1, Color.Orange);
            //Root.instance.graphics.DrawLine(3, null, null, closest, gameObject.transform.DerivedPosition, Color.Blue);
            Root.instance.graphics.DrawRect(shape, Color.Red);

        }
    }
}
