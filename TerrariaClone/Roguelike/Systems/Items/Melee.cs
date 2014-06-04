using GameName1.Roguelike.Scripts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Systems.Items
{
    public class Melee : Weapon
    {
        public float arcRange = (float)Math.PI / 2f;

        protected override void doattack(GameObject parent, Vector2 target)
        {
            var pivotPoint = parent.createChild(string.Format("{0} - {1}", parent.name, "attack"));
            var meleeSwing = pivotPoint.createScript<MeleeSwing>();
            var wp = parent.transform.DerivedPosition;
            var mp = target;

            var arcStart = wp.GetRotation(mp) + arcRange * 0.5f; ;
            var arcEnd = arcStart - arcRange;

            if (mp.X < wp.X)
            {
                var temp = arcStart;
                arcStart = arcEnd;
                arcEnd = temp;
            }
            meleeSwing.swingSpeed = duration;
            meleeSwing.arcStart = arcStart;
            meleeSwing.arcEnd = arcEnd;
            pivotPoint.transform.Orientation = arcStart;

            var autoDestroy = pivotPoint.createScript<DestroyTimer>();
            autoDestroy.duration = duration;

            var weaponPoint = pivotPoint.createChild("weaponpoint");
            weaponPoint.transform.Position = new Vector2(5, 0);

            var weaponSprite = weaponPoint.createScript<Sprite>();
            weaponSprite.material = Root.instance.resources.createMaterialFromTexture(texture);
            //weaponSprite.material = Root.instance.resources.defaultMaterial;
            //weaponSprite.textureName = texture;//"content/textures/sword2.png";
            weaponSprite.renderQueue = 2;
            weaponSprite.size = new Vector2(50, 30);
            weaponSprite.origin = new Vector2(0, 0.5f);

            var collider = weaponPoint.createScript<Collider>();
            collider.flags = Physics.QUERY_PLAYER;
            collider.collidesWith = this.flags;
            //collider.notifyObject = parent;
            collider.size = weaponSprite.size;
            collider.origin = new Vector2(0, 0.5f);

            var applyDamage = weaponPoint.createScript<ApplyDamage>();
            applyDamage.flags = this.flags;
            applyDamage.amount = weaponDamage;
        }
    }
}
