using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Space
{
    [Flags]
    public enum TargetFilters : int
    {
        None = 0,
        Player,
        Aliens,
    }

    public class TargetableObjects : Script
    {
        private List<GameObject>[] _gameObjects = new List<GameObject>[32];

        public TargetableObjects()
        {
            for (var i = 0; i < 32; i++)
                _gameObjects[i] = new List<GameObject>();
        }

        public IEnumerable<GameObject> get(Vector2 from, float radius, TargetFilters filters)
        {
            var radiusSq = radius * radius;
            for (var i = 0; i < 32; i++)
            {
                var filter = (TargetFilters)(1 << i);
                if ((filter & filters) == filter)
                {
                    foreach (var go in _gameObjects[i])
                    {
                        var distanceSq = (from - go.transform.DerivedPosition).LengthSquared();
                        if (distanceSq < radiusSq)
                            yield return go;
                    }
                }
            }
        }

        public IEnumerable<GameObject> get(LineSegment line, float radius, TargetFilters filters)
        {
            for (var i = 0; i < 32; i++)
            {
                var filter = (TargetFilters)(1 << i);
                if ((filter & filters) == filter)
                {
                    foreach (var go in _gameObjects[i])
                    {
                        var p = go.transform.DerivedPosition;
                        var circle = new Circle(p, radius);
                        
                        if (circle.Intersects(line))
                            yield return go;
                    }
                }
            }
        }

        public void add(GameObject go, TargetFilters filters)
        {
            for (var i = 0; i < 32; i++)
            {
                var filter = (TargetFilters)(1 << i);
                if ((filter & filters) == filter)
                {
                    _gameObjects[i].Add(go);
                }
            }
        }

        public void remove(GameObject go, TargetFilters filters)
        {
            for (var i = 0; i < 32; i++)
            {
                var filter = (TargetFilters)(1 << i);
                if ((filter & filters) == filter)
                {
                    _gameObjects[i].Remove(go);
                }
            }
        }

    }
}
