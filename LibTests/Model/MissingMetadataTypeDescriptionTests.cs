// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

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
        public void NamePropertyReturnsNameWithoutArityForMissingNonGenericTypes()
        {
            var compilation = CreateCompilation(string.Empty);
            var symbol = (IErrorTypeSymbol)compilation.CreateErrorTypeSymbol(compilation.GlobalNamespace, "Test", 0);

            var name = new MissingMetadataTypeDescription(symbol).Name;

            name.ShouldBe("Test");
        }

        [Fact]
        public void NamePropertyReturnsNameWithArityForMissingGenericTypes()
        {
            var compilation = CreateCompilation(string.Empty);
            var symbol = (IErrorTypeSymbol)compilation.CreateErrorTypeSymbol(compilation.GlobalNamespace, "Test", 1);

            var name = new MissingMetadataTypeDescription(symbol).Name;

            name.ShouldBe("Test`1");
        }

        [Fact]
        public void AccessibilityPropertyReturnsNullForMissingTypes()
        {
            var compilation = CreateCompilation(string.Empty);
            var symbol = (IErrorTypeSymbol)compilation.CreateErrorTypeSymbol(compilation.GlobalNamespace, "Test", 0);

            var accessibility = new MissingMetadataTypeDescription(symbol).Accessibility;

            accessibility.ShouldBeNull();
        }

        [Fact]
        public void KindPropertyReturnsMissingForMissingTypes()
        {
            var compilation = CreateCompilation(string.Empty);
            var symbol = (IErrorTypeSymbol)compilation.CreateErrorTypeSymbol(compilation.GlobalNamespace, "Test", 0);

            var kind = new MissingMetadataTypeDescription(symbol).Kind;

            kind.ShouldBe(Apiview.Model.TypeKind.Missing);
        }

        [Fact]
        public void ParentPropertyReturnsMetadataTypeDescriptionForMissingTypesContainedInOtherNonMissingTypes()
        {
            var source = @"
            public class TestContainer
            {
            }
            ";

            // To create a missing type with Test type as parent, we have to use CreateErrorTypeSymbol method, to wrap it.
            var compilation = CreateCompilation(source);
            var baseSymbol = compilation.GetTypeByMetadataName("TestContainer");
            var symbol = (IErrorTypeSymbol)compilation.CreateErrorTypeSymbol(baseSymbol, "Test", 0);

            var parent = new MissingMetadataTypeDescription(symbol).Parent;

            _ = parent.ShouldBeOfType<MetadataTypeDescription>();
        }

        [Fact]
        public void ParentPropertyReturnsMissingMetadataTypeDescriptionForMissingTypesContainedInOtherMissingTypes()
        {
            var compilation = CreateCompilation(string.Empty);
            var symbol = (IErrorTypeSymbol)compilation.CreateErrorTypeSymbol(compilation.GlobalNamespace, "TestBase", 0);
            symbol = (IErrorTypeSymbol)compilation.CreateErrorTypeSymbol(symbol, "Test", 0);

            var parent = new MissingMetadataTypeDescription(symbol).Parent;

            _ = parent.ShouldBeOfType<MissingMetadataTypeDescription>();
        }
    }
}