// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace Apiview.Model
{
    /// <summary>
    /// Describes any type, whether user defined or not, including builtin types, array and pointer types, etc.
    /// Instances of this class represent both type usages and type definitions.
    /// </summary>
    public abstract class TypeDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDescription"/> class.
        /// </summary>
        /// <param name="symbol">The symbol instance.</param>
        protected TypeDescription(ITypeSymbol symbol)
        {
            Debug.Assert(!symbol.IsImplicitlyDeclared, $"{nameof(MetadataTypeDescription)} does not support compiler generated types");
            this.Symbol = symbol;
        }

        /// <summary>
        /// Gets a type name without namespace.
        /// </summary>
        /// <value>The type name without namespace, or null if the type is not named.</value>
        public virtual string? Name => this.Symbol.MetadataName;

        /// <summary>
        /// Gets the type's kind.
        /// </summary>
        /// <value>The type's kind.</value>
        public TypeKind Kind => this.Symbol.TypeKind switch
        {
            Microsoft.CodeAnalysis.TypeKind.Class => TypeKind.Class,
            Microsoft.CodeAnalysis.TypeKind.Interface => TypeKind.Interface,
            Microsoft.CodeAnalysis.TypeKind.Struct => TypeKind.Struct,
            Microsoft.CodeAnalysis.TypeKind.Enum => TypeKind.Enum,
            Microsoft.CodeAnalysis.TypeKind.Delegate => TypeKind.Delegate,
            Microsoft.CodeAnalysis.TypeKind.Error => TypeKind.Missing,
            _ => throw new NotImplementedException($"Unknown type kind '{this.Symbol.TypeKind}'")
        };

        /// <summary>
        /// Gets the wrapped symbol.
        /// </summary>
        /// <value>The wrapped symbol.</value>
        protected ITypeSymbol Symbol
        {
            get;
        }
    }
}