// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using System.Collections.Generic;
using System.Collections.Immutable;
using Apiview.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Apiview
{
    /// <summary>
    /// Instances of this class represent a loaded, immutable documentation model.
    /// </summary>
    /// <remarks>
    /// <para>Instances of this class can be used to retrieve assembly, namespace, type and type member metadata along with their documentation.</para>
    /// <para>The documentation can be dynamically generated from many sources, including from actual source code or compiled metadata.</para>
    /// <para>Documentation objects can be created using a builder. To get a builder, use one of the static methods in this class returning a <see cref="DocumentationBuilder"/> instance.</para>
    /// </remarks>
    public class Documentation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Documentation"/> class.
        /// </summary>
        /// <param name="assemblies">The assembly images to be part of the compilation.</param>
        internal Documentation(IEnumerable<ImmutableArray<byte>> assemblies)
        {
            var compilation = CSharpCompilation.Create(null);
            foreach (var assembly in assemblies)
            {
                compilation = compilation.AddReferences(MetadataReference.CreateFromImage(assembly));
            }

            this.Compilation = compilation;
        }

        /// <summary>
        /// Gets the roslyn's <see cref="Compilation"/> used as the metadata source.
        /// </summary>
        /// <value>The compilation object.</value>
        protected virtual Compilation Compilation
        {
            get;
        }

        /// <summary>
        /// Creates a <see cref="DocumentationBuilder"/> for building documentation based on compiled packages and assemblies.
        /// </summary>
        /// <returns>A new documentation builder.</returns>
        public static DocumentationBuilder CreateBuilder()
        {
            return new DocumentationBuilder();
        }

        /// <summary>
        /// Retrieves a metadatadata type given by name.
        /// </summary>
        /// <remarks>
        /// <para>The type is searched in all referenced assemblies.</para>
        /// </remarks>
        /// <param name="name">The full metadata name of the type, including arity.</param>
        /// <returns>The type that was found, or <c>null</c> if not found.</returns>
        public MetadataTypeDescription? GetMetadataType(string name)
        {
            var symbol = this.Compilation.GetTypeByMetadataName(name);
            return symbol != null ? new MetadataTypeDescription(symbol) : null;
        }
    }
}