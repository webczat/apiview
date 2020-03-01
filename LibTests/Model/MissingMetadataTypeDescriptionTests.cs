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
        public void NamePropertyReturnsNameWithoutArityForMissingNonGenericTypes()
        {
            var compilation = CreateCompilation(string.Empty);
            var symbol = (IErrorTypeSymbol)compilation.CreateErrorTypeSymbol(compilation.GlobalNamespace, "Test", 0);

            var name = new MissingMetadataTypeDescription(symbol).Name;

            Assert.Equal("Test", name);
        }

        [Fact]
        public void NamePropertyReturnsNameWithArityForMissingGenericTypes()
        {
            var compilation = CreateCompilation(string.Empty);
            var symbol = (IErrorTypeSymbol)compilation.CreateErrorTypeSymbol(compilation.GlobalNamespace, "Test", 1);

            var name = new MissingMetadataTypeDescription(symbol).Name;

            Assert.Equal("Test`1", name);
        }

        [Fact]
        public void AccessibilityPropertyReturnsNullForMissingTypes()
        {
            var compilation = CreateCompilation(string.Empty);
            var symbol = (IErrorTypeSymbol)compilation.CreateErrorTypeSymbol(compilation.GlobalNamespace, "Test", 0);

            var accessibility = new MissingMetadataTypeDescription(symbol).Accessibility;

            Assert.Null(accessibility);
        }

        [Fact]
        public void KindPropertyReturnsMissingForMissingTypes()
        {
            var compilation = CreateCompilation(string.Empty);
            var symbol = (IErrorTypeSymbol)compilation.CreateErrorTypeSymbol(compilation.GlobalNamespace, "Test", 0);

            var kind = new MissingMetadataTypeDescription(symbol).Kind;

            Assert.Equal(Apiview.Model.TypeKind.Missing, kind);
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

            _ = Assert.IsType<MetadataTypeDescription>(parent);
        }

        [Fact]
        public void ParentPropertyReturnsMissingMetadataTypeDescriptionForMissingTypesContainedInOtherMissingTypes()
        {
            var compilation = CreateCompilation(string.Empty);
            var symbol = (IErrorTypeSymbol)compilation.CreateErrorTypeSymbol(compilation.GlobalNamespace, "TestBase", 0);
            symbol = (IErrorTypeSymbol)compilation.CreateErrorTypeSymbol(symbol, "Test", 0);

            var parent = new MissingMetadataTypeDescription(symbol).Parent;

            _ = Assert.IsType<MissingMetadataTypeDescription>(parent);
        }
    }
}