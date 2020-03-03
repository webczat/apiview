// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using System.Threading.Tasks;
using Apiview.Model;
using Shouldly;
using Xunit;

namespace Apiview.Tests.Model
{
    /// <summary>
    /// Tests for a <see cref="MetadataTypeDescription"/> class.
    /// </summary>
    public class MetadataTypeDescriptionTests : TestBase
    {
        [Fact]
        public async Task NamePropertyReturnsSimpleTypeNameWhenTypeNotGeneric()
        {
            var source = @"
            namespace TestNamespace
            {
                public class TestType
                {
                }
            }
        ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestNamespace.TestType");

            var name = type.Name;

            name.ShouldBe("TestType");
        }

        [Fact]
        public async Task NamePropertyReturnsSimpleTypeNameWithNumberOfTypeParametersWhenTypeGeneric()
        {
            var source = @"
            namespace TestNamespace
            {
                public class TestType<T, T2>
                {
                }
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestNamespace.TestType`2");

            var name = type.Name;

            name.ShouldBe("TestType`2");
        }

        [Fact]
        public async Task IsAbstractReturnsTrueWhenTypeAbstract()
        {
            var source = @"
            abstract class TestClass
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestClass");

            var value = type.IsAbstract;

            value.ShouldBeTrue();
        }

        [Fact]
        public async Task IsAbstractReturnsFalseWhenTypeNotAbstract()
        {
            var source = @"
            class TestClass
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestClass");

            var value = type.IsAbstract;

            value.ShouldBeFalse();
        }

        [Fact]
        public async Task IsSealedReturnsTrueWhenTypeSealed()
        {
            var source = @"
                    sealed class TestClass
                    {
                    }
                    ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestClass");

            var value = type.IsSealed;

            value.ShouldBeTrue();
        }

        [Fact]
        public async Task IsSealedReturnsFalseWhenTypeNotSealed()
        {
            var source = @"
                    class TestClass
                    {
                    }
                    ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestClass");

            var value = type.IsSealed;

            value.ShouldBeFalse();
        }

        [Fact]
        public async Task IsGenericReturnsTrueWhenTypeGeneric()
        {
            var source = @"
                            class TestClass<T1, T2>
                            {
                            }
                            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestClass`2");

            var value = type.IsGeneric;

            value.ShouldBeTrue();
        }

        [Fact]
        public async Task IsGenericReturnsFalseWhenTypeNotGeneric()
        {
            var source = @"
                            class TestClass
                            {
                            }
                            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestClass");

            var value = type.IsGeneric;

            value.ShouldBeFalse();
        }

        [Fact]
        public async Task IsStaticReturnsTrueWhenTypeIsStatic()
        {
            var source = @"
                                    static class TestClass
                                    {
                                    }
                                    ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestClass");

            var value = type.IsStatic;

            value.ShouldBeTrue();
        }

        [Fact]
        public async Task IsStaticReturnsFalseWhenTypeIsNotStatic()
        {
            var source = @"
                                    class TestClass
                                    {
                                    }
                                    ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestClass");

            var value = type.IsStatic;

            value.ShouldBeFalse();
        }

        [Fact]
        public async Task IsRefLikeReturnsTrueWhenTypeIsRefLike()
        {
            var source = @"
            ref struct TestStruct
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestStruct");

            var value = type.IsRefLike;

            value.ShouldBeTrue();
        }

        [Fact]
        public async Task IsRefLikeReturnsFalseWhenTypeNotRefLike()
        {
            var source = @"
            struct TestStruct
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestStruct");

            var value = type.IsRefLike;

            value.ShouldBeFalse();
        }

        [Fact]
        public async Task IsReadonlyReturnsTrueIfTypeIsReadonly()
        {
            var source = @"
            readonly struct TestStruct
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestStruct");

            var value = type.IsReadonly;

            value.ShouldBeTrue();
        }

        [Fact]
        public async Task IsReadonlyReturnsFalseWhenTypeNotReadonly()
        {
            var source = @"
            struct TestStruct
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestStruct");

            var value = type.IsReadonly;

            value.ShouldBeFalse();
        }

        [Fact]
        public async Task AccessibilityPropertyReturnsPublicWhenTypeIsPublic()
        {
            var source = @"
            class TestParent
            {
                public class TestClass
                {
                }
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestParent+TestClass");

            var accessibility = type.Accessibility;

            accessibility.ShouldBe(AccessibilityModifier.Public);
        }

        [Fact]
        public async Task AccessibilityPropertyReturnsProtectedWhenTypeIsProtected()
        {
            var source = @"
            class TestContainer
            {
                protected class TestClass
                {
                }
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestContainer+TestClass");

            var accessibility = type.Accessibility;

            accessibility.ShouldBe(AccessibilityModifier.Protected);
        }

        [Fact]
        public async Task AccessibilityPropertyReturnsProtectedInternalWhenTypeIsProtectedInternal()
        {
            var source = @"
            class TestContainer
            {
                protected internal class TestClass
                {
                }
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestContainer+TestClass");

            var accessibility = type.Accessibility;

            accessibility.ShouldBe(AccessibilityModifier.ProtectedInternal);
        }

        [Fact]
        public async Task AccessibilityPropertyReturnsPrivateProtectedWhenTypeIsPrivateProtected()
        {
            var source = @"
            class TestParent
            {
                private protected class TestClass
                {
                }
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestParent+TestClass");

            var accessibility = type.Accessibility;

            accessibility.ShouldBe(AccessibilityModifier.PrivateProtected);
        }

        [Fact]
        public async Task AccessibilityPropertyReturnsInternalWhenTypeIsInternal()
        {
            var source = @"
            class TestParent
            {
                internal class TestClass
                {
        }
    }
    ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestParent+TestClass");

            var accessibility = type.Accessibility;

            accessibility.ShouldBe(AccessibilityModifier.Internal);
        }

        [Fact]
        public async Task AccessibilityPropertyReturnsPrivateWhenTypeIsPrivate()
        {
            var source = @"
            class TestContainer
            {
                private class TestClass
                {
                }
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestContainer+TestClass");

            var accessibility = type.Accessibility;

            accessibility.ShouldBe(AccessibilityModifier.Private);
        }

        [Fact]
        public async Task KindPropertyReturnsClassWhenTypeIsClass()
        {
            var source = @"
            class TestClass
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestClass");

            var kind = type.Kind;

            kind.ShouldBe(TypeKind.Class);
        }

        [Fact]
        public async Task KindPropertyReturnsInterfaceWhenTypeIsInterface()
        {
            var source = @"
            interface TestInterface
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestInterface");

            var kind = type.Kind;

            kind.ShouldBe(TypeKind.Interface);
        }

        [Fact]
        public async Task KindPropertyReturnsStructWhenTypeIsStruct()
        {
            var source = @"
            struct TestStruct
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestStruct");

            var kind = type.Kind;

            kind.ShouldBe(TypeKind.Struct);
        }

        [Fact]
        public async Task KindPropertyReturnsEnumWhenTypeIsEnum()
        {
            var source = @"
            enum TestEnum
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestEnum");

            var kind = type.Kind;

            kind.ShouldBe(TypeKind.Enum);
        }

        [Fact]
        public async Task KindPropertyReturnsDelegateWhenTypeIsDelegate()
        {
            var source = @"
            delegate void TestDelegate();
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "TestDelegate");

            var kind = type.Kind;

            kind.ShouldBe(TypeKind.Delegate);
        }

        [Fact]
        public async Task ParentPropertyReturnsMetadataTypeDescriptionWhenParentIsType()
        {
            var source = @"
            class Test
            {
                class Test2
                {
                }
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "Test+Test2");

            var parent = type.Parent;

            _ = parent.ShouldBeOfType<MetadataTypeDescription>();
        }

        [Fact]
        public async Task BaseTypePropertyReturnsNullForSystemObject()
        {
            var type = await RetrieveTypeFromSourceFragmentAsync(null, "System.Object");

            var baseType = type.BaseType;

            baseType.ShouldBeNull();
        }

        [Fact]
        public async Task BaseTypePropertyReturnsMetadataTypeDescriptionWhenBaseNotMissing()
        {
            var source = @"
            public class TestBase
            {
            }

            public class Test : TestBase
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "Test");

            var baseType = type.BaseType;

            _ = baseType.ShouldBeOfType<MetadataTypeDescription>();
        }

        [Fact]
        public async Task BaseTypePropertyReturnsMissingMetadataTypeDescriptionWhenBaseMissing()
        {
            var source = @"
            public class Test : MissingClass
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "Test", MissingTypesAssembly);

            var baseType = type.BaseType;

            _ = baseType.ShouldBeOfType<MissingMetadataTypeDescription>();
        }

        [Fact]
        public async Task BaseTypePropertyReturnsBaseTypeWithCorrectName()
        {
            var source = @"
            public class TestBase
            {
            }

            public class Test : TestBase
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "Test");

            var baseTypeName = type.BaseType!.Name;

            baseTypeName.ShouldBe("TestBase");
        }

        [Fact]
        public async Task InterfacesPropertyReturnsEmptyArrayWhenNoInterfacesImplemented()
        {
            var source = @"
            public class Test
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "Test");

            var interfaces = type.Interfaces;

            interfaces.ShouldBeEmpty();
        }

        [Fact]
        public async Task InterfacesPropertyReturnsArrayWithOneElementWhenTypeImplementsOneInterface()
        {
            var source = @"
            public interface TestInterface
            {
            }

            public class Test : TestInterface
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "Test");

            var interfaces = type.Interfaces;

            interfaces.Length.ShouldBe(1);
        }

        [Fact]
        public async Task InterfacesPropertyReturnsArrayWithMultipleElementsWhenTypeImplementsMultipleInterfaces()
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
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "Test");

            var interfaces = type.Interfaces;

            interfaces.Length.ShouldBe(2);
        }

        [Fact]
        public async Task InterfacesPropertyReturnsCorrectInterfaceName()
        {
            var source = @"
            public interface TestInterface
            {
            }

            public class Test : TestInterface
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "Test");

            var interfaces = type.Interfaces;

            interfaces[0].Name.ShouldBe("TestInterface");
        }

        [Fact]
        public async Task InterfacesPropertyReturnsMetadataTypeDescriptionWhenInterfaceNotMissing()
        {
            var source = @"
            public interface TestInterface
            {
            }

            public class Test : TestInterface
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "Test");

            var interfaces = type.Interfaces;

            _ = interfaces[0].ShouldBeOfType<MetadataTypeDescription>();
        }

        [Fact]
        public async Task InterfacePropertyReturnsMissingMetadataTypeDescriptionWhenInterfaceMissing()
        {
            var source = @"
            public class Test : MissingInterface
            {
            }
            ";
            var type = await RetrieveTypeFromSourceFragmentAsync(source, "Test", MissingTypesAssembly);

            var interfaces = type.Interfaces;

            _ = interfaces[0].ShouldBeOfType<MissingMetadataTypeDescription>();
        }
    }
}