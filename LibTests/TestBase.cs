// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Apiview.Tests
{
    /// <summary>
    /// This class serves as a base for all tests of the model.
    /// </summary>
    public class TestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestBase"/> class.
        /// </summary>
        protected TestBase()
        {
        }

        /// <summary>
        /// Creates a <see cref="Compilation"/> instance based on a given c# source code fragment.
        /// </summary>
        /// <param name="source">Source text to compile.</param>
        /// <returns>The <see cref="Compilation"/> instance.</returns>
        protected static Compilation CreateCompilation(string source)
        {
            var parsedSource = SyntaxFactory.ParseSyntaxTree(source);
            var compilation = CSharpCompilation.Create(source, new SyntaxTree[] { parsedSource });
            return compilation;
        }
    }
}