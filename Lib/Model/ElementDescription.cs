// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace Apiview.Model
{
    /// <summary>
    /// This class describes all program elements that are of interest to the documentation library. Instances of this class can describe assemblies and modules, namespaces and types, their members, method parameters etc.
    /// </summary>
    public abstract class ElementDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementDescription"/> class.
        /// </summary>
        /// <param name="symbol">The roslyn symbol to wrap.</param>
        protected ElementDescription(ISymbol symbol)
        {
            Debug.Assert(!symbol.IsImplicitlyDeclared, $"{nameof(ElementDescription)} does not support compiler generated types");
            this.Symbol = symbol;
        }

        /// <summary>
        /// Gets the symbol name.
        /// </summary>
        /// <remarks>
        /// <para>Some elements, such as many types, do not have names. In case of named elements, the name returned is a metadata name along with generic type parameter count, if applicable, but without containing type/namespace name.</para>
        /// </remarks>
        /// <value>The symbol name, or null if the type is not named.</value>
        public virtual string? Name => this.Symbol.MetadataName;

        /// <summary>
        /// Gets the element's accessibility.
        /// </summary>
        /// <value>The type's accessibility.</value>
        public AccessibilityModifier? Accessibility => this.Symbol.DeclaredAccessibility switch
        {
            Microsoft.CodeAnalysis.Accessibility.Public => AccessibilityModifier.Public,
            Microsoft.CodeAnalysis.Accessibility.Protected => AccessibilityModifier.Protected,
            Microsoft.CodeAnalysis.Accessibility.ProtectedOrInternal => AccessibilityModifier.ProtectedInternal,
            Microsoft.CodeAnalysis.Accessibility.Internal => AccessibilityModifier.Internal,
            Microsoft.CodeAnalysis.Accessibility.ProtectedAndInternal => AccessibilityModifier.PrivateProtected,
            Microsoft.CodeAnalysis.Accessibility.Private => AccessibilityModifier.Private,
            Microsoft.CodeAnalysis.Accessibility.NotApplicable => null,
            _ => throw new NotImplementedException($"Type with unknown accessibility '{this.Symbol.DeclaredAccessibility}'")
        };

        /// <summary>
        /// Gets the wrapped symbol.
        /// </summary>
        /// <value>The wrapped symbol.</value>
        protected ISymbol Symbol
        {
            get;
        }
    }
}