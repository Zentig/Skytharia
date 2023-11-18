using UnityEngine;

namespace Skytharia.Character.Facing
{
    /**
     * <summary>Primary Facing controller. Will cause a property change in an animator.
     * Corresponding directions for animation proerty are as follows:
     * <code>0 - West
     * 1 - North West
     * 2 - North
     * 3 - North East
     * 4 - East
     * 5 - South East
     * 6 - South
     * 7 - South West</code>
     * 4 Direction:
     * <code>0 - West
     * 1 - North
     * 2 - East
     * 3 - South</code>
     * 2 Direction:
     * <code>0 - West
     * 1 - East</code></summary>
     */
    public class FacingAnimationProperty: MonoBehaviour, IFacingBehavior
    {
        private enum DirectionCount {_1 = 8, _2 = 4, _4 = 2, _8 = 1 }
        
        [SerializeField] [Tooltip("Animator to adjust. Will usually be the same as attached to Character Behavior")]
        private Animator animator;
        [SerializeField] [Tooltip("Name of Animation Property to set")] 
        private string animationProperty;
        [SerializeField] [Tooltip("Number of directions allowed for this animation")] 
        private DirectionCount directionCount;

        public void SetFacing(float degrees)
        {
            if (directionCount == DirectionCount._1) return;
            
            float angle = Mathf.DeltaAngle(0, degrees - 45 * (int)directionCount / 2.0f);
            if (angle < 0) angle += 360;
            int direction = (int)(angle / 8) / (int)directionCount;
            animator.SetInteger(animationProperty, direction);
        }
    }
}