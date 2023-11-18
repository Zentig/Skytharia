using UnityEngine;

namespace Skytharia.Character.Facing
{
    /**
     * <summary>Facing component that causes a scale X flip. This can have unintended results with colliders,
     * and should be only used on very basic characters.</summary>
     */
    public class FacingLRFlip: MonoBehaviour, IFacingBehavior
    {
        [SerializeField] [Tooltip("The object to flip. This will change Scale X from 1 to -1. Do not change local scale of this object.")]
        private GameObject flipObject;

        public void SetFacing(float degrees)
        {
            flipObject.transform.localScale = Mathf.Abs(Mathf.DeltaAngle(0, degrees)) <= 90
                            ? new Vector3(1, 1, 1)
                            : new Vector3(-1, 1, 1);
        }
    }
}