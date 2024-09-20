using System;
using ET;

namespace ET
{
    [AttributeUsage(AttributeTargets.Field)]
    public class GenerateAttribute : BaseAttribute
    {
        public string Level;
    }
}