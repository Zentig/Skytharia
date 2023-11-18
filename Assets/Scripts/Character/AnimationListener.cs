using UnityEngine;

namespace Skytharia.Character
{
    public interface IAnimationListener
    {
        public void OnAnimationFinished();
    }

    /**
     * <summary>Stub class to attach to Animator components to ensure that triggers get
     * passed along to the base object. SetEnemyBase must be called with a valid enemy
     * to pass along calls.</summary>
     */
    [RequireComponent(typeof(Animator))]
    public class AnimationListener: MonoBehaviour
    {
        private Animator _ani;
        private IAnimationListener _rootObject;

        private void Awake()
        {
            _ani = GetComponent<Animator>();
        }

        /**
         * <summary>Used to attach the base GameObject the animator appears in.</summary>
         */
        public void SetEnemyBase(IAnimationListener baseObject)
        {
            _rootObject = baseObject;
        }

        /**
         * <summary>Entry point for single play animations that end. Used to adjust to a new
         * Enemy State from the EnemyBase object.</summary>
         */
        public void OnAnimationDone()
        {
            _rootObject.OnAnimationFinished();
        }

        /** <summary>Reference to the animator.</summary> */
        public Animator Animator => _ani;
    }
}