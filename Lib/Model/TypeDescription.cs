// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using System;
using System.Diagnostics;
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
        public TypeDescription(INamedTypeSymbol symbol)
        {
            Debug.Assert(!(symbol is IErrorTypeSymbol), $"{nameof(TypeDescription)} does not support unresolved types");
            Debug.Assert(!symbol.IsImplicitlyDeclared, $"{nameof(TypeDescription)} does not support compiler generated types");
            this.symbol = symbol;
        }

        /// <summary>
        /// Gets a type name without namespace.
        /// </summary>
        /// <value>The type name without namespace.</value>
        public string Name => this.symbol.MetadataName;

        /// <summary>
        /// Gets a value indicating whether the type is abstract.
        /// </summary>
        /// <value><c>true</c> when the type is abstract.</value>
        public bool IsAbstract => this.symbol.IsAbstract;

        /// <summary>
        /// Gets a value indicating whether the type is sealed.
        /// </summary>
        /// <value><c>true</c> if the type is sealed.</value>
        public bool IsSealed => this.symbol.IsSealed;

        /// <summary>
        /// Gets a value indicating whether the type is generic.
        /// </summary>
        /// <value><c>true</c> when the type is generic.</value>
        public bool IsGeneric => this.symbol.IsGenericType;

        /// <summary>
        /// Gets a value indicating whether the type is static.
        /// </summary>
        /// <value><c>true</c> if the type is static.</value>
        public bool IsStatic => this.symbol.IsStatic;

        /// <summary>
        /// Gets a value indicating whether the type is ref-like (ref struct).
        /// </summary>
        /// <value><c>true</c> if the type is ref-like.</value>
        public bool IsRefLike => this.symbol.IsRefLikeType;

        /// <summary>
        /// Gets a value indicating whether the type is readonly.
        /// </summary>
        /// <value><c>true</c> if the type is readonly.</value>
        public bool IsReadonly => this.symbol.IsReadOnly;

        /// <summary>
        /// Gets the type's accessibility.
        /// </summary>
        /// <value>The type's accessibility.</value>
        public Accessibility Accessibility => this.symbol.DeclaredAccessibility switch
        {
            Microsoft.CodeAnalysis.Accessibility.Public => Accessibility.Public,
            Microsoft.CodeAnalysis.Accessibility.Protected => Accessibility.Protected,
            Microsoft.CodeAnalysis.Accessibility.ProtectedOrInternal => Accessibility.ProtectedInternal,
            Microsoft.CodeAnalysis.Accessibility.Internal => Accessibility.Internal,
            Microsoft.CodeAnalysis.Accessibility.ProtectedAndInternal => Accessibility.PrivateProtected,
            Microsoft.CodeAnalysis.Accessibility.Private => Accessibility.Private,
            _ => throw new NotImplementedException($"Type with unknown accessibility '{this.symbol.DeclaredAccessibility}'")
        };

        /// <summary>
        /// Gets the type's kind.
        /// </summary>
        /// <value>The type's kind.</value>
        public TypeKind Kind => this.symbol.TypeKind switch
        {
            Microsoft.CodeAnalysis.TypeKind.Class => TypeKind.Class,
            Microsoft.CodeAnalysis.TypeKind.Interface => TypeKind.Interface,
            Microsoft.CodeAnalysis.TypeKind.Struct => TypeKind.Struct,
            Microsoft.CodeAnalysis.TypeKind.Enum => TypeKind.Enum,
            Microsoft.CodeAnalysis.TypeKind.Delegate => TypeKind.Delegate,
            _ => throw new NotImplementedException($"Unknown type kind '{this.symbol.TypeKind}'")
        };
    }
}