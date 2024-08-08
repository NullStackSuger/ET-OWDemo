using System.Numerics;
using BulletSharp;
using BulletSharp.Math;

namespace ET
{
    [EntitySystemOf(typeof(CastComponent))]
    [FriendOf(typeof(CastComponent))]
    [FriendOfAttribute(typeof(ET.Cast))]
    [FriendOfAttribute(typeof(ET.LSUnit))]
    public static partial class CastComponentSystem
    {
        [EntitySystem]
        private static void Awake(this CastComponent self)
        {

        }

        public static Cast Creat(this CastComponent self, int configId)
        {
            Cast cast = self.AddChild<Cast, int>(configId);
            self.Casts.Add(cast);
            return cast;
        }

        public static Cast Creat(this CastComponent self, int configId, long castUnitId)
        {
            Cast cast = self.AddChild<Cast, int, long>(configId, castUnitId);
            self.Casts.Add(cast);
            return cast;
        }

        public static void Remove(this CastComponent self, Cast cast)
        {
            self.Casts.Remove(cast);
            self.RemoveChild(cast.Id);
        }
    }
}