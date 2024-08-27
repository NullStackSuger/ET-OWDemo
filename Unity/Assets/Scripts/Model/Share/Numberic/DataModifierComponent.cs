using System;
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
        public static (
                long, long, long, 
                long, long, long, 
                long, long, long, 
                long, long, long, 
                long, long) GetValues(this DataModifierComponent self, int dataModifierType)
        {
            long add = 0;
            long addMax = 0;
            long addMin = 0;
            long pct = 0;
            long pctMax = 0;
            long pctMin = 0;
            long finalAdd = 0;
            long finalAddMax = 0;
            long finalAddMin = 0;
            long finalPct = 0;
            long finalPctMax = 0;
            long finalPctMin = 0;
            long max = 0;
            long min = 0;

            foreach (var modifier in self.NumericDic[dataModifierType])
            {
                switch (modifier.ModifierType)
                {
                    case ModifierType.Constant:
                        add += modifier.Get(self);
                        break;
                    case ModifierType.ConstantMax:
                        addMax = modifier.Get(self);
                        break;
                    case ModifierType.ConstantMin:
                        addMin = modifier.Get(self);
                        break;
                    case ModifierType.Percentage:
                        pct += modifier.Get(self);
                        break;
                    case ModifierType.PercentageMax:
                        pctMax = modifier.Get(self);
                        break;
                    case ModifierType.PercentageMin:
                        pctMin = modifier.Get(self);
                        break;
                    case ModifierType.FinalConstant:
                        finalAdd += modifier.Get(self);
                        break;
                    case ModifierType.FinalConstantMax:
                        finalAddMax = modifier.Get(self);
                        break;
                    case ModifierType.FinalConstantMin:
                        finalAddMin = modifier.Get(self);
                        break;
                    case ModifierType.FinalPercentage:
                        finalPct += modifier.Get(self);
                        break;
                    case ModifierType.FinalPercentageMax:
                        finalPctMax = modifier.Get(self);
                        break;
                    case ModifierType.FinalPercentageMin:
                        finalPctMin = modifier.Get(self);
                        break;
                    case ModifierType.FinalMax:
                        max = modifier.Get(self);
                        break;
                    case ModifierType.FinalMin:
                        min = modifier.Get(self);
                        break;
                }
            }

            return (add, addMax, addMin, 
                pct, pctMax, pctMin, 
                finalAdd, finalAddMax, finalAddMin, 
                finalPct, finalPctMax, finalPctMin,
                max, min);
        }
        
        public static long Get(this DataModifierComponent self, int dataModifierType)
        {
            long result = 0;
            self.Get(dataModifierType, ref result);
            return result;
        }
        public static void Get(this DataModifierComponent self, int dataModifierType, ref long result)
        {
            if (!self.NumericDic.ContainsKey(dataModifierType))
            {
                Log.Error($"未找到{dataModifierType}类型");
                return;
            }

            var values = self.GetValues(dataModifierType);

            long add = values.Item1;
            long addMax = values.Item2;
            long addMin = values.Item3;
            long pct = values.Item4;
            long pctMax = values.Item5;
            long pctMin = values.Item6;
            long finalAdd = values.Item7;
            long finalAddMax = values.Item8;
            long finalAddMin = values.Item9;
            long finalPct = values.Item10;
            long finalPctMax = values.Item11;
            long finalPctMin = values.Item12;
            long max = values.Item13;
            long min = values.Item14;
            
            // 限制add值
            if (!(addMin == addMax && addMax == 0)) add = Math.Clamp(add, addMin, addMax);
            // 限制pct值
            if (!(pctMin == pctMax && pctMax == 0)) pct = Math.Clamp(pct, pctMin, pctMax);
            // 限制finalAdd值
            if (!(finalAddMin == finalAddMax && finalAddMax == 0)) finalAdd = Math.Clamp(finalAdd, finalAddMin, finalAddMax);
            // 限制finalPct值
            if (!(finalPctMin == finalPctMax && finalPctMax == 0)) finalPct = Math.Clamp(finalPct, finalPctMin, finalPctMax);
            
            // 限制最终结果
            long value = ((result + add) * (1 + pct) + finalAdd) * (1 + finalPct);
            result = (min == max && max == 0) ? value : Math.Clamp(value, min, max);
        }
        
        public static void Add(this DataModifierComponent self, ADataModifier modifier, bool isPublicEvent = false)
        {
            int dataModifierType = modifier.Key;
            
            if (!self.NumericDic.TryGetValue(dataModifierType, out var modifiers))
            {
                modifiers = new();
                self.NumericDic.Add(dataModifierType, modifiers);
            }

            long oldValue = self.Get(dataModifierType);
            
            modifiers.AddLast(modifier);

            long newValue = self.Get(dataModifierType);

            if (isPublicEvent)
            {
                EventSystem.Instance.Publish(self.Scene(), new DataModifierChange() 
                        { Unit =  self.GetParent<LSUnit>(), DataModifierType = dataModifierType, Old = oldValue, New = newValue });
            }
        }

        public static bool Remove(this DataModifierComponent self, ADataModifier modifier, bool isPublicEvent = false)
        {
            int dataModifierType = modifier.Key;
            
            if (!self.NumericDic.TryGetValue(dataModifierType, out var modifiers))
            {
                modifiers = new();
                self.NumericDic.Add(dataModifierType, modifiers);
            }

            long oldValue = self.Get(dataModifierType);

            bool res = modifiers.Remove(modifier);

            long newValue = self.Get(dataModifierType);

            if (isPublicEvent)
            {
                EventSystem.Instance.Publish(self.Scene(), new DataModifierChange() 
                        { Unit =  self.GetParent<LSUnit>(), DataModifierType = dataModifierType, Old = oldValue, New = newValue });
            }

            return res;
        }

        /// <summary>
        /// 清理指定类型的多余数据, 使集合长度更短, Get速度更快
        /// </summary>
        public static void Clear(this DataModifierComponent self, int dataModifierType, ref ADataModifier addModifier, ref ADataModifier finalAddModifier)
        {
            if (!self.NumericDic.TryGetValue(dataModifierType, out var modifiers))
            {
                Log.Error($"未找到{dataModifierType}类型");
                return;
            }

            Queue<ADataModifier> waitToClear = new();
            long add = 0;
            long finalAdd = 0;

            foreach (ADataModifier modifier in modifiers)
            {
                if (!modifier.NeedClear) continue;

                if (modifier.ModifierType == ModifierType.Constant) add += modifier.Get(self);
                
                if (modifier.ModifierType == ModifierType.FinalConstant) finalAdd += modifier.Get(self);
                
                waitToClear.Enqueue(modifier);
            }

            // LinkedList似乎没法根据下标获取值, 没法倒序遍历
            while (waitToClear.TryDequeue(out ADataModifier modifier))
            {
                modifiers.Remove(modifier);
            }

            if (add != 0)
            {
                addModifier.Set(add);
                addModifier.NeedClear = true;
                modifiers.AddLast(addModifier);
            }

            if (finalAdd != 0)
            {
                finalAddModifier.Set(finalAdd);
                finalAddModifier.NeedClear = true;
                modifiers.AddLast(finalAddModifier);
            }
        }
    }
    
    [FriendOf(typeof(DataModifierComponent))]
    public static class DataModifierHelper
    {
        /// <summary>
        /// 处理默认战斗相关
        /// </summary>
        /// <param name="value">攻击值</param>
        /// <param name="b">被攻击方</param>
        /// <param name="dataModifierTypeB">生命或护盾类型</param>
        /// <returns></returns>
        public static long DefaultBattle(long value, DataModifierComponent b, int dataModifierTypeB)
        {
            if (!b.NumericDic.ContainsKey(dataModifierTypeB))
            {
                Log.Error($"未找到{dataModifierTypeB}类型");
                return 0;
            }
            
            var values = b.GetValues(dataModifierTypeB);
            long add = values.Item1;
            long addMin = values.Item3;
            long pct = values.Item4;
            long finalAdd = values.Item7;
            long finalAddMin = values.Item9;
            long finalPct = values.Item10;

            long tmp = finalAdd - finalAddMin;
            value *= (1 / (1 + finalPct));
            if (value > tmp)
            {
                b.Add(new Default_Hp_FinalConstantModifier(-tmp));
                value -= tmp;

                tmp = add - addMin;
                value *= (1 / (1 + pct));
                if (value > tmp)
                {
                    b.Add(new Default_Hp_ConstantModifier(-tmp));
                    value -= tmp;
                }
                else
                {
                    b.Add(new Default_Hp_ConstantModifier(-value));
                    value = 0;
                }
            }
            else
            {
                b.Add(new Default_Hp_FinalConstantModifier(-value));
                value = 0;
            }

            return value;
        }
    }
}