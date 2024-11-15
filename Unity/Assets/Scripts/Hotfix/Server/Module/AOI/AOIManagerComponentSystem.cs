using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace ET.Server
{
    [EntitySystemOf(typeof(AOIManagerComponent))]
    public static partial class AOIManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this AOIManagerComponent self)
        {
            string path = $"D:\\AOI{self.GetParent<Room>().Name}.bytes";
            if (!File.Exists(path)) return;
            byte[] bytes = File.ReadAllBytes(path);
            List<Vector2> points = MemoryPackHelper.Deserialize<List<Vector2>>(bytes);

            foreach (Vector2 point in points)
            {
                self.CreatCell((int)point.X, (int)point.Y);
            }
        }
        [EntitySystem]
        private static void Destroy(this AOIManagerComponent self)
        {
            
        }

        public static Cell CreatCell(this AOIManagerComponent self, int x, int y)
        {
            long id = (long) ((ulong) x << 32) | (uint) y;
            Cell cell = self.AddChildWithId<Cell>(id);
            return cell;
        }
        
        public static Cell GetCell(this AOIManagerComponent self, float x, float y)
        {
            long id = (long) ((ulong) x << 32) | (uint) y;
            Cell cell = self.GetChild<Cell>(id);
            return cell;
        }
        
        /// <summary>
        /// 计算附近的9宫格
        /// </summary>
        public static List<Cell> GetCells(this AOIManagerComponent self, Cell cell, int radius)
        {
            List<Cell> cells = new List<Cell>();
            int x = (int)(cell.Id >> 32);
            int y = (int)(cell.Id & 0xFFFF);
            for (int i = x - radius; i <= x + radius; ++i)
            {
                for (int j = y - radius; j <= y + radius; ++j)
                {
                    Cell cell1 = self.GetCell(i, j);
                    if (cell1 == null)
                    {
                        // 通常是碰到Cell的边界了, 但是Cell的区域应该比玩家的活动范围大一圈, 碰到边界是不对的
                        Log.Warning($"AOI:Cell:{i},{j}不存在");
                        continue;
                    }
                    cells.Add(self.GetCell(i, j));
                }
            }
            return cells;
        }
    }
}