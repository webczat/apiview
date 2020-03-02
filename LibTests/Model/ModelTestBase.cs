// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using System.Threading.Tasks;
using Apiview.Model;
using Apiview.Tests;

namespace Apiview.Tests.Model
{
    /// <summary>
    /// Base for all model tests.
    /// </summary>
    public abstract class ModelTestBase : TestBase
    {
        protected ModelTestBase()
        {
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