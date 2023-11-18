using UnityEngine;

namespace Skytharia.Enemy
{
    /**
 * <summary>Intermediate Enemy class to control ground walking enemies. Terrain and path
 * information will eventually go here. Enemy moves in a straight line at this point.</summary>
 */
    public abstract class WalkingEnemy : FacingEnemy
    {
        protected abstract class WalkingEnemyContext : FacingEnemyContext
        {
            /** <summary>Speed to move at on the next frame</summary> */
            public float WalkSpeed;

            /** <summary>Target position.</summary> */
            public Vector2 TargetPosition;

            /** <summary>Turn automatic walking on and off</summary> */
            public bool IsWalking;

            /** <summary>Check to see if enemy is already at target position (no motion)</summary> */
            public bool IsAtTarget => (Body.position - TargetPosition).magnitude < 0.00001;
        }

        /**
     * <summary>Base walking state class. Can be used or ignored by end enemies, but should eventually
     * be part of terrain/pathfinding system.</summary>
     */
        protected class WalkingState : FacingState
        {
            protected WalkingEnemyContext WalkingContext => Context as WalkingEnemyContext;

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                if (WalkingContext.IsWalking && !WalkingContext.IsAtTarget)
                {
                    Vector2 diff = WalkingContext.TargetPosition - Context.Body.position;
                    Vector2 direction = diff.normalized;
                    FacingContext.FacingAngleDegrees = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    var speed = Mathf.Min(WalkingContext.WalkSpeed, diff.magnitude / Time.fixedDeltaTime);
                    Context.Body.velocity = direction * speed;
                }
                else Context.Body.velocity = Vector2.zero;
            }

            public override void EndState()
            {
                base.EndState();
                Context.Body.velocity = Vector2.zero;
            }
        }
    }
}