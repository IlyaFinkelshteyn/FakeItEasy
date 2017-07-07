﻿namespace FakeItEasy.Analyzer.CSharp.Tests
{
    using System.Collections.Generic;
    using FakeItEasy.Analyzer.Tests.Helpers;
    using FakeItEasy.Tests;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Xunit;

    public class ArgumentConstraintTypeMismatchAnalyzerTests : CodeFixVerifier
    {
        private const string CodeTemplate = @"using FakeItEasy;
namespace TheNamespace
{{
    class TheClass
    {{
        void TheTest()
        {{
            var foo = A.Fake<IFoo>();
            A.CallTo(() => {0}).Returns(42);
        }}
    }}

    interface IFoo
    {{
        int NullableIntParam(int? x);
        int NullableLongParam(long? x);
        int NonNullableIntParam(int x);
        int NonNullableDoubleParam(double x);
    }}
}}";

        public static IEnumerable<object[]> SupportedConstraints =>
            TestCases.FromObject(
                "_",
                "Ignored",
                "That.Matches(_ => true)");

        [Theory]
        [MemberData(nameof(SupportedConstraints))]
        public void Diagnostic_should_be_triggered_when_wrong_typed_nonnullable_constraint_is_used_with_nonnullable_parameter(string constraint)
        {
            string completeConstraint = $"A<int>.{constraint}";
            string call = $"foo.NonNullableDoubleParam({completeConstraint})";
            string code = string.Format(CodeTemplate, call);

            this.VerifyCSharpDiagnostic(
                code,
                new DiagnosticResult
                {
                    Id = "FakeItEasy0005",
                    Message = $"Parameter 'x' of method or indexer 'NonNullableDoubleParam' has type double, but argument constraint '{completeConstraint}' has type int. No calls can match this constraint.",
                    Severity = DiagnosticSeverity.Error,
                    Locations = new[] { new DiagnosticResultLocation("Test0.cs", 9, 55) }
                });
        }

        //[Theory]
        //[MemberData(nameof(SupportedConstraints))]
        //public void Diagnostic_should_be_triggered_when_non_nullable_constraint_is_used_for_nullable_parameter_for_indexer(string constraint)
        //{
        //    string completeConstraint = $"A<int>.{constraint}";
        //    string call = $"foo[{completeConstraint}]";
        //    string code = string.Format(CodeTemplate, call);

        //    this.VerifyCSharpDiagnostic(
        //        code,
        //        new DiagnosticResult
        //        {
        //            Id = "FakeItEasy0005",
        //            Message = $"Parameter 'x' of method or indexer 'this[]' is nullable, but argument constraint '{completeConstraint}' is not. Calls where this argument is null will not be matched.",
        //            Severity = DiagnosticSeverity.Warning,
        //            Locations = new[] { new DiagnosticResultLocation("Test0.cs", 9, 32) }
        //        });
        //}

        //[Fact]
        //public void Diagnostic_should_not_be_triggered_for_That_constraint()
        //{
        //    string completeConstraint = "A<int>.That.Matches(x => true)";
        //    string call = $"foo.NullableParam({completeConstraint})";
        //    string code = string.Format(CodeTemplate, call);

        //    this.VerifyCSharpDiagnostic(code);
        //}

        //[Theory]
        //[MemberData(nameof(SupportedConstraints))]
        //public void Diagnostic_should_not_be_triggered_when_nullable_constraint_is_used_for_nullable_parameter(string constraint)
        //{
        //    string completeConstraint = $"A<int?>.{constraint}";
        //    string call = $"foo.NullableParam({completeConstraint})";
        //    string code = string.Format(CodeTemplate, call);

        //    this.VerifyCSharpDiagnostic(code);
        //}

        //[Theory]
        //[MemberData(nameof(SupportedConstraints))]
        //public void Diagnostic_should_not_be_triggered_when_non_nullable_constraint_is_used_for_non_nullable_parameter(string constraint)
        //{
        //    string completeConstraint = $"A<int>.{constraint}";
        //    string call = $"foo.NonNullableParam({completeConstraint})";
        //    string code = string.Format(CodeTemplate, call);

        //    this.VerifyCSharpDiagnostic(code);
        //}

        //[Theory]
        //[MemberData(nameof(SupportedConstraints))]
        //public void Diagnostic_should_not_be_triggered_when_non_nullable_constraint_is_used_for_other_nullable_parameter(string constraint)
        //{
        //    string completeConstraint = $"A<int>.{constraint}";
        //    string call = $"foo.OtherNullableParam({completeConstraint})";
        //    string code = string.Format(CodeTemplate, call);

        //    this.VerifyCSharpDiagnostic(code);
        //}

        //[Theory]
        //[MemberData(nameof(SupportedConstraints))]
        //public void Diagnostic_should_not_be_triggered_when_nullable_constraint_is_used_for_other_nullable_parameter(string constraint)
        //{
        //    string completeConstraint = $"A<int?>.{constraint}";
        //    string call = $"foo.OtherNullableParam({completeConstraint})";
        //    string code = string.Format(CodeTemplate, call);

        //    this.VerifyCSharpDiagnostic(code);
        //}

        //[Theory]
        //[MemberData(nameof(SupportedConstraints))]
        //public void Diagnostic_should_not_be_triggered_when_non_nullable_constraint_is_used_for_other_non_nullable_parameter(string constraint)
        //{
        //    string completeConstraint = $"A<int>.{constraint}";
        //    string call = $"foo.OtherNonNullableParam({completeConstraint})";
        //    string code = string.Format(CodeTemplate, call);

        //    this.VerifyCSharpDiagnostic(code);
        //}

        //[Theory]
        //[MemberData(nameof(SupportedConstraints))]
        //public void MakeConstraintNullable_CodeFix_should_replace_constraint_with_nullable_constraint(string constraint)
        //{
        //    string completeConstraint = $"A<int>.{constraint}";
        //    string call = $"foo.NullableParam({completeConstraint})";
        //    string code = string.Format(CodeTemplate, call);

        //    string fixedConstraint = $"A<int?>.{constraint}";
        //    string fixedCall = $"foo.NullableParam({fixedConstraint})";
        //    string fixedCode = string.Format(CodeTemplate, fixedCall);
        //    this.VerifyCSharpFix(code, fixedCode, codeFixIndex: 0);
        //}

        //[Theory]
        //[MemberData(nameof(SupportedConstraints))]
        //public void MakeConstraintNullable_CodeFix_should_replace_constraint_with_nullable_constraint_for_indexer(string constraint)
        //{
        //    string completeConstraint = $"A<int>.{constraint}";
        //    string call = $"foo[{completeConstraint}]";
        //    string code = string.Format(CodeTemplate, call);

        //    string fixedConstraint = $"A<int?>.{constraint}";
        //    string fixedCall = $"foo[{fixedConstraint}]";
        //    string fixedCode = string.Format(CodeTemplate, fixedCall);
        //    this.VerifyCSharpFix(code, fixedCode, codeFixIndex: 0);
        //}

        //[Theory]
        //[MemberData(nameof(SupportedConstraints))]
        //public void MakeNotNullConstraint_CodeFix_should_replace_constraint_with_AThatIsNotNull(string constraint)
        //{
        //    string completeConstraint = $"A<int>.{constraint}";
        //    string call = $"foo.NullableParam({completeConstraint})";
        //    string code = string.Format(CodeTemplate, call);

        //    string fixedConstraint = "A<int?>.That.IsNotNull()";
        //    string fixedCall = $"foo.NullableParam({fixedConstraint})";
        //    string fixedCode = string.Format(CodeTemplate, fixedCall);
        //    this.VerifyCSharpFix(code, fixedCode, codeFixIndex: 1);
        //}

        //[Theory]
        //[MemberData(nameof(SupportedConstraints))]
        //public void MakeNotNullConstraint_CodeFix_should_replace_constraint_with_AThatIsNotNull_for_indexer(string constraint)
        //{
        //    string completeConstraint = $"A<int>.{constraint}";
        //    string call = $"foo[{completeConstraint}]";
        //    string code = string.Format(CodeTemplate, call);

        //    string fixedConstraint = "A<int?>.That.IsNotNull()";
        //    string fixedCall = $"foo[{fixedConstraint}]";
        //    string fixedCode = string.Format(CodeTemplate, fixedCall);
        //    this.VerifyCSharpFix(code, fixedCode, codeFixIndex: 1);
        //}

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new ArgumentConstraintTypeMismatchAnalyzer();
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new ArgumentConstraintTypeMismatchCodeFixProvider();
        }
    }
}
