using System;
using UnityEngine;

namespace Skytharia.Enemy
{
    /**
     * <summary>Stub class to attach to Animator components to ensure that triggers get
     * passed along to the base object. SetEnemyBase must be called with a valid enemy
     * to pass along calls.</summary>
     */
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimationMarker: MonoBehaviour
    {
        private Animator _ani;
        private EnemyBase _enemyRoot;

        private void Awake()
        {
            _ani = GetComponent<Animator>();
        }

        /**
         * <summary>Used to attach the base Enemy the animator appears in.</summary>
         */
        public void SetEnemyBase(EnemyBase enemy)
        {
            _enemyRoot = enemy;
        }

        /**
         * <summary>Entry point for single play animations that end. Used to adjust to a new
         * Enemy State from the EnemyBase object.</summary>
         */
        public void OnAnimationDone()
        {
            _enemyRoot.FinishAnimation();
        }

        /**
         * Reference to the animator.
         */
        public Animator Animator => _ani;
    }
}