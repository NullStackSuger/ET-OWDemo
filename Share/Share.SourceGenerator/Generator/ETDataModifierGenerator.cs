using System.Collections.Generic;
using System.Linq;
using ET.Analyzer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
        if (context.SyntaxContextReceiver is not SyntaxContextReceiver receiver) return;

        if (receiver.Generates.Any())
        {
            foreach (string modifierType in receiver.ModifierTypes)
            {
                context.AddSource($"ET.Server.{modifierType}Modifier.g.cs", GenerateCode1("ET.Server", modifierType));
            }
        }

        foreach (var pair in receiver.Generates)
        {
            string nameSpace = pair.Item1;
            string modifierType = pair.Item2;
            string dataModifierType = pair.Item3;
            
            context.AddSource($"{nameSpace}.{dataModifierType}_{modifierType}Modifier.g.cs", GenerateCode2(nameSpace, modifierType, dataModifierType));
            context.AddSource($"{nameSpace}.Default_{dataModifierType}_{modifierType}Modifier.g.cs", GenerateCode3(nameSpace, modifierType, dataModifierType));
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
                        public override string ModifierType { get; } = ET.Server.ModifierType.{{modifierName}};   
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
                         public override int Key { get; } = ET.Server.DataModifierType.{{dataModifierName}};
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

        public readonly List<string> ModifierTypes =
        [
            "Constant", "ConstantMax", "ConstantMin",
            "Percentage", "PercentageMax", "PercentageMin",
            "FinalConstant", "FinalConstantMax", "FinalConstantMin",
            "FinalPercentage", "FinalPercentageMax", "FinalPercentageMin",
            "FinalMax", "FinalMin"
        ];

        public readonly List<(string, string, string)> Generates = new();
        
        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            // 这里面有2个坑
            // 1.不能直接用字典Add, 但可以参考ETSystemGenerator中dic[key] = value
            // 2.不能在这个函数里嵌套函数
            
            SyntaxNode node = context.Node;
            
            if (node is not FieldDeclarationSyntax fieldDeclarationSyntax) return;

            AttributeSyntax? attributeSyntax = fieldDeclarationSyntax.GetAttribute("Generate");
            if (attributeSyntax == null) return;
            
            var attrArgs = attributeSyntax.ArgumentList?.Arguments.FirstOrDefault(args =>
                args.NameEquals?.Name.Identifier.ValueText == "Level");
            if (attrArgs == null || attrArgs.Expression.Kind() != SyntaxKind.StringLiteralExpression) return;
            
            string nameSpace = fieldDeclarationSyntax.GetNamespace();
            
            string dataModifierType = fieldDeclarationSyntax.Declaration.Variables.First().Identifier.ValueText;
            
            string level = ((LiteralExpressionSyntax)attrArgs.Expression).Token.ValueText;
            switch (level)
            {
                case "All":
                    foreach (string modifierType in ModifierTypes)
                    {
                        Generates.Add((nameSpace, modifierType, dataModifierType));
                    }
                    break;
                case "Constant":
                    Generates.Add((nameSpace, "Constant", dataModifierType));
                    Generates.Add((nameSpace, "FinalMax", dataModifierType));
                    Generates.Add((nameSpace, "FinalMin", dataModifierType));
                    break;
                case "Constant Extremum":
                    Generates.Add((nameSpace, "Constant", dataModifierType));
                    Generates.Add((nameSpace, "ConstantMax", dataModifierType));
                    Generates.Add((nameSpace, "ConstantMin", dataModifierType));
                    Generates.Add((nameSpace, "FinalMax", dataModifierType));
                    Generates.Add((nameSpace, "FinalMin", dataModifierType));
                    break;
                case "FinalConstant":
                    Generates.Add((nameSpace, "Constant", dataModifierType));
                    Generates.Add((nameSpace, "FinalConstant", dataModifierType));
                    Generates.Add((nameSpace, "FinalMax", dataModifierType));
                    Generates.Add((nameSpace, "FinalMin", dataModifierType));
                    break;
                case "FinalConstant Extremum":
                    Generates.Add((nameSpace, "Constant", dataModifierType));
                    Generates.Add((nameSpace, "ConstantMax", dataModifierType));
                    Generates.Add((nameSpace, "ConstantMin", dataModifierType));
                    
                    Generates.Add((nameSpace, "FinalConstant", dataModifierType));
                    Generates.Add((nameSpace, "FinalConstantMax", dataModifierType));
                    Generates.Add((nameSpace, "FinalConstantMin", dataModifierType));
                    
                    Generates.Add((nameSpace, "FinalMax", dataModifierType));
                    Generates.Add((nameSpace, "FinalMin", dataModifierType));
                    break;
                case "None":
                    break;
                case "":
                    break;
                default:
                    Generates.Add((nameSpace, level, dataModifierType));
                    break;
            }
        }
    }
}