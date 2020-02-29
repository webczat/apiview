// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using Apiview.Model;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Apiview.Tests.Model
{
    /// <summary>
    /// Tests for <see cref="MissingMetadataTypeDescription"/> class.
    /// </summary>
    public class MissingMetadataTypeDescriptionTests : ModelTestBase
    {
        [Fact]
        public void NamePropertyReturnsNameForMissingTypes()
        {
            var source = @"
            public class X
            {
            }
            ";
            var symbol = (IErrorTypeSymbol)(CreateCompilation(source).GetTypeByMetadataName("X").BaseType!);

            var name = new MissingMetadataTypeDescription(symbol).Name;

            Assert.Equal("Object", name);
        }

        [Fact]
        public void AccessibilityPropertyReturnsUnknownForMissingTypes()
        {
            var source = @"
            public class X
            {
            }
            ";
            var symbol = (IErrorTypeSymbol)(CreateCompilation(source).GetTypeByMetadataName("X").BaseType!);

            var accessibility = new MissingMetadataTypeDescription(symbol).Accessibility;

            Assert.Equal(Apiview.Model.Accessibility.Unknown, accessibility);
        }
    }
}