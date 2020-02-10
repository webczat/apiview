// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using Apiview.Model;
using Xunit;

namespace Apiview.Tests.Model
{
    /// <summary>
    /// Tests for a <see cref="TypeDescription"/> class.
    /// </summary>
    public class TypeDescriptionTests : ModelTestBase
    {
        [Fact]
        public void NamePropertyReturnsSimpleTypeNameWhenTypeNotGeneric()
        {
            var source = @"
            namespace TestNamespace
            {
                public class TestType
                {
                }
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestNamespace.TestType");

            var name = new TypeDescription(symbol).Name;

            Assert.Equal("TestType", name);
        }

        [Fact]
        public void NamePropertyReturnsSimpleTypeNameWithNumberOfTypeParametersWhenTypeGeneric()
        {
            var source = @"
            namespace TestNamespace
            {
                public class TestType<T, T2>
                {
                }
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestNamespace.TestType`2");

            var name = new TypeDescription(symbol).Name;

            Assert.Equal("TestType`2", name);
        }
    }
}