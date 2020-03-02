// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Apiview.Tests
{
    /// <summary>
    /// Tests for >see cref="Documentation"/> class.
    /// </summary>
    public class DocumentationTests : TestBase
    {
        [Fact]
        public async Task CreatingDocumentationWithoutAssembliesFails()
        {
            _ = await Should.ThrowAsync<InvalidOperationException>(() => Documentation.CreateBuilder().BuildAsync());
        }

        [Fact]
        public async Task GetMetadataTypeReturnsNullWhenTypeNotFound()
        {
            var doc = await RetrieveDocumentationAsync();

            var type = doc.GetMetadataType("Test");

            type.ShouldBeNull();
        }
    }
}