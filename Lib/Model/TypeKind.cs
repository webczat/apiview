// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

namespace Apiview.Model
{
    /// <summary>
    /// Enumeration describing available kinds of types.
    /// </summary>
    public enum TypeKind
    {
        /// <summary>
        /// The type is a class.
        /// </summary>
        Class,

        /// <summary>
        /// The type is an interface.
        /// </summary>
        Interface,

        /// <summary>
        /// The type is a struct.
        /// </summary>
        Struct,

        /// <summary>
        /// The type is an enum.
        /// </summary>
        Enum,

        /// <summary>
        /// The type is a delegate.
        /// </summary>
        Delegate,

        /// <summary>
        /// The type is missing, so we don't know it's kind.
        /// </summary>
        Missing,
    }
}