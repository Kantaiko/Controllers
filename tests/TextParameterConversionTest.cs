using Kantaiko.Controllers.EndpointMatching;
using Kantaiko.Controllers.Execution;
using Kantaiko.Controllers.ParameterConversion;
using Kantaiko.Controllers.Tests.Shared;
using Xunit;

namespace Kantaiko.Controllers.Tests;

public class CustomTextParameterConverterTest
{
    private static readonly PackageReference TestPackageReference = new("test", Version.Parse("2.0.0"));
    private static readonly PackageInfo TestPackageInfo = new(TestPackageReference, 42);

    [Fact]
    public async Task ShouldUseSyncParameterConverter()
    {
        var controllerExecutor = CreateControllerExecutor();

        var context = new TestContext("parse test@2.0.0");
        var result = await controllerExecutor.HandleAsync(context);

        Assert.Equal(TestPackageReference, result.EndpointResult);
    }

    [Fact]
    public async Task ShouldReportSyncConversionError()
    {
        var controllerExecutor = CreateControllerExecutor();

        var context = new TestContext("parse test");
        var result = await controllerExecutor.HandleAsync(context);

        Assert.Equal(ControllerErrorCodes.ParameterConversionFailed, result.Error?.Code);
        Assert.Equal(PackageErrorCodes.InvalidReference, result.Error?.InnerError?.Code);
    }

    [Fact]
    public async Task ShouldUseAsyncParameterConverter()
    {
        var controllerExecutor = CreateControllerExecutor();

        var context = new TestContext("find test@2.0.0");
        var result = await controllerExecutor.HandleAsync(context);

        Assert.Equal(TestPackageInfo, result.EndpointResult);
    }

    [Fact]
    public async Task ShouldReportAsyncConversionError()
    {
        var controllerHandler = CreateControllerExecutor();

        var context = new TestContext("find test@1.0.0");
        var result = await controllerHandler.HandleAsync(context);

        Assert.Equal(ControllerErrorCodes.ParameterConversionFailed, result.Error?.Code);
        Assert.Equal(PackageErrorCodes.NotFound, result.Error?.InnerError?.Code);
    }

    [Fact]
    public async Task ShouldReportNoConverterError()
    {
        var controllerHandler = CreateControllerExecutor();

        var context = new TestContext("test foo");
        var result = await controllerHandler.HandleAsync(context);

        Assert.Equal(ControllerErrorCodes.NoSuitableParameterConverter, result.Error?.Code);
        Assert.Equal("file", result.Error?.Parameter?.Name);
    }

    private class TestController : Controller
    {
        [Pattern("parse {package}")]
        public PackageReference ParsePackageReference(PackageReference package) => package;

        [Pattern("find {package}")]
        public PackageInfo FindPackageInfo(PackageInfo package) => package;

        [Pattern("test {file}")]
        public void Test(FileInfo file) { }
    }

    private record PackageReference(string Name, Version Version);

    private record PackageInfo(PackageReference Reference, int Rating);

    private class PackageInfoRepository
    {
        public Task<PackageInfo?> FindByReference(PackageReference reference)
        {
            return reference == TestPackageReference
                ? Task.FromResult<PackageInfo?>(TestPackageInfo)
                : Task.FromResult<PackageInfo?>(null);
        }
    }

    private static class PackageErrorCodes
    {
        private const string Prefix = "Package:";

        public const string InvalidReference = Prefix + nameof(InvalidReference);
        public const string NotFound = Prefix + nameof(NotFound);
    }

    private class PackageReferenceConverter : ITextParameterConverter<PackageReference>
    {
        public ConversionResult<PackageReference> Convert(string value, ParameterConversionContext context)
        {
            var segments = value.Split("@");

            if (segments.Length != 2)
            {
                return new ControllerError(PackageErrorCodes.InvalidReference);
            }

            if (!Version.TryParse(segments[1], out var version))
            {
                return new ControllerError(PackageErrorCodes.InvalidReference);
            }

            return new PackageReference(segments[0], version);
        }
    }

    private class PackageInfoConverter : ITextParameterConverter<PackageInfo>
    {
        private readonly PackageInfoRepository _packageInfoRepository = new();
        private readonly PackageReferenceConverter _packageReferenceConverter = new();

        public async ValueTask<ConversionResult<PackageInfo>> ConvertAsync(string value,
            ParameterConversionContext context)
        {
            var referenceResult = _packageReferenceConverter.Convert(value, context);

            if (!referenceResult.Success)
            {
                return referenceResult.Error;
            }

            var packageInfo = await _packageInfoRepository.FindByReference(referenceResult.Value);

            if (packageInfo is null)
            {
                return new ControllerError(PackageErrorCodes.NotFound);
            }

            return packageInfo;
        }
    }

    private static ControllerExecutor CreateControllerExecutor()
    {
        return TestUtils.CreateControllerExecutor<CustomTextParameterConverterTest>(builder =>
        {
            builder.AddEndpointMatching();
            builder.AddTextParameterConversion();
            builder.AddDefaultHandlers();

            builder.AddConverter(new PackageInfoConverter());
            builder.AddConverter(new PackageReferenceConverter());
        });
    }
}
