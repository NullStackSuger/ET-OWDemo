using System.Collections.Generic;
using System.Linq;

namespace ET.Server
{
    [EntitySystemOf(typeof(Cell))]
    [FriendOf(typeof(Cell))]
    public static partial class CellSystem
    {
        [EntitySystem]
        private static void Awake(this Cell self)
        {

        }
        [EntitySystem]
        private static void Destroy(this Cell self)
        {
            if (self.Entities.Count > 0)
                Log.Warning($"{self.Id} Entities Error");
            
            if (self.OnEnter.Count > 0)
                Log.Warning($"{self.Id} OnEnter Error");
            
            if (self.OnExit.Count > 0)
                Log.Warning($"{self.Id} OnExit Error");
        }

        /// <summary>
        /// 使其他Entity关注一个Entity
        /// </summary>
        public static void AddEntity(this Cell self, AOIEntity entity)
        {
            entity.Cell = self;
            self.Entities.Add(entity.Id, entity);
            
            foreach (var kv in entity.Cell.OnEnter)
            {
                AOIEntity a = kv.Value;
                AOIHelper.OnEnter(a, entity);
            }
        }

        /// <summary>
        /// 使其他Entity取关一个Entity
        /// </summary>
        public static void RemoveEntity(this Cell self, AOIEntity entity)
        {
            foreach (var kv in entity.Cell.OnExit)
            {
                AOIEntity a = kv.Value;
                AOIHelper.OnExit(a, entity);
            }
            
            self.Entities.Remove(entity.Id);
            entity.Cell = null;
        }

        /// <summary>
        /// 使其他Entity关注一个Entity
        /// </summary>
        /// <param name="disHandler">哪些Cell下的Entity不需要执行OnEnter</param>
        public static void AddEntityWithDisHandler(this Cell self, AOIEntity entity, IEnumerable<Cell> disHandler)
        {
            entity.Cell = self;
            self.Entities.Add(entity.Id, entity);
            
            foreach (var kv in entity.Cell.OnEnter)
            {
                AOIEntity a = kv.Value;
                if (disHandler.Contains(a.Cell)) continue;
                AOIHelper.OnEnter(a, entity);
            }
        }

        /// <summary>
        /// 使其他Entity取关一个Entity
        /// </summary>
        /// <param name="disHandler">哪些Cell下的Entity不需要执行OnEnter</param>
        public static void RemoveEntityWithDisHandler(this Cell self, AOIEntity entity, IEnumerable<Cell> disHandler)
        {
            foreach (var kv in entity.Cell.OnExit)
            {
                AOIEntity a = kv.Value;
                if (disHandler.Contains(a.Cell)) continue;
                AOIHelper.OnExit(a, entity);
            }
            
            self.Entities.Remove(entity.Id);
            entity.Cell = null;
        }
    }
}