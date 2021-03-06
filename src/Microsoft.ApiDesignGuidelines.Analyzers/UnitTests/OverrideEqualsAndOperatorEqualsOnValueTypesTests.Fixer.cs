// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.ApiDesignGuidelines.CSharp.Analyzers;
using Microsoft.ApiDesignGuidelines.VisualBasic.Analyzers;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Test.Utilities;
using Xunit;

namespace Microsoft.ApiDesignGuidelines.Analyzers.UnitTests
{
    public class OverrideEqualsAndOperatorEqualsOnValueTypesFixerTests : CodeFixTestBase
    {
        [Fact]
        public void CSharpCodeFixNoEqualsOverrideOrEqualityOperators()
        {
            VerifyCSharpFix(@"
public struct A
{
    public int X;
}
",

@"
public struct A
{
    public int X;

    public override bool Equals(object obj)
    {
        throw new System.NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new System.NotImplementedException();
    }

    public static bool operator ==(A left, A right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(A left, A right)
    {
        return !(left == right);
    }
}
");
        }

        [Fact]
        public void CSharpCodeFixNoEqualsOverride()
        {
            VerifyCSharpFix(@"
public struct A
{
    public static bool operator ==(A left, A right)
    {
        throw new System.NotImplementedException();
    }

    public static bool operator !=(A left, A right)
    {
        throw new System.NotImplementedException();
    }
}
",

@"
public struct A
{
    public static bool operator ==(A left, A right)
    {
        throw new System.NotImplementedException();
    }

    public static bool operator !=(A left, A right)
    {
        throw new System.NotImplementedException();
    }

    public override bool Equals(object obj)
    {
        throw new System.NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new System.NotImplementedException();
    }
}
");
        }

        [Fact]
        public void CSharpCodeFixNoEqualityOperator()
        {
            VerifyCSharpFix(@"
public struct A
{
    public override bool Equals(object obj)
    {
        throw new System.NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new System.NotImplementedException();
    }

    public static bool operator !=(A left, A right)   // error CS0216: The operator requires a matching operator '==' to also be defined
    {
        throw new System.NotImplementedException();
    }
}
",

@"
public struct A
{
    public override bool Equals(object obj)
    {
        throw new System.NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new System.NotImplementedException();
    }

    public static bool operator !=(A left, A right)   // error CS0216: The operator requires a matching operator '==' to also be defined
    {
        throw new System.NotImplementedException();
    }

    public static bool operator ==(A left, A right)
    {
        return left.Equals(right);
    }
}
", validationMode: TestValidationMode.AllowCompileErrors);
        }

        [Fact]
        public void CSharpCodeFixNoInequalityOperator()
        {
            VerifyCSharpFix(@"
public struct A
{
    public override bool Equals(object obj)
    {
        throw new System.NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new System.NotImplementedException();
    }

    public static bool operator ==(A left, A right)   // error CS0216: The operator requires a matching operator '!=' to also be defined
    {
        throw new System.NotImplementedException();
    }
}
",

@"
public struct A
{
    public override bool Equals(object obj)
    {
        throw new System.NotImplementedException();
    }

    public override int GetHashCode()
    {
        throw new System.NotImplementedException();
    }

    public static bool operator ==(A left, A right)   // error CS0216: The operator requires a matching operator '!=' to also be defined
    {
        throw new System.NotImplementedException();
    }

    public static bool operator !=(A left, A right)
    {
        return !(left == right);
    }
}
", validationMode: TestValidationMode.AllowCompileErrors);
        }
        [Fact]
        public void BasicCodeFixNoEqualsOverrideOrEqualityOperators()
        {
            VerifyBasicFix(@"
Public Structure A
    Public X As Integer
End Structure
",

@"
Public Structure A
    Public X As Integer

    Public Overrides Function Equals(obj As Object) As Boolean
        Throw New System.NotImplementedException()
    End Function

    Public Overrides Function GetHashCode() As Integer
        Throw New System.NotImplementedException()
    End Function

    Public Shared Operator =(left As A, right As A) As Boolean
        Return left.Equals(right)
    End Operator

    Public Shared Operator <>(left As A, right As A) As Boolean
        Return Not left = right
    End Operator
End Structure
");
        }

        [Fact]
        public void BasicCodeFixNoEqualsOverride()
        {
            VerifyBasicFix(@"
Public Structure A
    Public Shared Operator =(left As A, right As A) As Boolean
        Throw New System.NotImplementedException()
    End Operator

    Public Shared Operator <>(left As A, right As A) As Boolean
        Throw New System.NotImplementedException()
    End Operator
End Structure
",

@"
Public Structure A
    Public Shared Operator =(left As A, right As A) As Boolean
        Throw New System.NotImplementedException()
    End Operator

    Public Shared Operator <>(left As A, right As A) As Boolean
        Throw New System.NotImplementedException()
    End Operator

    Public Overrides Function Equals(obj As Object) As Boolean
        Throw New System.NotImplementedException()
    End Function

    Public Overrides Function GetHashCode() As Integer
        Throw New System.NotImplementedException()
    End Function
End Structure
");
        }

        [Fact]
        public void BasicCodeFixNoEqualityOperator()
        {
            VerifyBasicFix(@"
Public Structure A
    Public Overrides Function Equals(obj As Object) As Boolean
        Throw New System.NotImplementedException()
    End Function

    Public Overrides Function GetHashCode() As Integer
        Throw New System.NotImplementedException()
    End Function

    Public Shared Operator <>(left As A, right As A) As Boolean   ' error BC33033: Matching '=' operator is required
        Throw New System.NotImplementedException()
    End Operator
End Structure
",

@"
Public Structure A
    Public Overrides Function Equals(obj As Object) As Boolean
        Throw New System.NotImplementedException()
    End Function

    Public Overrides Function GetHashCode() As Integer
        Throw New System.NotImplementedException()
    End Function

    Public Shared Operator <>(left As A, right As A) As Boolean   ' error BC33033: Matching '=' operator is required
        Throw New System.NotImplementedException()
    End Operator

    Public Shared Operator =(left As A, right As A) As Boolean
        Return left.Equals(right)
    End Operator
End Structure
", validationMode: TestValidationMode.AllowCompileErrors);
        }

        [Fact]
        public void BasicCodeFixNoInequalityOperator()
        {
            VerifyBasicFix(@"
Public Structure A
    Public Overrides Function Equals(obj As Object) As Boolean
        Throw New System.NotImplementedException()
    End Function

    Public Overrides Function GetHashCode() As Integer
        Throw New System.NotImplementedException()
    End Function

    Public Shared Operator =(left As A, right As A) As Boolean   ' error BC33033: Matching '<>' operator is required
        Throw New System.NotImplementedException()
    End Operator
End Structure
",

@"
Public Structure A
    Public Overrides Function Equals(obj As Object) As Boolean
        Throw New System.NotImplementedException()
    End Function

    Public Overrides Function GetHashCode() As Integer
        Throw New System.NotImplementedException()
    End Function

    Public Shared Operator =(left As A, right As A) As Boolean   ' error BC33033: Matching '<>' operator is required
        Throw New System.NotImplementedException()
    End Operator

    Public Shared Operator <>(left As A, right As A) As Boolean
        Return Not left = right
    End Operator
End Structure
", validationMode: TestValidationMode.AllowCompileErrors);
        }

        protected override DiagnosticAnalyzer GetBasicDiagnosticAnalyzer()
        {
            return new OverrideEqualsAndOperatorEqualsOnValueTypesAnalyzer();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new OverrideEqualsAndOperatorEqualsOnValueTypesAnalyzer();
        }

        protected override CodeFixProvider GetBasicCodeFixProvider()
        {
            return new BasicOverrideEqualsAndOperatorEqualsOnValueTypesFixer();
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new CSharpOverrideEqualsAndOperatorEqualsOnValueTypesFixer();
        }
    }
}