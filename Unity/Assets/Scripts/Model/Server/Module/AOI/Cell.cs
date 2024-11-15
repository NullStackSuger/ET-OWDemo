using System.Collections.Generic;

namespace ET.Server
{
   [ChildOf(typeof(AOIManagerComponent))]
   public class Cell : Entity, IAwake, IDestroy
   {
      public Dictionary<long, EntityRef<AOIEntity>> Entities = new Dictionary<long, EntityRef<AOIEntity>>();
      public Dictionary<long, EntityRef<AOIEntity>> OnEnter = new Dictionary<long, EntityRef<AOIEntity>>();
      public Dictionary<long, EntityRef<AOIEntity>> OnExit = new Dictionary<long, EntityRef<AOIEntity>>();
   }
}