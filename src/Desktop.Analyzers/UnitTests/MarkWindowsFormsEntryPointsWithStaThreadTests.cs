// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Desktop.CSharp.Analyzers;
using Desktop.VisualBasic.Analyzers;
using Microsoft.CodeAnalysis.Diagnostics;
using Test.Utilities;

namespace Desktop.Analyzers.UnitTests
{
    public class MarkWindowsFormsEntryPointsWithStaThreadTests : DiagnosticAnalyzerTestBase
    {
        protected override DiagnosticAnalyzer GetBasicDiagnosticAnalyzer()
        {
            return new BasicMarkWindowsFormsEntryPointsWithStaThreadAnalyzer();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new CSharpMarkWindowsFormsEntryPointsWithStaThreadAnalyzer();
        }
    }
}