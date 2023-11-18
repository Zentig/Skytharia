using Unity.VisualScripting;
using UnityEngine;

namespace Skytharia.Enemy
{
    /**
 * <summary>Base for an enemy that points in different directions. Only left/right currently implemented
 * (sprite flip).</summary>
 */
    public abstract class FacingEnemy : EnemyBase
    {
        [Header("Facing Settings")]
        [SerializeField] [Tooltip("Facing mode to use for this enemy.")]
        private FacingEnemyContext.FacingMode mode;
        [SerializeField] [Tooltip("Root object to flip for left/right Direction control")]
        private GameObject leftRightDirectionControl;

        protected override void SetupContext(EnemyContext ctx)
        {
            base.SetupContext(ctx);
            if (ctx is FacingEnemyContext context)
            {
                context.Mode = mode;
                context.FacingObject = leftRightDirectionControl;
            }
        }

        protected abstract class FacingEnemyContext : EnemyContext
        {
            /** <summary>Facing angle to convert to a sprite direction</summary> */
            public float FacingAngleDegrees;

            /** <summary>Mode this sprite is running in</summary> */
            public FacingMode Mode;

            /** <summary>Object to flip in left/right mode</summary> */
            public GameObject FacingObject;

            /** <summary>List of facing modes</summary> */
            public enum FacingMode
            {
                /** <summary>Single facing, no directional changes</summary> */
                Single,

                /** <summary>Left/Right scale flip only</summary> */
                LeftRight,

                /** <summary>Left/Right sprite set swap</summary> */
                Direction2,

                /** <summary>Left/Right/Up/Down sprite set swap</summary> */
                Direction4,

                /** <summary>8 Direction sprite set swap</summary> */
                Direction8
            }
        }

        protected class FacingState : EnemyState
        {
            protected FacingEnemyContext FacingContext => Context as FacingEnemyContext;
            
            public override void Update()
            {
                base.Update();
                switch (FacingContext.Mode)
                {
                    case FacingEnemyContext.FacingMode.Single:
                        // No change to sprite.
                        break;
                    case FacingEnemyContext.FacingMode.LeftRight:
                        var angle = Mathf.DeltaAngle(0, FacingContext.FacingAngleDegrees);
                        FacingContext.FacingObject.transform.localScale = Mathf.Abs(angle) <= 90 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
                        break;
                }
            }
        }
    }
}