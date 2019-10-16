﻿using Verify = CodeAnalyzers.Episerver.Test.CSharpVerifier<CodeAnalyzers.Episerver.DiagnosticAnalyzers.CSharp.ContentTypeMustHaveGuidAnalyzer>;

using System.Threading.Tasks;
using Xunit;

namespace CodeAnalyzers.Episerver.Test
{
    public class ContentTypeMustHaveGuidAnalyzerTests
    {
        [Fact]
        public async Task CanIgnoreEmptySource()
        {
            await Verify.VerifyAnalyzerAsync("");
        }

        [Fact]
        public async Task CanIgnoreOtherAttribute()
        {
            var test = @"
                using System;

                namespace Test
                {
                    public class OtherAttribute : Attribute
                    {
                    }

                    [Other]
                    public class TypeName
                    {
                    }
                }";

            await Verify.VerifyAnalyzerAsync(test);
        }

        [Fact]
        public async Task CanIgnoreContentTypeWithGuid()
        {
            var test = @"
                using EPiServer.DataAnnotations;

                namespace Test
                {
                    [ContentType(GUID = ""1F218487-9C23-4944-A0E6-76FC1995CBF0"")]
                    public class TypeName
                    {
                    }
                }";

            await Verify.VerifyAnalyzerAsync(test);
        }

        [Fact]
        public async Task CanIgnoreCustomContentTypeWithGuid()
        {
            var test = @"
                using EPiServer.DataAnnotations;

                namespace Test
                {
                    public class CustomContentTypeAttribute : ContentTypeAttribute
                    {
                    }

                    [CustomContentType(GUID = ""1F218487-9C23-4944-A0E6-76FC1995CBF0"")]
                    public class TypeName
                    {
                    }
                }";

            await Verify.VerifyAnalyzerAsync(test);
        }

        [Fact]
        public async Task CanDetectContentTypeWithNoArgumentList()
        {
            var test = @"
                using EPiServer.DataAnnotations;

                namespace Test
                {
                    [ContentType]
                    public class TypeName
                    {
                    }
                }";

            var expected = Verify.Diagnostic().WithLocation(6, 22).WithArguments("Test.TypeName");

            await Verify.VerifyAnalyzerAsync(test, expected);
        }

        [Fact]
        public async Task CanDetectCustomContentTypeWithNoArgumentList()
        {
            var test = @"
                using EPiServer.DataAnnotations;

                namespace Test
                {
                    public class CustomContentTypeAttribute : ContentTypeAttribute
                    {
                    }

                    [CustomContentType]
                    public class TypeName
                    {
                    }
                }";

            var expected = Verify.Diagnostic().WithLocation(10, 22).WithArguments("Test.TypeName");

            await Verify.VerifyAnalyzerAsync(test, expected);
        }

        [Fact]
        public async Task CanDetectContentTypeWithEmptyArgumentList()
        {
            var test = @"
                using EPiServer.DataAnnotations;

                namespace Test
                {
                    [ContentType()]
                    public class TypeName
                    {
                    }
                }";

            var expected = Verify.Diagnostic().WithLocation(6, 22).WithArguments("Test.TypeName");

            await Verify.VerifyAnalyzerAsync(test, expected);
        }

        [Fact]
        public async Task CanDetectCustomContentTypeWithEmptyArgumentList()
        {
            var test = @"
                using EPiServer.DataAnnotations;

                namespace Test
                {
                    public class CustomContentTypeAttribute : ContentTypeAttribute
                    {
                    }

                    [CustomContentType()]
                    public class TypeName
                    {
                    }
                }";

            var expected = Verify.Diagnostic().WithLocation(10, 22).WithArguments("Test.TypeName");

            await Verify.VerifyAnalyzerAsync(test, expected);
        }

        [Fact(Skip = "TODO")]
        public async Task CanDetectContentTypeWithEmptyGuid()
        {
            var test = @"
                using EPiServer.DataAnnotations;

                namespace Test
                {
                    [ContentType(GUID="")]
                    public class TypeName
                    {
                    }
                }";

            var expected = Verify.Diagnostic().WithLocation(6, 22).WithArguments("Test.TypeName");

            await Verify.VerifyAnalyzerAsync(test, expected);
        }

        [Fact(Skip = "TODO")]
        public async Task CanDetectCustomContentTypeWithEmptyGuid()
        {
            var test = @"
                using EPiServer.DataAnnotations;

                namespace Test
                {
                    public class CustomContentTypeAttribute : ContentTypeAttribute
                    {
                    }

                    [CustomContentType(GUID="")]
                    public class TypeName
                    {
                    }
                }";

            var expected = Verify.Diagnostic().WithLocation(6, 22).WithArguments("Test.TypeName");

            await Verify.VerifyAnalyzerAsync(test, expected);
        }

        [Fact(Skip = "TODO")]
        public async Task CanDetectContentTypeWithInvalidGuid()
        {
            var test = @"
                using EPiServer.DataAnnotations;

                namespace Test
                {
                    [ContentType(GUID=""abc"")]
                    public class TypeName
                    {
                    }
                }";

            var expected = Verify.Diagnostic().WithLocation(6, 22).WithArguments("Test.TypeName");

            await Verify.VerifyAnalyzerAsync(test, expected);
        }

        [Fact(Skip = "TODO")]
        public async Task CanDetectCustomContentTypeWithInvalidGuid()
        {
            var test = @"
                using EPiServer.DataAnnotations;

                namespace Test
                {
                    public class CustomContentTypeAttribute : ContentTypeAttribute
                    {
                    }

                    [CustomContentType(GUID=""abc"")]
                    public class TypeName
                    {
                    }
                }";

            var expected = Verify.Diagnostic().WithLocation(6, 22).WithArguments("Test.TypeName");

            await Verify.VerifyAnalyzerAsync(test, expected);
        }
    }
}