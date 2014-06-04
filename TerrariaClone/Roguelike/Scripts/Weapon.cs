using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Scripts
{
    //public class Weapon : Script
    //{

    //}

    //public class MeleeWeapon : Weapon
    //{
    //    private bool isAttacking = false;
    //    private float arcStart = 0f;
    //    private float arcEnd = 0f;
    //    private float swingTimer = 0f;
    //    private float attackDelayTimer = 0f;

    //    public float arcRange = (float)Math.PI / 2f;
    //    public float swingSpeed = 0.1f;
    //    public float attackDelay = 0.25f;
    //    public float weaponDamage = 10f;

    //    private void fixedupdate()
    //    {
    //        if (attackDelayTimer > 0f)
    //            attackDelayTimer -= Root.instance.time.deltaTime;

    //        if (Root.instance.input.IsLeftMouseDown)
    //            doattack();

    //        if (isAttacking)
    //        {
    //            swingTimer -= Root.instance.time.deltaTime;
    //            if (swingTimer <= 0f)
    //            {
    //                isAttacking = false;
    //                pivotPoint.enabled = false;
    //                attackDelayTimer = attackDelay;
    //                return;
    //            }

    //            var lerp = swingTimer / swingSpeed;
    //            var rot = arcStart.Lerp(arcEnd, lerp);

    //            pivotPoint.transform.Orientation = rot;
    //        }
    //    }

    //    private void doattack()
    //    {
    //        if (!isAttacking && attackDelayTimer <= 0f)
    //        {
    //            var wp = gameObject.transform.DerivedPosition;
    //            var mp = Camera.mainCamera.screenToWorld(Root.instance.input.MousePosition);

    //            arcStart = wp.GetRotation(mp) + arcRange * 0.5f; ;
    //            arcEnd = arcStart - arcRange;
    //            swingTimer = swingSpeed;

    //            if (mp.X < wp.X)
    //            {
    //                var temp = arcStart;
    //                arcStart = arcEnd;
    //                arcEnd = temp;
    //            }

    //            pivotPoint.transform.Orientation = arcStart;
    //            pivotPoint.enabled = true;
    //            isAttacking = true;
    //        }
    //    }
    //}
}
