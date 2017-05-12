using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NewRelicInstrumentationGenerator
{
    public class HandlerClassCollector : CSharpSyntaxWalker
    {
        private readonly string _baseClassName;
        public List<ValueTuple<string, string>> Result = new List<(string, string)>();

        public HandlerClassCollector(string baseClassName)
        {
            _baseClassName = baseClassName;
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (node.BaseList == null) return;

            var typeNode = node.BaseList.Types[0].Type;
            var baseClassIdentifier = typeNode.GetFirstToken().ToString();

            if (baseClassIdentifier != _baseClassName) return;

            var typeArgument = new string(typeNode.ToString().Substring(baseClassIdentifier.Length).Skip(1).Reverse().Skip(1).Reverse().ToArray());

            if (typeArgument.Contains(","))
            {
                typeArgument = typeArgument.Split(',')[0];
            }

            Result.Add((node.Identifier.ToString(), typeArgument));
        }
    }
}