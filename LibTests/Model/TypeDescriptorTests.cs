// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using Apiview.Model;
using Xunit;

namespace Apiview.Tests.Model
{
    /// <summary>
    /// Tests for a <see cref="TypeDescriptor"/> class.
    /// </summary>
    public class TypeDescriptorTests : ModelTestBase
    {
        [Fact]
        public void NamePropertyReturnsTypeNameWithoutNamespaceWhenTypeNotGeneric()
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

            var name = new TypeDescriptor(symbol).Name;

            Assert.Equal("TestType", name);
        }
    }
}