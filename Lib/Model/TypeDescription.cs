// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using Microsoft.CodeAnalysis;

namespace Apiview.Model
{
    /// <summary>
    /// Instances of this class describe an user defined named type, whether a class, struct, interface, enum or delegate.
    /// </summary>
    public class TypeDescription
    {
        // The underlying ISymbol describing the type
        private readonly INamedTypeSymbol symbol;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDescription"/> class.
        /// </summary>
        /// <param name="symbol">The wrapped roslyn symbol.</param>
        public TypeDescription(INamedTypeSymbol symbol) => this.symbol = symbol;

        /// <summary>
        /// Gets a type name without namespace.
        /// </summary>
        /// <value>The type name without namespace.</value>
        public string Name => this.symbol.MetadataName;
    }
}