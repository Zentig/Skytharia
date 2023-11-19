using UnityEngine;

namespace Skytharia.Character
{
    public enum AnimationType { Idle=0, Walk=1, Run=2, Dead=3, Attack=4 }
    
    public interface IAnimatorContext
    {
        public Animator Animator { get; set; }
        public AnimationType CurrentAnimation { get; set; }
    }
}