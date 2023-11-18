namespace Skytharia.Character
{
    public interface IFacingBehavior
    {
        public void SetFacing(float degrees);
    }
    
    public interface IFacingContext
    {
        public float Facing { get; set; }
        public IFacingBehavior FacingBehavior { get; set; }
    }
}