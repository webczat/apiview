// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using Apiview.Model;
using Xunit;

namespace Apiview.Tests.Model
{
    /// <summary>
    /// Tests for a <see cref="MetadataTypeDescription"/> class.
    /// </summary>
    public class MetadataTypeDescriptionTests : ModelTestBase
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

            var name = new MetadataTypeDescription(symbol).Name;

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

            var name = new MetadataTypeDescription(symbol).Name;

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

            var value = new MetadataTypeDescription(symbol).IsAbstract;

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

            var value = new MetadataTypeDescription(symbol).IsAbstract;

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

            var value = new MetadataTypeDescription(symbol).IsSealed;

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

            var value = new MetadataTypeDescription(symbol).IsSealed;

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

            var value = new MetadataTypeDescription(symbol).IsGeneric;

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

            var value = new MetadataTypeDescription(symbol).IsGeneric;

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

            var value = new MetadataTypeDescription(symbol).IsStatic;

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

            var value = new MetadataTypeDescription(symbol).IsStatic;

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

            var value = new MetadataTypeDescription(symbol).IsRefLike;

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

            var value = new MetadataTypeDescription(symbol).IsRefLike;

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

            var value = new MetadataTypeDescription(symbol).IsReadonly;

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

            var value = new MetadataTypeDescription(symbol).IsReadonly;

            Assert.False(value);
        }

        [Fact]
        public void AccessibilityPropertyReturnsPublicWhenTypeIsPublic()
        {
            var source = @"
            class TestParent
            {
                public class TestClass
                {
                }
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestParent+TestClass");

            var accessibility = new MetadataTypeDescription(symbol).Accessibility;

            Assert.Equal(Accessibility.Public, accessibility);
        }

        [Fact]
        public void AccessibilityPropertyReturnsProtectedWhenTypeIsProtected()
        {
            var source = @"
            class TestContainer
            {
                protected class TestClass
                {
                }
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestContainer+TestClass");

            var accessibility = new MetadataTypeDescription(symbol).Accessibility;

            Assert.Equal(Accessibility.Protected, accessibility);
        }

        [Fact]
        public void AccessibilityPropertyReturnsProtectedInternalWhenTypeIsProtectedInternal()
        {
            var source = @"
            class TestContainer
            {
                protected internal class TestClass
                {
                }
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestContainer+TestClass");

            var accessibility = new MetadataTypeDescription(symbol).Accessibility;

            Assert.Equal(Accessibility.ProtectedInternal, accessibility);
        }

        [Fact]
        public void AccessibilityPropertyReturnsPrivateProtectedWhenTypeIsPrivateProtected()
        {
            var source = @"
            class TestParent
            {
                private protected class TestClass
                {
                }
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestParent+TestClass");

            var accessibility = new MetadataTypeDescription(symbol).Accessibility;

            Assert.Equal(Accessibility.PrivateProtected, accessibility);
        }

        [Fact]
        public void AccessibilityPropertyReturnsInternalWhenTypeIsInternal()
        {
            var source = @"
            class TestParent
            {
                internal class TestClass
                {
        }
    }
    ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestParent+TestClass");

            var accessibility = new MetadataTypeDescription(symbol).Accessibility;

            Assert.Equal(Accessibility.Internal, accessibility);
        }

        [Fact]
        public void AccessibilityPropertyReturnsPrivateWhenTypeIsPrivate()
        {
            var source = @"
            class TestContainer
            {
                private class TestClass
                {
                }
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestContainer+TestClass");

            var accessibility = new MetadataTypeDescription(symbol).Accessibility;

            Assert.Equal(Accessibility.Private, accessibility);
        }

        [Fact]
        public void KindReturnsClassWhenTypeIsClass()
        {
            var source = @"
            class TestClass
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestClass");

            var kind = new MetadataTypeDescription(symbol).Kind;

            Assert.Equal(TypeKind.Class, kind);
        }

        [Fact]
        public void KindPropertyReturnsInterfaceWhenTypeIsInterface()
        {
            var source = @"
            interface TestInterface
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestInterface");

            var kind = new MetadataTypeDescription(symbol).Kind;

            Assert.Equal(TypeKind.Interface, kind);
        }

        [Fact]
        public void KindPropertyReturnsStructWhenTypeIsStruct()
        {
            var source = @"
            struct TestStruct
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestStruct");

            var kind = new MetadataTypeDescription(symbol).Kind;

            Assert.Equal(TypeKind.Struct, kind);
        }

        [Fact]
        public void KindPropertyReturnsEnumWhenTypeIsEnum()
        {
            var source = @"
            enum TestEnum
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestEnum");

            var kind = new MetadataTypeDescription(symbol).Kind;

            Assert.Equal(TypeKind.Enum, kind);
        }

        [Fact]
        public void KindPropertyReturnsDelegateWhenTypeIsDelegate()
        {
            var source = @"
            delegate void TestDelegate();
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestDelegate");

            var kind = new MetadataTypeDescription(symbol).Kind;

            Assert.Equal(TypeKind.Delegate, kind);
        }
    }
}