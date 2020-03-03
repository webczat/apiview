// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Apiview.Tests
{
    /// <summary>
    /// This class serves as a base for all tests.
    /// </summary>
    public class TestBase
    {
        protected static readonly ImmutableArray<byte> BaseMetadataImage = ImmutableArray.Create(File.ReadAllBytes(typeof(object).Assembly.Location));
        protected static readonly MetadataReference BaseMetadata = MetadataReference.CreateFromImage(BaseMetadataImage, MetadataReferenceProperties.Assembly, null, typeof(object).Assembly.Location);

        /// <summary>
        /// Initializes a new instance of the <see cref="TestBase"/> class.
        /// </summary>
        protected TestBase()
        {
        }

        /// <summary>
        /// Creates a <see cref="Compilation"/> instance based on a given c# source code fragment.
        /// </summary>
        /// <param name="source">Source text to compile, <c>null</c> for an empty compilation.</param>
        /// <param name="assemblyName">The assembly name, <c>null</c> for no name.</param>
        /// <returns>The <see cref="Compilation"/> instance.</returns>
        protected static Compilation CreateCompilation(string? source = null, string? assemblyName = null)
        {
            // Start with an empty compilation
            var compilation = CSharpCompilation.Create(assemblyName).WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            // Optionally add syntax tree to interpret, parsed from a given source fragment.
            if (source != null)
            {
                compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(source));
            }

            return compilation;
        }

        /// <summary>
        /// Compiles the given source fragment to a single module assembly, and returns it's image.
        /// </summary>
        /// <param name="source">The source fragment to compile, or null for empty assembly.</param>
        /// <param name="assemblyName">The assembly name to compile.</param>
        /// <param name="assemblies">References to other assemblies. Note that a reference to an assembly containing <c>System.Object</c> is always added.</param>
        /// <exception cref="EmitException">Thrown when assembly emitting fails.</exception>
        /// <returns>The compiled assembly as an immutable byte array.</returns>
        protected static ImmutableArray<byte> CompileAssembly(string? source = null, string assemblyName = "TestAssembly", params ImmutableArray<byte>[] assemblies)
        {
            var references = from a in assemblies
                             select MetadataReference.CreateFromImage(a, MetadataReferenceProperties.Assembly);
            var compilation = CreateCompilation(source, assemblyName).AddReferences(references).AddReferences(BaseMetadata);
            using var str = new MemoryStream();
            var result = compilation.Emit(str);
            if (!result.Success)
            {
                throw new EmitException($"Unable to compile assembly {assemblyName}.", result.Diagnostics);
            }

            return ImmutableArray.Create(str.GetBuffer(), 0, (int)str.Length);
        }

        /// <summary>
        /// Creates a new documentation object based on the given compiled assemblies.
        /// </summary>
        /// <param name="assemblies">The compiled assembly images.</param>
        /// <returns>A <see cref="Task{TResult}"/> providing the new instance of <see cref="Documentation"/> after completion.</returns>
        protected static async Task<Documentation> RetrieveDocumentationAsync(params ImmutableArray<byte>[] assemblies)
        {
            var builder = Documentation.CreateBuilder();
            foreach (var assembly in assemblies)
            {
                _ = builder.AddAssembly(assembly);
            }

            // We should actually add the assembly containing System.Object and other fundamentals.
            _ = builder.AddAssembly(BaseMetadataImage);
            return await builder.BuildAsync();
        }

        /// <summary>
        /// Creates a <see cref="Documentation"/> object using a source fragment.
        /// </summary>
        /// <remarks>
        /// <para>If you need to generate documentation from different inputs, change the assembly name or combine multiple assemblies, see the overload accepting byte arrays.</para>
        /// </remarks>
        /// <param name="source">The source to compile.</param>
        /// <returns>The <see cref="Task{TResult}"/> containing the <see cref="Documentation"/> object after completion.</returns>
        protected static Task<Documentation> RetrieveDocumentationAsync(string? source = null)
        {
            return RetrieveDocumentationAsync(CompileAssembly(source));
        }

        /// <summary>
        /// Retrieves the given type name from the source fragment.
        /// </summary>
        /// <remarks>
        /// <para>The source fragment is first compiled, and a documentation object is generated for it.</para>
        /// </remarks>
        /// <param name="source">The source fragment to compile.</param>
        /// <param name="typeName">The full type metadata name that is expected to be found in the fragment.</param>
        /// <returns>The type.</returns>
        protected static async Task<MetadataTypeDescription> RetrieveTypeFromSourceFragmentAsync(string source, string typeName)
        {
            var doc = await RetrieveDocumentationAsync(source);
            return doc.GetMetadataType(typeName)!;
        }
    }
}