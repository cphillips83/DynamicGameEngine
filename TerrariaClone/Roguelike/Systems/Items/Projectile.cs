using GameName1.Roguelike.Scripts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Systems.Items
{
    public class Projectile : Weapon
    {
        public float speed = 4f;
        
        protected override void doattack(GameObject parent, Vector2 target)
        {
            var projectileObject = Root.instance.RootObject.createChild(string.Format("{0} - {1}", parent.name, "attack"));
            projectileObject.transform.Position = parent.transform.DerivedPosition;

            var velocity = projectileObject.createScript<ConstantVelocity>();

            var wp = parent.transform.DerivedPosition;
            var mp = target; //Camera.mainCamera.screenToWorld(Root.instance.input.MousePosition);

            var dir = mp - wp;
            dir.Normalize();
            velocity.speed = dir * speed;
            projectileObject.transform.Forward = dir;

            var autoDestroy = projectileObject.createScript<DestroyTimer>();
            autoDestroy.duration = duration;

            var weaponSprite = projectileObject.createScript<Sprite>();
            weaponSprite.material = Root.instance.resources.defaultMaterial;

            //weaponSprite.material = Root.instance.resources.defaultMaterial;
            weaponSprite.material = Root.instance.resources.createMaterialFromTexture(texture);
            //weaponSprite.textureName = texture;//"content/textures/sword2.png";
            weaponSprite.renderQueue = 2;
            weaponSprite.size = new Vector2(10, 10);
            weaponSprite.origin = new Vector2(0.5f, 0.5f);

            var collider = projectileObject.createScript<Collider>();
            collider.flags = Physics.QUERY_ENEMIES;
            collider.collidesWith = this.flags;
            //collider.notifyObject = parent;
            collider.size = weaponSprite.size;
            collider.origin = new Vector2(0.5f, 0.5f);

            var applyDamage = projectileObject.createScript<ApplyDamage>();
            applyDamage.flags = this.flags;
            applyDamage.amount = weaponDamage;

            var ricochet = projectileObject.createScript<Ricochet>();
            ricochet.flags = flags;
        }
    }
}
