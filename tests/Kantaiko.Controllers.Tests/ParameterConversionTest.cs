using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Converters;
using Kantaiko.Controllers.Result;
using Kantaiko.Controllers.Tests.Shared;
using Kantaiko.Controllers.Validation;
using Xunit;

namespace Kantaiko.Controllers.Tests
{
    public class ParameterConversionTest : IClassFixture<RequestHandlerProvider>
    {
        private readonly RequestHandlerProvider _requestHandlerProvider;

        public ParameterConversionTest(RequestHandlerProvider requestHandlerProvider)
        {
            _requestHandlerProvider = requestHandlerProvider;
        }

        private static readonly PackageReference TestPackageReference = new("test", Version.Parse("2.0.0"));
        private static readonly PackageInfo TestPackageInfo = new(TestPackageReference, 42);

        private record PackageReference(string Name, Version Version);

        private record PackageInfo(PackageReference Reference, int Rating);

        private class TestConverter : SingleParameterConverter<PackageReference>
        {
            public override ResolutionResult<PackageReference> Convert(ParameterConversionContext context, string value)
            {
                var segments = value.Split("@");
                if (segments.Length != 2)
                    return ResolutionResult.Error("Invalid package reference");

                if (!Version.TryParse(segments[1], out var version))
                    return ResolutionResult.Error("Invalid package reference");

                return ResolutionResult.Success(new PackageReference(segments[0], version));
            }
        }

        private class PackageInfoRepository
        {
            public Task<PackageInfo?> FindByReference(PackageReference reference,
                CancellationToken cancellationToken = default)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return reference == TestPackageReference
                    ? Task.FromResult<PackageInfo?>(TestPackageInfo)
                    : Task.FromResult<PackageInfo?>(null);
            }
        }

        private class AsyncTestConverter : AsyncSingleParameterConverter<PackageInfo>
        {
            private readonly PackageInfoRepository _packageInfoRepository = new();

            private PackageReference? _reference;

            public override ValidationResult Validate(ParameterConversionContext context, string value)
            {
                var conversionResult = new TestConverter().Convert(context);

                if (!conversionResult.Success)
                    return ValidationResult.Error(conversionResult.ErrorMessage);

                _reference = conversionResult.Value;
                return ValidationResult.Success;
            }

            public override async Task<ResolutionResult<PackageInfo>> Resolve(ParameterConversionContext context,
                string value, CancellationToken cancellationToken)
            {
                Debug.Assert(_reference is not null);

                var packageInfo = await _packageInfoRepository.FindByReference(_reference, cancellationToken);

                return packageInfo is null
                    ? ResolutionResult.Error("Package info not found")
                    : ResolutionResult.Success(packageInfo);
            }
        }

        private class PackageController : TestController
        {
            [RegexPattern(@"parse (?<package>.+)")]
            public PackageReference ParsePackageReference(PackageReference package) => package;

            [RegexPattern(@"find (?<package>.+)")]
            public PackageInfo FindPackageInfo(PackageInfo package) => package;
        }

        [Fact]
        public async Task ShouldUseSyncParameterConverter()
        {
            var request = new TestRequest("parse test@2.0.0");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            Assert.True(result.HasReturnValue);
            Assert.Equal(TestPackageReference, result.ReturnValue);
        }

        [Fact]
        public async Task ShouldReportSyncConversionError()
        {
            var request = new TestRequest("parse test");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            Assert.True(result.IsExited);

            var exitReason = Assert.IsType<ErrorExitReason>(result.ExitReason);
            Assert.Equal(RequestErrorStage.ParameterValidation, exitReason.Stage);
            Assert.Equal("Invalid package reference", exitReason.ErrorMessage);
        }

        [Fact]
        public async Task ShouldUseAsyncParameterConverter()
        {
            var request = new TestRequest("find test@2.0.0");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            Assert.True(result.HasReturnValue);
            Assert.Equal(TestPackageInfo, result.ReturnValue);
        }

        [Fact]
        public async Task ShouldReportAsyncValidationError()
        {
            var request = new TestRequest("find test");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            Assert.True(result.IsExited);

            var exitReason = Assert.IsType<ErrorExitReason>(result.ExitReason);
            Assert.Equal(RequestErrorStage.ParameterValidation, exitReason.Stage);
            Assert.Equal("Invalid package reference", exitReason.ErrorMessage);
        }

        [Fact]
        public async Task ShouldReportAsyncResolutionError()
        {
            var request = new TestRequest("find test@1.0.0");
            var result = await _requestHandlerProvider.RequestHandler.HandleAsync(request);

            Assert.True(result.IsExited);

            var exitReason = Assert.IsType<ErrorExitReason>(result.ExitReason);
            Assert.Equal(RequestErrorStage.ParameterResolution, exitReason.Stage);
            Assert.Equal("Package info not found", exitReason.ErrorMessage);
        }
    }
}
