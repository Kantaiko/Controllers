using System;
using System.Collections.Generic;
using Kantaiko.Controllers.Introspection.Factory.Deconstruction;
using Kantaiko.Controllers.Introspection.Factory.Transformers;

namespace Kantaiko.Controllers.Introspection.Factory;

public class IntrospectionBuilder<TContext> : IntrospectionBuilder
{
    public IntrospectionInfo CreateIntrospectionInfo(IEnumerable<Type> lookupTypes)
    {
        return BuildIntrospectionFactory().CreateIntrospectionInfo<TContext>(lookupTypes);
    }
}

public class IntrospectionBuilder
{
    private readonly List<IntrospectionInfoTransformer> _transformers = new();
    private IServiceProvider? _serviceProvider;
    private IDeconstructionValidator? _deconstructionValidator;

    public void AddTransformer(IntrospectionInfoTransformer transformer)
    {
        _transformers.Add(transformer);
    }

    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void SetDeconstructionValidator(IDeconstructionValidator deconstructionValidator)
    {
        _deconstructionValidator = deconstructionValidator;
    }

    public IntrospectionInfoFactory BuildIntrospectionFactory()
    {
        return new IntrospectionInfoFactory(_transformers, _serviceProvider, _deconstructionValidator);
    }
}
