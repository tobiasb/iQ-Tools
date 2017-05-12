using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NewRelicInstrumentationGenerator
{
    public abstract class NewRelicInstrumentationBase
    {
        private readonly string _baseFolder;
        private readonly string _baseClassIdentifier;

        private const string NewRelicFileBase =
@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<extension xmlns=""urn:newrelic-extension"">
  <instrumentation>
{0}
  </instrumentation>
</extension>
";

        protected NewRelicInstrumentationBase(string baseFolder, string baseClassIdentifier)
        {
            _baseFolder = baseFolder;
            _baseClassIdentifier = baseClassIdentifier;
        }

        protected abstract string GenerateTracerFactoryXml((string, string) tuple);

        public string Generate()
        {
            var result = GetTypesAndTypeArgument(_baseFolder, _baseClassIdentifier);
            var tracerFactoriesXml = string.Join(Environment.NewLine, result.Select(GenerateTracerFactoryXml));
            return string.Format(NewRelicFileBase, tracerFactoriesXml);
        }

        private static IEnumerable<(string, string)> GetTypesAndTypeArgument(string path, string baseClassName)
        {
            var files = Directory.GetFiles(path, "*.cs");

            var classWalker = new HandlerClassCollector(baseClassName);

            foreach (var file in files)
            {
                var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(Path.Combine(path, file)));

                var root = (CompilationUnitSyntax)tree.GetRoot();

                classWalker.Visit(root);
            }

            return classWalker.Result;
        }
    }
}
