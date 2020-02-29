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
    public class MetadataTypeDescription : TypeDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataTypeDescription"/> class.
        /// </summary>
        /// <param name="symbol">The wrapped roslyn symbol.</param>
        public MetadataTypeDescription(INamedTypeSymbol symbol)
            : base(symbol)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the type is abstract.
        /// </summary>
        /// <value><c>true</c> when the type is abstract.</value>
        public bool IsAbstract => this.Symbol.IsAbstract;

        /// <summary>
        /// Gets a value indicating whether the type is sealed.
        /// </summary>
        /// <value><c>true</c> if the type is sealed.</value>
        public bool IsSealed => this.Symbol.IsSealed;

        /// <summary>
        /// Gets a value indicating whether the type is generic.
        /// </summary>
        /// <value><c>true</c> when the type is generic.</value>
        public bool IsGeneric => this.Symbol.IsGenericType;

        /// <summary>
        /// Gets a value indicating whether the type is static.
        /// </summary>
        /// <value><c>true</c> if the type is static.</value>
        public bool IsStatic => this.Symbol.IsStatic;

        /// <summary>
        /// Gets a value indicating whether the type is ref-like (ref struct).
        /// </summary>
        /// <value><c>true</c> if the type is ref-like.</value>
        public bool IsRefLike => this.Symbol.IsRefLikeType;

        /// <summary>
        /// Gets a value indicating whether the type is readonly.
        /// </summary>
        /// <value><c>true</c> if the type is readonly.</value>
        public bool IsReadonly => this.Symbol.IsReadOnly;

        /// <summary>
        /// Gets the type's accessibility.
        /// </summary>
        /// <value>The type's accessibility.</value>
        public Accessibility Accessibility => this.Symbol.DeclaredAccessibility switch
        {
            Microsoft.CodeAnalysis.Accessibility.Public => Accessibility.Public,
            Microsoft.CodeAnalysis.Accessibility.Protected => Accessibility.Protected,
            Microsoft.CodeAnalysis.Accessibility.ProtectedOrInternal => Accessibility.ProtectedInternal,
            Microsoft.CodeAnalysis.Accessibility.Internal => Accessibility.Internal,
            Microsoft.CodeAnalysis.Accessibility.ProtectedAndInternal => Accessibility.PrivateProtected,
            Microsoft.CodeAnalysis.Accessibility.Private => Accessibility.Private,
            Microsoft.CodeAnalysis.Accessibility.NotApplicable => Accessibility.Unknown,
            _ => throw new NotImplementedException($"Type with unknown accessibility '{this.Symbol.DeclaredAccessibility}'")
        };

        /// <summary>
        /// Gets the symbol from base class downcasted to an instance of <see cref="INamedTypeSymbol"/>.
        /// </summary>
        /// <value>The symbol.</value>
        protected new INamedTypeSymbol Symbol => (INamedTypeSymbol)base.Symbol;
    }
}