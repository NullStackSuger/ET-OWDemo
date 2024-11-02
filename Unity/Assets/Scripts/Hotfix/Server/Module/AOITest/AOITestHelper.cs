using System.Collections.Generic;
using TrueSync;

namespace ET.Server
{
    [FriendOf(typeof(CellTest))]
    public static class AOITestHelper
    {
        public static long GetCellId(int x, int y)
        {
            return (long)((ulong)x << 32) | (uint)y;
        }

        /// <summary>
        /// 获取Unit所在Cell以及附近的Cell(9宫格)
        /// </summary>
        public static List<CellTest> GetCells(this AOIEntityTest self)
        {
            return null;
        }

        /// <summary>
        /// 获取附近的AOIEntity(包括自己)
        /// </summary>
        public static List<AOIEntityTest> GetAOIEntitys(this AOIEntityTest self)
        {
            List<AOIEntityTest> entitys = new();
            foreach (CellTest cell in self.GetCells())
            {
                foreach (AOIEntityTest entity in cell.Units.Values)
                {
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        /// <summary>
        /// 计算Cell的变化
        /// </summary>
        public static void CalcEnterAndLeaveCell(AOIEntityTest entity, TSVector position, out HashSet<CellTest> enterCell, out HashSet<CellTest> leaveCell)
        {
            enterCell = new();
            leaveCell = new();

            // 找到OldCell和NewCell的9宫格
            // OldCells - NewCells = leave
            // NewCells - OldCells = enter
            // OldCells * NewCells = move
        }
    }
}