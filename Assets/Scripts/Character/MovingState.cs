using UnityEngine;

namespace Skytharia.Character
{
    public abstract class MovingBehavior : CharacterBehavior
    {
        [Header("Motion Setup")]
        [SerializeField] [Tooltip("A link to the facing component this character uses.")] 
        private IFacingBehavior facingComponent;

        protected override void BuildContext()
        {
            var ctx = Context as IFacingContext;
            ctx!.FacingBehavior = facingComponent;
        }
    }
    
    public abstract class MovingContext : CharacterContext, IFacingContext
    {
        public float Facing { get; set; }
        public IFacingBehavior FacingBehavior { get; set; }
        public bool IsMoving;
        public Vector2 TargetPosition;
        public float Speed;

        public bool IsAtPosition => (TargetPosition - Body.position).magnitude < 0.00001;
    }
    
    public class MovingState : CharacterState
    {
        private MovingContext _context;

        public override void BeginState(CharacterContext context)
        {
            base.BeginState(context);
            _context = Context as MovingContext;
        }

        public override void Update()
        {
            base.Update();
            _context.FacingBehavior.SetFacing(_context.Facing);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (_context.IsMoving && !_context.IsAtPosition)
            {
                Vector2 diff = _context.TargetPosition - _context.Body.position;
                Vector2 direction = diff.normalized;
                _context.Facing = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                float speed = Mathf.Min(_context.Speed, diff.magnitude / Time.fixedDeltaTime);
                Context.Body.velocity = direction * speed;
            }
        }
    }
}