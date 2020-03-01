// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
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
            // This constructor can be called only for INamedTypeSymbols proper, unless it is itself a subclass.
            Debug.Assert(this.GetType() != typeof(MetadataTypeDescription) || this.Symbol is INamedTypeSymbol, $"Cannot pass a symbol of type {this.Symbol.TypeKind} directly to {nameof(MetadataTypeDescription)}");
            this.BaseType = this.Symbol.BaseType switch
            {
                IErrorTypeSymbol e => new MissingMetadataTypeDescription(e),
                INamedTypeSymbol n => new MetadataTypeDescription(n),
                null => null,
            };
            var interfaceQuery = from i in this.Symbol.Interfaces
                                 select i switch
                                 {
                                     IErrorTypeSymbol e => new MissingMetadataTypeDescription(e),
                                     INamedTypeSymbol n => new MetadataTypeDescription(n),
                                 };
            this.Interfaces = interfaceQuery.ToImmutableArray();
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
        /// Gets the base type of this type.
        /// </summary>
        /// <remarks>
        /// <para>The base type returned by this property may be missing. In that case it is an instance of <see cref="MissingMetadataTypeDescription"/>.</para>
        /// <para>The only type for which this property gives <c>null</c> should be <c>System.Object</c>, as it is the only metadata type having no base.</para>
        /// </remarks>
        /// <value>Base type of this type, or null when there is no base type.</value>
        public MetadataTypeDescription? BaseType
        {
            get;
        }

        /// <summary>
        /// Gets all interfaces this type directly implements.
        /// </summary>
        /// <remarks>
        /// <para>Some of the interfaces returned may be missing. In that case, an instance of <see cref="MissingMetadataTypeDescription"/> is returned in their place.</para>
        /// </remarks>
        /// <value>An array of interfaces that this type implements, can be empty if this type does not implement any interfaces.</value>
        public ImmutableArray<MetadataTypeDescription> Interfaces
        {
            get;
        }

        /// <summary>
        /// Gets the symbol from base class downcasted to an instance of <see cref="INamedTypeSymbol"/>.
        /// </summary>
        /// <value>The symbol.</value>
        protected new INamedTypeSymbol Symbol => (INamedTypeSymbol)base.Symbol;
    }
}