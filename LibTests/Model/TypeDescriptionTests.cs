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

        [Fact]
        public void IsAbstractReturnsTrueWhenTypeAbstract()
        {
            var source = @"
            abstract class TestClass
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestClass");

            var value = new TypeDescription(symbol).IsAbstract;

            Assert.True(value);
        }

        [Fact]
        public void IsAbstractReturnsFalseWhenTypeNotAbstract()
        {
            var source = @"
            class TestClass
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestClass");

            var value = new TypeDescription(symbol).IsAbstract;

            Assert.False(value);
        }

        [Fact]
        public void IsSealedReturnsTrueWhenTypeSealed()
        {
            var source = @"
                    sealed class TestClass
                    {
                    }
                    ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestClass");

            var value = new TypeDescription(symbol).IsSealed;

            Assert.True(value);
        }

        [Fact]
        public void IsSealedReturnsFalseWhenTypeNotSealed()
        {
            var source = @"
                    class TestClass
                    {
                    }
                    ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestClass");

            var value = new TypeDescription(symbol).IsSealed;

            Assert.False(value);
        }

        [Fact]
        public void IsGenericReturnsTrueWhenTypeGeneric()
        {
            var source = @"
                            class TestClass<T1, T2>
                            {
                            }
                            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestClass`2");

            var value = new TypeDescription(symbol).IsGeneric;

            Assert.True(value);
        }

        [Fact]
        public void IsGenericReturnsFalseWhenTypeNotGeneric()
        {
            var source = @"
                            class TestClass
                            {
                            }
                            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestClass");

            var value = new TypeDescription(symbol).IsGeneric;

            Assert.False(value);
        }

        [Fact]
        public void IsStaticReturnsTrueWhenTypeIsStatic()
        {
            var source = @"
                                    static class TestClass
                                    {
                                    }
                                    ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestClass");

            var value = new TypeDescription(symbol).IsStatic;

            Assert.True(value);
        }

        [Fact]
        public void IsStaticReturnsFalseWhenTypeIsNotStatic()
        {
            var source = @"
                                    class TestClass
                                    {
                                    }
                                    ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestClass");

            var value = new TypeDescription(symbol).IsStatic;

            Assert.False(value);
        }

        [Fact]
        public void IsRefLikeReturnsTrueWhenTypeIsRefLike()
        {
            var source = @"
            ref struct TestStruct
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestStruct");

            var value = new TypeDescription(symbol).IsRefLike;

            Assert.True(value);
        }

        [Fact]
        public void IsRefLikeReturnsFalseWhenTypeNotRefLike()
        {
            var source = @"
            struct TestStruct
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestStruct");

            var value = new TypeDescription(symbol).IsRefLike;

            Assert.False(value);
        }

        [Fact]
        public void IsReadonlyReturnsTrueIfTypeIsReadonly()
        {
            var source = @"
            readonly struct TestStruct
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestStruct");

            var value = new TypeDescription(symbol).IsReadonly;

            Assert.True(value);
        }

        [Fact]
        public void IsReadonlyReturnsFalseWhenTypeNotReadonly()
        {
            var source = @"
            struct TestStruct
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestStruct");

            var value = new TypeDescription(symbol).IsReadonly;

            Assert.False(value);
        }
    }
}