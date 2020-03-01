// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using Apiview.Model;
using Shouldly;
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

            name.ShouldBe("TestType");
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

            name.ShouldBe("TestType`2");
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

            value.ShouldBeTrue();
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

            value.ShouldBeFalse();
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

            value.ShouldBeTrue();
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

            value.ShouldBeFalse();
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

            value.ShouldBeTrue();
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

            value.ShouldBeFalse();
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

            value.ShouldBeTrue();
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

            value.ShouldBeFalse();
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

            value.ShouldBeTrue();
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

            value.ShouldBeFalse();
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

            value.ShouldBeTrue();
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

            value.ShouldBeFalse();
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

            accessibility.ShouldBe(AccessibilityModifier.Public);
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

            accessibility.ShouldBe(AccessibilityModifier.Protected);
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

            accessibility.ShouldBe(AccessibilityModifier.ProtectedInternal);
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

            accessibility.ShouldBe(AccessibilityModifier.PrivateProtected);
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

            accessibility.ShouldBe(AccessibilityModifier.Internal);
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

            accessibility.ShouldBe(AccessibilityModifier.Private);
        }

        [Fact]
        public void KindPropertyReturnsClassWhenTypeIsClass()
        {
            var source = @"
            class TestClass
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestClass");

            var kind = new MetadataTypeDescription(symbol).Kind;

            kind.ShouldBe(TypeKind.Class);
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

            kind.ShouldBe(TypeKind.Interface);
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

            kind.ShouldBe(TypeKind.Struct);
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

            kind.ShouldBe(TypeKind.Enum);
        }

        [Fact]
        public void KindPropertyReturnsDelegateWhenTypeIsDelegate()
        {
            var source = @"
            delegate void TestDelegate();
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("TestDelegate");

            var kind = new MetadataTypeDescription(symbol).Kind;

            kind.ShouldBe(TypeKind.Delegate);
        }

        [Fact]
        public void ParentPropertyReturnsMetadataTypeDescriptionWhenParentIsType()
        {
            var source = @"
            class Test
            {
                class Test2
                {
                }
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("Test+Test2");

            var parent = new MetadataTypeDescription(symbol).Parent;

            _ = parent.ShouldBeOfType<MetadataTypeDescription>();
        }

        [Fact]
        public void BaseTypePropertyReturnsNullForSystemObject()
        {
            // Can we even fake System.Object? let's assume we can.
            var source = @"
            namespace System
            {
                public class Object
                {
                }
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("System.Object");

            var baseType = new MetadataTypeDescription(symbol).BaseType;

            baseType.ShouldBeNull();
        }

        [Fact]
        public void BaseTypePropertyReturnsMetadataTypeDescriptionWhenBaseNotMissing()
        {
            var source = @"
            public class TestBase
            {
            }

            public class Test : TestBase
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("Test");

            var baseType = new MetadataTypeDescription(symbol).BaseType;

            _ = baseType.ShouldBeOfType<MetadataTypeDescription>();
        }

        [Fact]
        public void BaseTypePropertyReturnsMissingMetadataTypeDescriptionWhenBaseMissing()
        {
            var source = @"
            public class Test
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("Test");

            var baseType = new MetadataTypeDescription(symbol).BaseType;

            _ = baseType.ShouldBeOfType<MissingMetadataTypeDescription>();
        }

        [Fact]
        public void BaseTypePropertyReturnsBaseTypeWithCorrectName()
        {
            var source = @"
            public class TestBase
            {
            }

            public class Test : TestBase
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("Test");

            var baseTypeName = new MetadataTypeDescription(symbol).BaseType!.Name;

            baseTypeName.ShouldBe("TestBase");
        }

        [Fact]
        public void InterfacesPropertyReturnsEmptyArrayWhenNoInterfacesImplemented()
        {
            var source = @"
            public class Test
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("Test");

            var interfaces = new MetadataTypeDescription(symbol).Interfaces;

            interfaces.ShouldBeEmpty();
        }

        [Fact]
        public void InterfacesPropertyReturnsArrayWithOneElementWhenTypeImplementsOneInterface()
        {
            var source = @"
            public interface TestInterface
            {
            }

            public class Test : TestInterface
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("Test");

            var interfaces = new MetadataTypeDescription(symbol).Interfaces;

            interfaces.Length.ShouldBe(1);
        }

        [Fact]
        public void InterfacesPropertyReturnsArrayWithTwoElementsWhenTypeImplementsTwoInterfaces()
        {
            var source = @"
            public interface TestInterface1
            {
            }

            public interface TestInterface2
            {
            }

            public class Test : TestInterface1, TestInterface2
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("Test");

            var interfaces = new MetadataTypeDescription(symbol).Interfaces;

            interfaces.Length.ShouldBe(2);
        }

        [Fact]
        public void InterfacesPropertyReturnsCorrectInterfaceName()
        {
            var source = @"
            public interface TestInterface
            {
            }

            public class Test : TestInterface
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("Test");

            var interfaces = new MetadataTypeDescription(symbol).Interfaces;

            interfaces[0].Name.ShouldBe("TestInterface");
        }

        [Fact]
        public void InterfacesPropertyReturnsMetadataTypeDescriptionWhenInterfaceNotMissing()
        {
            var source = @"
            public interface TestInterface
            {
            }

            public class Test : TestInterface
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("Test");

            var interfaces = new MetadataTypeDescription(symbol).Interfaces;

            _ = interfaces[0].ShouldBeOfType<MetadataTypeDescription>();
        }

        [Fact]
        public void InterfacePropertyReturnsMissingMetadataTypeDescriptionWhenInterfaceMissing()
        {
            // Make a class with explicit base class and one interface, that makes things less ambiguous and will result in an error type in place of interface.
            var source = @"
            public class TestBase
            {
            }

            public class Test : TestBase, TestInterface
            {
            }
            ";
            var symbol = CreateCompilation(source).GetTypeByMetadataName("Test");

            var interfaces = new MetadataTypeDescription(symbol).Interfaces;

            _ = interfaces[0].ShouldBeOfType<MissingMetadataTypeDescription>();
        }
    }
}