// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Apiview
{
    /// <summary>
    /// Instances of this class are used to build <see cref="Documentation"/> objects. One can set all parameters related to documentation loading and specify inputs like source code, packages or assemblies.
    /// </summary>
    public class DocumentationBuilder
    {
        private IList<ImmutableArray<byte>> assemblies = new List<ImmutableArray<byte>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentationBuilder"/> class.
        /// </summary>
        internal DocumentationBuilder()
        {
        }

        /// <summary>
        /// Adds an assembly to the set of referenced assemblies that the documentation object will process.
        /// </summary>
        /// <param name="assembly">The loaded assembly.</param>
        /// <returns>This instance.</returns>
        public DocumentationBuilder AddAssembly(ImmutableArray<byte> assembly)
        {
            this.assemblies.Add(assembly);
            return this;
        }

        /// <summary>
        /// Builds the documentation.
        /// </summary>
        /// <remarks>
        /// <para>This operation may cause disk and network I/O because of downloading packages and loading assemblies.</para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when trying to create documentation with no inputs.</exception>
        /// <returns>The task returning the newly created documentation on completion.</returns>
        public Task<Documentation> BuildAsync()
        {
            if (this.assemblies.Count == 0)
            {
                throw new InvalidOperationException("Unable to create documentation with no inputs");
            }

            return Task.FromResult(new Documentation(this.assemblies));
        }
    }
}