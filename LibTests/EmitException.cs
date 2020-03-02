// Copyright (C) Michał Zegan <webczat_200@poczta.onet.pl>.
// All rights reserved.
// This file is licensed under the BSD-2-Clause license, see 'LICENSE' file in source root for more details.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Apiview.Tests
{
    public class EmitException : Exception
    {
        private readonly IEnumerable<Diagnostic> diagnostics;

        public EmitException(string message, IEnumerable<Diagnostic> diagnostics)
            : base(message)
        {
            this.diagnostics = diagnostics;
        }

        public EmitException()
        {
            this.diagnostics = Array.Empty<Diagnostic>();
        }

        public EmitException(string message)
            : base(message)
        {
            this.diagnostics = Array.Empty<Diagnostic>();
        }

        public EmitException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.diagnostics = Array.Empty<Diagnostic>();
        }

        public override string Message
        {
            get
            {
                var b = new StringBuilder();
                var formatter = new DiagnosticFormatter();
                foreach (var d in this.diagnostics)
                {
                    _ = b.Append("\n");
                    _ = b.Append(formatter.Format(d));
                }

                return base.Message + b.ToString();
            }
        }
    }
}