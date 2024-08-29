using System.Collections.Generic;
using ET.Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ET.Generator;

[Generator(LanguageNames.CSharp)]
public class ETDataModifierGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(SyntaxContextReceiver.Create);
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxContextReceiver is not SyntaxContextReceiver receiver)
        {
            return;
        }
        
        foreach (string modifierName in receiver.ModifierTypes)
        {
            string code = GenerateCode1("ET", modifierName);
            context.AddSource($"ET.{modifierName}Modifier.g.cs", code);

            foreach (var pair in receiver.DataModifierTypes)
            {
                string namespaceName = pair.Item1;
                string dataModifierName = pair.Item2;
                
                code = GenerateCode2(namespaceName, modifierName, dataModifierName);
                context.AddSource($"{namespaceName}.{dataModifierName}_{modifierName}Modifier.g.cs", code);
                
                code = GenerateCode3(namespaceName, modifierName, dataModifierName);
                context.AddSource($"{namespaceName}.Default_{dataModifierName}_{modifierName}Modifier.g.cs", code);
            }
        }
    }

    /// <summary>
    /// 生成"修改器类型Modifier"
    /// </summary>
    private string GenerateCode1(string namespaceName, string modifierName)
    {
        return $$"""
                namespace {{namespaceName}}
                {
                    public abstract class {{modifierName}}Modifier : ADataModifier
                    {
                        public override int ModifierType { get; } = ET.ModifierType.{{modifierName}};   
                    }
                }
                """;
    }

    /// <summary>
    /// 生成"数值类型_修改器类型Modifier"
    /// </summary>
    private string GenerateCode2(string namespaceName, string modifierName, string dataModifierName)
    {
        return $$"""
                 namespace {{namespaceName}}
                 {
                     public abstract class {{dataModifierName}}_{{modifierName}}Modifier : {{modifierName}}Modifier
                     {
                         public override int Key { get; } = ET.DataModifierType.{{dataModifierName}};
                     }
                 }
                 """;
    }

    /// <summary>
    /// 生成"Default_数值类型_修改器类型Modifier"
    /// </summary>
    private string GenerateCode3(string namespaceName, string modifierName, string dataModifierName)
    {
        return $$"""
                 namespace {{namespaceName}}
                 {
                     public class Default_{{dataModifierName}}_{{modifierName}}Modifier : {{dataModifierName}}_{{modifierName}}Modifier
                     {
                         public static Default_{{dataModifierName}}_{{modifierName}}Modifier Creat(bool isFromPool = false)
                         {
                            return ObjectPool.Instance.Fetch(typeof(Default_{{dataModifierName}}_{{modifierName}}Modifier), isFromPool) as Default_{{dataModifierName}}_{{modifierName}}Modifier;
                         }
                     }
                 }
                 """;
    }
    
    class SyntaxContextReceiver: ISyntaxContextReceiver
    {
        internal static ISyntaxContextReceiver Create()
        {
            return new SyntaxContextReceiver();
        }

        /// <summary>
        /// key:namespace value:DataModifierType
        /// </summary>
        public readonly List<(string, string)> DataModifierTypes = new();

        public readonly List<string> ModifierTypes = new();
        
        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            // 这里面有2个坑
            // 1.不能直接用字典Add, 但可以参考ETSystemGenerator中dic[key] = value
            // 2.不能在这个函数里嵌套函数
            
            SyntaxNode node = context.Node;
            
            if (node is not ClassDeclarationSyntax classDeclarationSyntax) return;
            
            if (classDeclarationSyntax.AttributeLists.Count == 0) return;
            
            var classTypeSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) as INamedTypeSymbol;
            if (classTypeSymbol == null) return;
            
            string? nameSpace = classTypeSymbol.GetNameSpace();
            if (nameSpace == null) return;
            
            // 筛选出含有 DataModifier标签的
            var dataModifierAttrData = classTypeSymbol.GetFirstAttribute(Definition.DataModifierAttribute);
            if (dataModifierAttrData != null)
            {
                foreach (var childNode in classTypeSymbol.GetMembers())
                {
                    if (childNode is not IFieldSymbol fieldSymbol) continue;

                    string name = fieldSymbol.Name;

                    if (name == "Max" || name == "Min") continue;

                    DataModifierTypes.Add((nameSpace, name));
                }
            }

            // 筛选出含有 Modifier标签的
            var modifierAttrData = classTypeSymbol.GetFirstAttribute(Definition.ModifierAttribute);
            if (modifierAttrData != null)
            {
                foreach (var childNode in classTypeSymbol.GetMembers())
                {
                    if (childNode is not IFieldSymbol fieldSymbol) continue;

                    string name = fieldSymbol.Name;
                    
                    if (name == "Max" || name == "Min") continue;
                    
                    ModifierTypes.Add(name);
                }
            }
        }
    }
}