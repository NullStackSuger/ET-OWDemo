using System;

namespace ET.Server
{
    [AttributeUsage(AttributeTargets.Field)]
    public class GenerateAttribute : BaseAttribute
    {
        public string Level;
    }
}