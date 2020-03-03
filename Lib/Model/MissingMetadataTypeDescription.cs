// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using Microsoft.CodeAnalysis;

namespace Apiview.Model
{
    /// <summary>
    /// Represents missing metadata types, that is types that cannot be found because of missing assembly references or because of other errors.
    /// </summary>
    /// <remarks>
    /// <para>Note that values of most properties except these defining the type name, kind and containing types/namespaces are undefined for missing types.</para>
    /// <para>Before operating on named metadata types, one should usually check if an instance is not an <see cref="MissingMetadataTypeDescription"/>. This may not be necessary if an API returning <see cref="MetadataTypeDescription"/> instances is guaranteed to return known types.</para>
    /// </remarks>
    public class MissingMetadataTypeDescription : MetadataTypeDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingMetadataTypeDescription"/> class.
        /// </summary>
        /// <param name="symbol">An instance of <see cref="IErrorTypeSymbol"/>.</param>
        internal MissingMetadataTypeDescription(IErrorTypeSymbol symbol)
            : base(symbol)
        {
        }
    }
}
