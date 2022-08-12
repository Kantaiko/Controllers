using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.Introspection.Factory;
using Kantaiko.Controllers.ParameterConversion;
using Kantaiko.Controllers.ParameterConversion.Text;
using Kantaiko.Controllers.ParameterConversion.Validation;
using Kantaiko.Controllers.Result;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class CustomTextParameterConverterTest
{
    private static readonly PackageReference TestPackageReference = new("test", Version.Parse("2.0.0"));
    private static readonly PackageInfo TestPackageInfo = new(TestPackageReference, 42);

    public CustomTextParameterConverterTest()
    {
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
    }

    [Fact]
    public async Task ShouldUseSyncParameterConverter()
    {
        var controllerHandler = CreateControllerHandler();

        var context = new TestContext("parse test@2.0.0");
        var result = await controllerHandler.HandleAsync(context);

        Assert.Equal(TestPackageReference, result.ReturnValue);
    }

    [Fact]
    public async Task ShouldReportSyncConversionError()
    {
        var controllerHandler = CreateControllerHandler();

        var context = new TestContext("parse test");
        var result = await controllerHandler.HandleAsync(context);

        Assert.True(result.IsExited);

        var exitReason = Assert.IsType<ErrorExitReason>(result.ExitReason);
        Assert.Equal("Invalid package reference", exitReason.ErrorMessage);
    }

    [Fact]
    public async Task ShouldUseAsyncParameterConverter()
    {
        var controllerHandler = CreateControllerHandler();

        var context = new TestContext("find test@2.0.0");
        var result = await controllerHandler.HandleAsync(context);

        Assert.True(result.HasReturnValue);
        Assert.Equal(TestPackageInfo, result.ReturnValue);
    }

    [Fact]
    public async Task ShouldReportAsyncValidationError()
    {
        var controllerHandler = CreateControllerHandler();

        var context = new TestContext("find test");
        var result = await controllerHandler.HandleAsync(context);

        Assert.True(result.IsExited);

        var exitReason = Assert.IsType<ErrorExitReason>(result.ExitReason);
        Assert.Equal("Invalid package reference", exitReason.ErrorMessage);
    }

    [Fact]
    public async Task ShouldReportAsyncResolutionError()
    {
        var controllerHandler = CreateControllerHandler();

        var context = new TestContext("find test@1.0.0");
        var result = await controllerHandler.HandleAsync(context);

        Assert.True(result.IsExited);

        var exitReason = Assert.IsType<ErrorExitReason>(result.ExitReason);
        Assert.Equal("Package info not found", exitReason.ErrorMessage);
    }

    private class TestController : Controller
    {
        [Pattern("parse {package}")]
        public PackageReference ParsePackageReference(PackageReference package) => package;

        [Pattern("find {package}")]
        public PackageInfo FindPackageInfo(PackageInfo package) => package;

        [Pattern(@"{a} + ")]
        public int SumFailed(int a, int b) => 0;
    }

    private record PackageReference(string Name, Version Version);

    private record PackageInfo(PackageReference Reference, int Rating);

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

    private class PackageReferenceConverter : SingleTextParameterConverter<PackageReference>
    {
        protected override ResolutionResult<PackageReference> Resolve(TextParameterConversionContext context,
            string value)
        {
            var segments = value.Split("@");
            if (segments.Length != 2)
                return ResolutionResult.Error("Invalid package reference");

            if (!Version.TryParse(segments[1], out var version))
                return ResolutionResult.Error("Invalid package reference");

            return ResolutionResult.Success(new PackageReference(segments[0], version));
        }
    }

    private class PackageInfoConverter : SingleAsyncTextParameterConverter<PackageInfo>
    {
        private readonly PackageInfoRepository _packageInfoRepository = new();
        private readonly PackageReferenceConverter _packageReferenceConverter = new();

        private PackageReference? _reference;

        protected override ValidationResult Validate(TextParameterConversionContext context, string value)
        {
            var conversionResult = _packageReferenceConverter.Resolve(context);

            if (!conversionResult.Success)
                return ValidationResult.Error(conversionResult.ErrorMessage);

            _reference = conversionResult.Value;
            return ValidationResult.Success;
        }

        protected override async Task<ResolutionResult<PackageInfo>> ResolveAsync(
            TextParameterConversionContext context,
            string value, CancellationToken cancellationToken = default)
        {
            Debug.Assert(_reference is not null);

            var packageInfo = await _packageInfoRepository.FindByReference(_reference, cancellationToken);

            return packageInfo is null
                ? ResolutionResult.Error("Package info not found")
                : ResolutionResult.Success(packageInfo);
        }
    }

    private static IControllerHandler<TestContext> CreateControllerHandler()
    {
        return TestUtils.CreateControllerHandler<CustomTextParameterConverterTest>(
            (introspectionBuilder, lookupTypes) =>
            {
                var converterCollection = new TextParameterConverterCollection(lookupTypes);

                introspectionBuilder.AddEndpointMatching();
                introspectionBuilder.AddTextParameterConversion(converterCollection);
            },
            handlers =>
            {
                handlers.AddEndpointMatching();
                handlers.AddParameterConversion(h => h.AddTextParameterConversion());
                handlers.AddDefaultControllerHandling();
            }
        );
    }
}
