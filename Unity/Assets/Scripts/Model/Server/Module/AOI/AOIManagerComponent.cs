namespace ET.Server
{
   [ComponentOf(typeof(Room))]
   public class AOIManagerComponent : Entity, IAwake, IDestroy
   {
      public const int CellSize = 10;
   }
}