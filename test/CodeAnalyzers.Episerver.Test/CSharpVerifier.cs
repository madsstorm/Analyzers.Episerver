﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using System.Reflection;
using System.Threading.Tasks;

namespace CodeAnalyzers.Episerver.Test
{
    internal class CSharpVerifier<TAnalyzer>
        where TAnalyzer : DiagnosticAnalyzer, new()
    {
        public static DiagnosticResult Diagnostic()
            => CSharpCodeFixVerifier<TAnalyzer, EmptyCodeFixProvider, XUnitVerifier>.Diagnostic();

        public static DiagnosticResult Diagnostic(string diagnosticId)
            => CSharpCodeFixVerifier<TAnalyzer, EmptyCodeFixProvider, XUnitVerifier>.Diagnostic(diagnosticId);

        public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
            => new DiagnosticResult(descriptor);

        public static DiagnosticResult CompilerError(string errorIdentifier)
            => new DiagnosticResult(errorIdentifier, DiagnosticSeverity.Error);

        public static Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected)
        {
            var test = new EpiserverTest { TestCode = source };
            test.ExpectedDiagnostics.AddRange(expected);
            return test.RunAsync();
        }

        private class EpiserverTest : CSharpCodeFixTest<TAnalyzer, EmptyCodeFixProvider, XUnitVerifier>
        {
            public EpiserverTest()
            {
                SolutionTransforms.Add((solution, projectId) =>
                {
                    solution = solution
                        .AddMetadataReference(projectId, MetadataReference.CreateFromFile(typeof(EPiServer.DataFactory).GetTypeInfo().Assembly.Location))
                        .AddMetadataReference(projectId, MetadataReference.CreateFromFile(typeof(EPiServer.Data.Entity.IReadOnly).GetTypeInfo().Assembly.Location))
                        .AddMetadataReference(projectId, MetadataReference.CreateFromFile(typeof(EPiServer.Web.Routing.IRoutable).GetTypeInfo().Assembly.Location))
                        .AddMetadataReference(projectId, MetadataReference.CreateFromFile(typeof(EPiServer.Core.PageReference).GetTypeInfo().Assembly.Location))
                        .AddMetadataReference(projectId, MetadataReference.CreateFromFile(typeof(Mediachase.Commerce.Currency).GetTypeInfo().Assembly.Location))
                        .AddMetadataReference(projectId, MetadataReference.CreateFromFile(typeof(EPiServer.Commerce.Order.Internal.DefaultOrderEvents).GetTypeInfo().Assembly.Location));

                    return solution;
                });

                Exclusions &= ~AnalysisExclusions.GeneratedCode;
            }
        }
    }
}