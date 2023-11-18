using UnityEngine;

namespace Skytharia.Character.Facing
{
    /**
     * <summary>Add this component as an empty Facing component. Consider whether you can use
     * this component, or build the character without using a facing system.</summary>
     */
    public class FacingStatic : MonoBehaviour, IFacingBehavior
    {
        public void SetFacing(float degrees) { }
    }
}