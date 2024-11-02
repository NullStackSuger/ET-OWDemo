using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(LSEntity))]
    [FriendOf(typeof(CellTest))]
    public class AOIEntityTest : Entity, IAwake, IDestroy
    {
        private EntityRef<CellTest> cell;
        public CellTest Cell
        {
            get
            {
                return this.cell;
            }
            set
            {
                if (value == null)
                {
                    this.cell = null;
                    return;
                }
                
                CellTest oldCell = this.cell;
                CellTest newCell = value;
                if (oldCell == newCell) return;
                oldCell.Units.Remove(this.Id);
                newCell.Units.Add(this.Id, this);
                this.cell = value;
            }
        }
    }
}