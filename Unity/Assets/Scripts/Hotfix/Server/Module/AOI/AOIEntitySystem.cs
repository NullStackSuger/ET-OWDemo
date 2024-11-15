using System.Collections.Generic;
using System.Linq;

namespace ET.Server
{
    [EntitySystemOf(typeof(AOIEntity))]
    [FriendOf(typeof(Cell))]
    public static partial class AOIEntitySystem
    {
        [EntitySystem]
        private static void Awake(this AOIEntity self, float x, float y, int r)
        {
            AOIManagerComponent aoiManagerComponent = (self.IScene as LSWorld).GetParent<Room>().GetComponent<AOIManagerComponent>();
            
            self.Radius = r;
            
            Cell cell = aoiManagerComponent.GetCell(x, y);
            
            // 让自己关注周围的的Entity
            List<Cell> cells = aoiManagerComponent.GetCells(cell, r);
            foreach (Cell cell1 in cells)
            {
                self.AddCell(cell1);
            }
            
            cell.AddEntity(self);
        }
        [EntitySystem]
        private static void Destroy(this AOIEntity self)
        {
            Cell cell = self.Cell;
            
            cell.RemoveEntity(self);
            
            // 让自己取消关注周围的的Entity
            for (int i = self.AOICells.Count - 1; i >= 0; --i)
            {
                self.RemoveCell(self.AOICells[i]);
            }
            
            self.Radius = 0;
        }

        /// <summary>
        /// 关注一个Cell
        /// </summary>
        private static void AddCell(this AOIEntity self, Cell cell)
        {
            if (self.AOICells.Contains(cell)) return;
            
            self.AOICells.Add(cell);
            cell.OnEnter.Add(self.Id, self);
            cell.OnExit.Add(self.Id, self);

            foreach (var kv in cell.Entities)
            {
                AOIHelper.OnEnter(self, kv.Value);
            }
        }

        /// <summary>
        /// 取关一个Cell
        /// </summary>
        private static void RemoveCell(this AOIEntity self, Cell cell)
        {
            if (!self.AOICells.Contains(cell)) return;
            
            foreach (var kv in cell.Entities)
            {
                AOIHelper.OnExit(self, kv.Value);
            }
            
            cell.OnExit.Remove(self.Id);
            cell.OnEnter.Remove(self.Id);
            self.AOICells.Remove(cell);
        }

        public static void Move(this AOIEntity self, float x, float y)
        {
            AOIManagerComponent aoiManagerComponent = (self.IScene as LSWorld).GetParent<Room>().GetComponent<AOIManagerComponent>();
            
            Cell newCell = aoiManagerComponent.GetCell(x, y);
            Cell oldCell = self.Cell;
            if (newCell == oldCell) return;
            
            List<Cell> newCells = aoiManagerComponent.GetCells(newCell, self.Radius);
            List<Cell> oldCells = aoiManagerComponent.GetCells(oldCell, self.Radius);
            
            var enters = newCells.Except(oldCells);
            var exits = oldCells.Except(newCells);
            var moves = oldCells.Intersect(newCells);
            
            // 取关exits
            foreach (Cell exitCell in exits)
            {
                self.RemoveCell(exitCell);
            }
            
            // 关注enters
            foreach (Cell enterCell in enters)
            {
                self.AddCell(enterCell);
            }
            
            oldCell.RemoveEntityWithDisHandler(self, moves);
            newCell.AddEntityWithDisHandler(self, moves);
        }
    }
}