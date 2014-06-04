using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite;

namespace GameName1.Digger.Components
{
    public class Physic : Script
    {
        //http://gamedev.stackexchange.com/questions/24603/how-to-wire-finite-state-machine-into-script-based-architecture

        private bool _needsupdate = false;

        private float _width = 1f;
        public float width
        {
            get { return _width; }
            set
            {
                _width = value;
                NeedsUpdate();
            }
        }

        private float _height = 1f;
        public float height
        {
            get { return _height; }
            set
            {
                _height = value;
                NeedsUpdate();
            }
        }

        private float _boundingRadius = 1f;
        public float BoundingRadius
        {
            get
            {
                if (_needsupdate)
                    Update();

                return _boundingRadius;
            }
        }

        private void NeedsUpdate()
        {
            _needsupdate = true;
        }

        private void Update()
        {
            _boundingRadius = (new Vector2(width, height) / 2f).Length();

            _needsupdate = false;
        }

        private void fixedupdate()
        {

        }
    }
}
