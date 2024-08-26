using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    // 类似NumericComponent 但是对于50+护甲*0.3这种会进行处理, 包括随目标人数距离衰减这种
    // 因为要挂在LSUnit下, 必须是LSEntity, 所以不能复用NumericComponent了
    
    // 一般情况下, DataModifierComponent初始状态只会设置DataModifierComponent中可以获得的数值
    // 而和自身无关的修饰器(打目标生命百分比)需要在Value的get里面去执行对应操作(范围检测敌人, 获取敌人数值组件, 获取生命)
    // 一些例外情况: 比如移速和周围敌人数目有关, 那需要通过DataModifierComponent获取B3CollisionComponent, 去范围检测
    
    // 伤害公式等(如仓鼠球护盾技能)可以在对应的ActionHandler里手写公式
    
    [ComponentOf(typeof(LSUnit))]
    public class DataModifierComponent : LSEntity, IAwake
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, LinkedList<ADataModifier>> NumericDic = new();

        public long this[int dataModifierType]
        {
            get
            {
                return this.Get(dataModifierType);
            }
        }
    }

    [FriendOf(typeof(DataModifierComponent))]
    public static class DataModifierComponentSystem
    {
        public static long Get(this DataModifierComponent self, int dataModifierType)
        {
            long result = 0;
            self.Get(dataModifierType, ref result);
            return result;
        }
        public static void Get(this DataModifierComponent self, int dataModifierType, ref long result)
        {
            self.NumericDic.TryGetValue(dataModifierType, out var modifiers);

            long add = 0;
            long pct = 0;
            long finalAdd = 0;
            long finalPct = 0;

            foreach (var modifier in modifiers)
            {
                switch (modifier.ModifierType)
                {
                    case ModifierType.Constant:
                        add += modifier.Get();
                        break;
                    case ModifierType.Percentage:
                        pct += modifier.Get();
                        break;
                    case ModifierType.FinalConstant:
                        finalAdd += modifier.Get();
                        break;
                    case ModifierType.FinalPercentage:
                        finalPct += modifier.Get();
                        break;
                }
            }
            
            result = ((result + add) * (1 + pct) + finalAdd) * (1 + finalPct);
        }

        public static void Add(this DataModifierComponent self, int dataModifierType, ADataModifier modifier)
        {
            if (!self.NumericDic.TryGetValue(dataModifierType, out var modifiers))
            {
                modifiers = new();
                self.NumericDic.Add(dataModifierType, modifiers);
            }

            modifiers.AddLast(modifier);
        }

        public static bool Remove(this DataModifierComponent self, int dataModifierType, ADataModifier modifier)
        {
            if (!self.NumericDic.TryGetValue(dataModifierType, out var modifiers))
            {
                modifiers = new();
                self.NumericDic.Add(dataModifierType, modifiers);
            }

            bool res = modifiers.Remove(modifier);

            return res;
        }
    }
}