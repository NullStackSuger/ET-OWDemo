using System.Collections.Generic;
using MemoryPack;

namespace ET
{
	[ComponentOf(typeof(LSWorld))]
	[MemoryPackable]
	public partial class LSUnitComponent: LSEntity, IAwake, ILSUpdate, ISerializeToEntity
	{
		public Queue<long> WaitToRemove = new();
	}
}