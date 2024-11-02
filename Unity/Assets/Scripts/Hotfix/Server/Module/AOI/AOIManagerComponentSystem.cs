using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(AOIManagerComponent))]
    [FriendOf(typeof(AOIEntity))]
    [FriendOf(typeof(Cell))]
    public static partial class AOIManagerComponentSystem
    {
        public static void Add(this AOIManagerComponent self, AOIEntity aoiEntity, float x, float y)
        {
            int cellX = (int)(x * 1000) / AOIManagerComponent.CellSize;
            int cellY = (int)(y * 1000) / AOIManagerComponent.CellSize;

            if (aoiEntity.ViewDistance == 0)
            {
                aoiEntity.ViewDistance = 1;
            }
            
            AOIHelper.CalcEnterAndLeaveCell(aoiEntity, cellX, cellY, aoiEntity.SubEnterCells, aoiEntity.SubLeaveCells);

            // 新加入的9宫格
            // 遍历EnterCell
            foreach (long cellId in aoiEntity.SubEnterCells)
            {
                Cell cell = self.GetCell(cellId);
                aoiEntity.SubEnter(cell);
            }

            // 离开的之前的9宫格
            // 遍历LeaveCell
            foreach (long cellId in aoiEntity.SubLeaveCells)
            {
                Cell cell = self.GetCell(cellId);
                aoiEntity.SubLeave(cell);
            }

            // 处于哪个新的Cell
            // 自己加入的Cell
            Cell selfCell = self.GetCell(AOIHelper.CreateCellId(cellX, cellY));
            aoiEntity.Cell = selfCell;
            selfCell.Add(aoiEntity);
            // 通知订阅该Cell Enter的Unit
            foreach (KeyValuePair<long, EntityRef<AOIEntity>> kv in selfCell.SubsEnterEntities)
            {
                AOIEntity e = kv.Value;
                e.EnterSight(aoiEntity);
            }
        }

        public static void Remove(this AOIManagerComponent self, AOIEntity aoiEntity)
        {
            if (aoiEntity.Cell == null)
            {
                return;
            }

            // 先从之前的Cell移除, 为什么不像Add一样最后移除(添加), 因为Remove中最后移除会导致退出事件无法正常工作
            // 通知订阅该Cell Leave的Unit
            aoiEntity.Cell.Remove(aoiEntity);
            foreach (KeyValuePair<long, EntityRef<AOIEntity>> kv in aoiEntity.Cell.SubsLeaveEntities)
            {
                AOIEntity e = kv.Value;
                e?.LeaveSight(aoiEntity);
            }

            // 取消订阅之前Cell的进入事件, eg.有新玩家进入这个Cell, 由于取消订阅了, 就不会再创建GameObject了
            // 通知自己订阅的Enter Cell，清理自己
            foreach (long cellId in aoiEntity.SubEnterCells)
            {
                Cell cell = self.GetCell(cellId);
                aoiEntity.UnSubEnter(cell);
            }

            // 取消订阅之前Cell的退出事件
            foreach (long cellId in aoiEntity.SubLeaveCells)
            {
                Cell cell = self.GetCell(cellId);
                aoiEntity.UnSubLeave(cell);
            }
    
            // 移除之后这2个还有元素的话可能有泄露的地方, 应该再调用Add之后才有元素
            // 检查
            if (aoiEntity.SeeUnits.Count > 1)
            {
                Log.Error($"aoiEntity has see units: {aoiEntity.SeeUnits.Count}");
            }

            if (aoiEntity.BeSeeUnits.Count > 1)
            {
                Log.Error($"aoiEntity has beSee units: {aoiEntity.BeSeeUnits.Count}");
            }
        }

        private static Cell GetCell(this AOIManagerComponent self, long cellId)
        {
            Cell cell = self.GetChild<Cell>(cellId);
            if (cell == null)
            {
                cell = self.AddChildWithId<Cell>(cellId);
            }

            return cell;
        }

        public static void Move(AOIEntity aoiEntity, Cell newCell, Cell preCell)
        {
            aoiEntity.Cell = newCell;
            preCell.Remove(aoiEntity);
            newCell.Add(aoiEntity);
            
            // TODO　为什么又遍历一次？不直接调用Add?
            // 通知订阅该newCell Enter的Unit
            foreach (KeyValuePair<long, EntityRef<AOIEntity>> kv in newCell.SubsEnterEntities)
            {
                AOIEntity e = kv.Value;
                if (e.SubEnterCells.Contains(preCell.Id))
                {
                    continue;
                }
                e.EnterSight(aoiEntity);
            }

            // 通知订阅preCell leave的Unit
            foreach (KeyValuePair<long, EntityRef<AOIEntity>> kv in preCell.SubsLeaveEntities)
            {
                // 如果新的cell仍然在对方订阅的subleave中
                AOIEntity e = kv.Value;
                if (e.SubLeaveCells.Contains(newCell.Id))
                {
                    continue;
                }

                e.LeaveSight(aoiEntity);
            }
        }

        public static void Move(this AOIManagerComponent self, AOIEntity aoiEntity, int cellX, int cellY)
        {
            long newCellId = AOIHelper.CreateCellId(cellX, cellY);
            if (aoiEntity.Cell.Id == newCellId) // cell没有变化
            {
                return;
            }

            // 自己加入新的Cell
            Cell newCell = self.GetCell(newCellId);
            Move(aoiEntity, newCell, aoiEntity.Cell);

            // TODO ?
            AOIHelper.CalcEnterAndLeaveCell(aoiEntity, cellX, cellY, aoiEntity.enterHashSet, aoiEntity.leaveHashSet);
            
            // 算出自己leave新Cell
            foreach (long cellId in aoiEntity.leaveHashSet)
            {
                if (aoiEntity.SubLeaveCells.Contains(cellId))
                {
                    continue;
                }

                Cell cell = self.GetCell(cellId);
                aoiEntity.SubLeave(cell);
            }

            // 算出需要通知离开的Cell
            aoiEntity.SubLeaveCells.ExceptWith(aoiEntity.leaveHashSet);
            foreach (long cellId in aoiEntity.SubLeaveCells)
            {
                Cell cell = self.GetCell(cellId);
                aoiEntity.UnSubLeave(cell);
            }

            // 这里交换两个HashSet,提高性能
            ObjectHelper.Swap(ref aoiEntity.SubLeaveCells, ref aoiEntity.leaveHashSet);

            // 算出自己看到的新Cell
            foreach (long cellId in aoiEntity.enterHashSet)
            {
                if (aoiEntity.SubEnterCells.Contains(cellId))
                {
                    continue;
                }

                Cell cell = self.GetCell(cellId);
                aoiEntity.SubEnter(cell);
            }

            // 离开的Enter
            aoiEntity.SubEnterCells.ExceptWith(aoiEntity.enterHashSet);
            foreach (long cellId in aoiEntity.SubEnterCells)
            {
                Cell cell = self.GetCell(cellId);
                aoiEntity.UnSubEnter(cell);
            }

            // 这里交换两个HashSet,提高性能
            ObjectHelper.Swap(ref aoiEntity.SubEnterCells, ref aoiEntity.enterHashSet);
        }
    }
}