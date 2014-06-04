using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keyboard = Microsoft.Xna.Framework.Input.Keys;
using zSprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameName1.Digger.Components
{
    public class Item
    {
        public virtual void equip(GameObject owner)
        {
            sendInformationalErrorMsg(owner, "You can not equip this item!");
        }

        //public virtual void equipleft(GameObject owner)
        //{
        //    sendInformationalErrorMsg(owner, "You can not equip this item!");
        //}

        //public virtual void equipright(GameObject owner)
        //{
        //    sendInformationalErrorMsg(owner, "You can not equip this item!");
        //}

        //public virtual void unequipleft(GameObject owner)
        //{
        //    sendInformationalErrorMsg(owner, "You can not unequip this item!");
        //}

        //public virtual void unequipright(GameObject owner)
        //{
        //    sendInformationalErrorMsg(owner, "You can not unequip this item!");
        //}

        public virtual void update()
        {

        }

        public virtual void activate(GameObject owner)
        {
            sendInformationalErrorMsg(owner, "You can not use this item!");
        }

        protected void sendInformationalErrorMsg(GameObject owner, string msg)
        {
            owner.sendMessage("infoerror", msg);
        }
    }

    public class Equippable : Item
    {
        public string _material;
        protected GameObject _attachment;
        protected Transform _transform;
        protected Sprite _sprite;

        public override void equip(GameObject owner)
        {
            _attachment = owner.createChild();
            _transform = _attachment.transform2();
            _sprite = _attachment.createScript<Sprite>();
            _sprite.material = Root.instance.resources.findMaterial(_material);

            _attachment.enabled = false;
        }
    }

    public class Weapon : Equippable
    {
        public override void equip(GameObject owner)
        {
            base.equip(owner);
        }
    }

    public class Sword : Equippable
    {
        public float swingSpeed = 0.25f;
        public float swingWidth = MathHelper.PiOver2;
        public float swingTimer = 0f;
        public float delayTimer = 0.01f;
        public float timer = 0f;
        public float rotation = 0f;
        public float currentRotation = 0f;
        public bool attack = false;
        public bool attacking = false;


        public override void equip(GameObject owner)
        {
            base.equip(owner);
            _sprite.origin = new Vector2(0, 1);
            _sprite.rotation = MathHelper.PiOver4;
        }

        public override void activate(GameObject owner)
        {
            attack = true;
        }

        public override void update()
        {
            if (timer > 0f)
                timer -= Root.instance.time.deltaTime;

            if (attack && timer <= 0f)
            {
                if (!attacking)
                {
                    _attachment.enabled = true;
                    var mp = Camera.mainCamera.screenToWorld(Root.instance.input.MousePosition);
                    var wp = _transform.DerivedPosition;
                    rotation = wp.GetRotation(mp);
                    currentRotation = rotation - swingWidth / 2f;
                    swingTimer = swingSpeed;
                    attacking = true;
                }
            }

            if (attacking)
            {
                if (swingTimer < 0)
                {
                    _attachment.enabled = false;
                    attacking = false;
                    timer = delayTimer;
                }
                else
                {
                    currentRotation = ((1f - (swingTimer / swingSpeed)) * swingWidth) + (rotation - (swingWidth / 2f));
                    _transform.Orientation = currentRotation;
                    swingTimer -= Root.instance.time.deltaTime;
                    //test for hits
                }
            }

            attack = false;
        }
    }

    public class Player : Script
    {
        private Item _leftItem;
        private Item _rightItem;

        private void init()
        {
            this.gameObject.createScript<Movement>();
            var material = Root.instance.resources.createMaterialFromTexture("textures/sword.png");
            //material.textureName = ;
            material.SetBlendState(BlendState.NonPremultiplied);

            var sword = new Sword();
            sword._material = material.name;
            
            _leftItem = sword;
            _leftItem.equip(this.gameObject);
        }

        private void update()
        {
            if (Root.instance.input.IsKeyDown(Keyboard.Space))
                this.gameObject.sendMessage("jump");
            if (Root.instance.input.IsKeyDown(Keyboard.A))
                this.gameObject.sendMessage("left");
            if (Root.instance.input.IsKeyDown(Keyboard.D))
                this.gameObject.sendMessage("right");
            if (Root.instance.input.IsLeftMouseDown)
                this.gameObject.sendMessage("uselefthand");
            if (Root.instance.input.IsRightMouseDown)
                this.gameObject.sendMessage("userighthand");

            if (_leftItem != null)
                _leftItem.update();
            
            if (_rightItem != null)
                _rightItem.update();
        }


        private void uselefthand()
        {
            if (_leftItem != null)
                _leftItem.activate(this.gameObject);
        }

        private void userighthand()
        {
            if (_rightItem != null)
                _rightItem.activate(this.gameObject);
        }



        private void fixedupdate()
        {

        }
    }
}
