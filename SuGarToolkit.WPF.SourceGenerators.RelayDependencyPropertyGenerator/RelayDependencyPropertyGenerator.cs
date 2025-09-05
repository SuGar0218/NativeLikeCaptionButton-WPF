using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace SuGarToolkit.WPF.SourceGenerators
{
    [Generator]
    class RelayDependencyPropertyGenerator : IIncrementalGenerator
    {
        private const string TargetAttributeFullQualifiedName = "SuGarToolkit.WPF.SourceGenerators.RelayDependencyPropertyAttribute";

        private struct RelayDependencyPropertyInfo
        {
            public IPropertySymbol PropertySymbol { get; set; }
            public string DefaultValueLiteral { get; set; }
            public string TargetPropertyPath { get; set; }
        }

        public void Initialize(IncrementalGeneratorInitializationContext initContext)
        {
            //System.Diagnostics.Debugger.Launch();

            initContext.RegisterPostInitializationOutput(postContext =>
            {
                postContext.AddSource($"{TargetAttributeFullQualifiedName}.g.cs", @"
using System;

namespace SuGarToolkit.WPF.SourceGenerators
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class RelayDependencyPropertyAttribute : Attribute
    {
        public RelayDependencyPropertyAttribute(string TargetPropertyPath)
        {
            _path = TargetPropertyPath;
        }

        public object DefaultValue { get; set; }

        public string DefaultValuePath { get; set; }

        private readonly string _path;
    }
}");
            });

            IncrementalValueProvider<ImmutableArray<RelayDependencyPropertyInfo>> propertyInfosProvider = initContext.SyntaxProvider.ForAttributeWithMetadataName(
                TargetAttributeFullQualifiedName,
                (syntaxNode, _) => syntaxNode is PropertyDeclarationSyntax,
                (syntaxContext, _) =>
                {
                    AttributeData associatedAttribute = syntaxContext.Attributes[0];
                    string defaultValueLiteral = GetPropertyLiteral(associatedAttribute, "DefaultValue");
                    string propertyChangedCallbackLiteral = GetPropertyLiteral(associatedAttribute, "OnPropertyChanged");
                    string targetPropertyPath = associatedAttribute.ConstructorArguments[0].ToCSharpString();
                    targetPropertyPath = targetPropertyPath.Substring(1, targetPropertyPath.Length - 2);

                    if (!string.IsNullOrEmpty(defaultValueLiteral))
                    {
                        return new RelayDependencyPropertyInfo
                        {
                            PropertySymbol = (IPropertySymbol) syntaxContext.TargetSymbol,
                            DefaultValueLiteral = defaultValueLiteral,
                            TargetPropertyPath = targetPropertyPath
                        };
                    }

                    string defaultValuePath = GetPropertyLiteral(associatedAttribute, "DefaultValuePath");
                    if (string.IsNullOrEmpty(defaultValuePath))
                    {
                        return new RelayDependencyPropertyInfo
                        {
                            PropertySymbol = (IPropertySymbol) syntaxContext.TargetSymbol,
                            TargetPropertyPath = targetPropertyPath
                        };
                    }
                    else
                    {
                        defaultValuePath = defaultValuePath.Substring(1, defaultValuePath.Length - 2);
                        return new RelayDependencyPropertyInfo
                        {
                            PropertySymbol = (IPropertySymbol) syntaxContext.TargetSymbol,
                            DefaultValueLiteral = defaultValuePath,
                            TargetPropertyPath = targetPropertyPath
                        };
                    }
                }
            ).Collect();

            initContext.RegisterSourceOutput(propertyInfosProvider, (sourceProductionContext, propertyInfos) =>
            {
                IEnumerable<IGrouping<ISymbol, RelayDependencyPropertyInfo>> groupedByClass = propertyInfos.GroupBy(
                    dependencyPropertyInfo => dependencyPropertyInfo.PropertySymbol.ContainingType,
                    SymbolEqualityComparer.Default);

                foreach (IGrouping<ISymbol, RelayDependencyPropertyInfo> group in groupedByClass)
                {
                    INamedTypeSymbol classSymbol = (INamedTypeSymbol) group.Key;
                    StringBuilder stringBuilder = new StringBuilder().AppendLine($@"
using System;
using System.Windows;

namespace {classSymbol.ContainingNamespace}
{{
    {GetAccessibilityLiteral(classSymbol.DeclaredAccessibility)} {(classSymbol.IsAbstract ? "abstract" : "")} partial class {classSymbol.Name}
    {{");
                    foreach (RelayDependencyPropertyInfo dependencyPropertyInfo in group)
                    {
                        string accessModifier = GetAccessibilityLiteral(dependencyPropertyInfo.PropertySymbol.DeclaredAccessibility);
                        string propertyTypeName = $"{dependencyPropertyInfo.PropertySymbol.Type.ContainingNamespace}.{dependencyPropertyInfo.PropertySymbol.Type.Name}";
                        string propertyName = dependencyPropertyInfo.PropertySymbol.Name;
                        string ownerClassName = dependencyPropertyInfo.PropertySymbol.ContainingType.Name;
                        stringBuilder.AppendLine($@"
        {accessModifier} partial {dependencyPropertyInfo.PropertySymbol.Type} {propertyName}
        {{
            get => ({propertyTypeName}) GetValue({propertyName}Property);
            set => SetValue({propertyName}Property, value);
        }}");
                        if (!string.IsNullOrEmpty(dependencyPropertyInfo.DefaultValueLiteral))
                        {
                            stringBuilder.AppendLine($@"
        {accessModifier} static readonly DependencyProperty {propertyName}Property = DependencyProperty.Register(
            nameof({propertyName}),
            typeof({propertyTypeName}),
            typeof({ownerClassName}),
            new PropertyMetadata({dependencyPropertyInfo.DefaultValueLiteral}, (d, e) =>
            {{
                {classSymbol} self = ({classSymbol}) d;
                self.{dependencyPropertyInfo.TargetPropertyPath} = ({propertyTypeName}) e.NewValue;
            }})
        );
                            ");
                        }
                        else
                        {
                            stringBuilder.AppendLine($@"
        {accessModifier} static readonly DependencyProperty {propertyName}Property = DependencyProperty.Register(
            nameof({propertyName}),
            typeof({propertyTypeName}),
            typeof({ownerClassName}),
            new PropertyMetadata(default({propertyTypeName}), (d, e) =>
            {{
                {classSymbol} self = ({classSymbol}) d;
                self.{dependencyPropertyInfo.TargetPropertyPath} = ({propertyTypeName}) e.NewValue;
            }})
        );");
                        }
                    }
                    stringBuilder.AppendLine(@"
        /// <summary>
        /// Generated by RelayDependencyPropertyGenerator.
        /// <br/>
        /// Sync value from target properties to relay dependency properties.
        /// <br/>
        /// Please invoke this after target properties object is prepared to be accessed.
        /// </summary>
        private void InitializeRelayDependencyProperties()
        {");
                    foreach (RelayDependencyPropertyInfo dependencyPropertyInfo in group)
                    {
                        stringBuilder.AppendLine($@"
            {dependencyPropertyInfo.PropertySymbol.Name} = {dependencyPropertyInfo.TargetPropertyPath};");
                    }
                    stringBuilder.AppendLine(@"
        }");
                    stringBuilder.AppendLine($@"
    }}
}}");
                    sourceProductionContext.AddSource($"{classSymbol}.RelayDependencyProperty.g.cs", stringBuilder.ToString());
                }
            });
        }

        private static string GetPropertyLiteral(AttributeData attribute, string name)
        {
            foreach (KeyValuePair<string, TypedConstant> pair in attribute.NamedArguments)
            {
                if (pair.Key == name)
                {
                    return pair.Value.ToCSharpString();
                }
            }
            return null;
        }

        private static string GetAccessibilityLiteral(Accessibility accessibility)
        {
            switch (accessibility)
            {
                case Accessibility.Private:
                    return "private";
                case Accessibility.ProtectedAndInternal:
                    return "protected internal";
                case Accessibility.Protected:
                    return "protected";
                case Accessibility.Internal:
                    return "internal";
                case Accessibility.Public:
                    return "public";
                default:
                    break;
            }
            return string.Empty;
        }
    }
}
