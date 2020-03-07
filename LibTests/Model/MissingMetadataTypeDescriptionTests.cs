// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using System.Threading.Tasks;
using Apiview.Model;
using Microsoft.CodeAnalysis;
using Shouldly;
using Xunit;

namespace Apiview.Tests.Model
{
    /// <summary>
    /// Tests for <see cref="MissingMetadataTypeDescription"/> class.
    /// </summary>
    public class MissingMetadataTypeDescriptionTests : TestBase
    {
        [Fact]
        public async Task NamePropertyReturnsNameWithoutArityForMissingNonGenericTypes()
        {
            var type = await this.GetMissingTypeAsync("MissingClass");

            var name = type.Name;

            name.ShouldBe("MissingClass");
        }

        [Fact]
        public async Task NamePropertyReturnsNameWithArityForMissingGenericTypes()
        {
            var type = await this.GetMissingTypeAsync("MissingClass<int>");

            var name = type.Name;

            name.ShouldBe("MissingClass`1");
        }

        [Fact]
        public async Task AccessibilityPropertyReturnsNullForMissingTypes()
        {
            var type = await this.GetMissingTypeAsync("MissingClass");

            var accessibility = type.Accessibility;

            accessibility.ShouldBeNull();
        }

        [Fact]
        public async Task KindPropertyReturnsMissingForMissingTypes()
        {
            var type = await this.GetMissingTypeAsync("MissingClass");

            var kind = type.Kind;

            kind.ShouldBe(Apiview.Model.TypeKind.Missing);
        }

        [Fact]
        public async Task ParentPropertyReturnsCorrectParentForMissingTypesContainedInOtherMissingTypes()
        {
            var type = await this.GetMissingTypeAsync("MissingClass.MissingNestedClass");
            var expectedParent = await this.GetMissingTypeAsync("MissingClass");

            var actualParent = type.Parent;

            actualParent.ShouldBeEquivalentTo(expectedParent);
        }

        [Fact]
        public async Task ParentPropertyReturnsMissingMetadataTypeDescriptionForMissingTypesContainedInOtherMissingTypes()
        {
            var type = await this.GetMissingTypeAsync("MissingClass.MissingNestedClass");

            var parent = type.Parent;

            _ = parent.ShouldBeOfType<MissingMetadataTypeDescription>();
        }

        /// <summary>
        /// This method retrieves the missing type. As it is not possible in usual ways because they do not exist, it is done with a trick. We make some class inheriting from the type from the missing assembly.
        /// When we load documentation with the new assembly, the MissingTypes assembly is not referenced anymore, and the new type's base becomes missing. We could not get it directly because it would not be found.
        /// Note that the type name is given as c# language type name, not metadata, because this is input for source generation.
        /// </summary>
        private async Task<MetadataTypeDescription> GetMissingTypeAsync(string typeName)
        {
            var source = $@"
            public class Test : {typeName}
            {{
            }}
            ";
            return (await RetrieveTypeFromSourceFragmentAsync(source, "Test", MissingTypesAssembly)).BaseType!;
        }
    }
}