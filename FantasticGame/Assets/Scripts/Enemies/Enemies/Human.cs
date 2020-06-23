using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class Human : EnemyBaseRanged
{
    [SerializeField] private Transform magicCrouchPosition;

    protected override void AimCheck()
    {
        if (p1 != null)
        {
            RaycastHit2D aimTop = Physics2D.Raycast(attackPosition.position, attackPosition.right, maxAimRange);
            RaycastHit2D aimJump = Physics2D.Raycast(magicJumpPosition.position, attackPosition.right, maxAimRange);
            RaycastHit2D aimCrouch = Physics2D.Raycast(magicCrouchPosition.position, attackPosition.right, maxAimRange);
            if (aimTop.rigidbody == p1.Rb || aimJump.rigidbody == p1.Rb || aimCrouch.rigidbody == p1.Rb)
            {
                shootAnimation = false; // FOR ANIMATOR
                attacking = true; // FOR ANIMATOR

                // Sets a timer to move, sets a timer to attack
                canMoveTimer = attackDelay;
                Stats.RangedAttackDelay -= Time.deltaTime;

                if (Stats.RangedAttackDelay < 0)
                {
                    Shoot();
                    Stats.RangedAttackDelay = attackDelay;
                }
            }
            else
            {   // If the player leaves max range, if the timer is less than 0, the enemy moves
                canMoveTimer -= Time.deltaTime;

                if (canMoveTimer <= 0)
                {
                    shootAnimation = false; // FOR ANIMATOR
                    attacking = false; // FOR ANIMATOR
                    if (staticEnemy == false) Movement();
                }
            }
        }
    }
}
