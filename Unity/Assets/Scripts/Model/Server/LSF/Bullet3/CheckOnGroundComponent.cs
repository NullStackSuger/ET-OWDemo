namespace ET.Server
{
    [ComponentOf(typeof(LSUnit))]
    public class CheckOnGroundComponent : LSEntity, IAwake, ILSUpdate
    {
        private bool onGround;

        public bool OnGround
        {
            get
            {
                return this.onGround;
            }
            set
            {
                if (this.onGround == value) return;

                this.onGround = value;
                EventSystem.Instance.Publish(this.iScene as LSWorld, new UnitOnGround() { Unit = this.GetParent<LSUnit>(), OnGround = value });
            }
        }
    }
}