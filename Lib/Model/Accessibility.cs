// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

namespace Apiview.Model
{
    /// <summary>
    /// This enumeration describes the available accessibilities for code elements.
    /// </summary>
    public enum Accessibility
    {
        /// <summary>
        /// The code element is public.
        /// </summary>
        Public,

        /// <summary>
        /// Code element is protected.
        /// </summary>
        Protected,

        /// <summary>
        /// The code element is internal.
        /// </summary>
        Internal,

        /// <summary>
        /// Code element is protected internal.
        /// </summary>
        ProtectedInternal,

        /// <summary>
        /// The member is private protected.
        /// </summary>
        PrivateProtected,

        /// <summary>
        /// The member is private.
        /// </summary>
        Private,
    }
}