using System.Collections.Generic;
using TrueSync;

namespace ET.Server
{
    [EntitySystemOf(typeof(AOIManagerTestComponent))]
    [FriendOf(typeof(CellTest))]
    public static partial class AOIManagerTestComponentSystem
    {
        [EntitySystem]
        private static void Awake(this AOIManagerTestComponent self)
        {

        }

        public static void Add(this AOIManagerTestComponent self, LSUnit unit)
        {

        }

        public static void Remove(this AOIManagerTestComponent self, LSUnit unit)
        {

        }

        public static void Move(this AOIManagerTestComponent self, AOIEntityTest entity)
        {
            TSVector pos = self.GetParent<LSUnit>().Position;
            long newCellId = AOITestHelper.GetCellId((int)pos.x, (int)pos.z);
            if (entity.Cell.Id == newCellId) return;

            CellTest newCell = self.GetChild<CellTest>(newCellId);
            entity.Cell = newCell;
            
            AOITestHelper.CalcEnterAndLeaveCell(entity, pos, out var enters, out var leaves);

            foreach (CellTest leaveCell in leaves)
            {
                foreach (AOIEntityTest other in leaveCell.Units.Values)
                {
                    // 这里假设entity和other的视野相同, 当entity移除other时, other也要移除entity
                    entity.Remove(other);
                    other.Remove(entity);
                }
            }

            foreach (CellTest enterCell in enters)
            {
                foreach (AOIEntityTest other in enterCell.Units.Values)
                {
                    entity.Add(other);
                    other.Add(entity);
                }
            }
        }
    }
}