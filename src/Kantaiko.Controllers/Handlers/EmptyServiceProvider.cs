﻿using System;

namespace Kantaiko.Controllers.Handlers;

public class EmptyServiceProvider : IServiceProvider
{
    public static EmptyServiceProvider Instance { get; } = new();

    public object? GetService(Type serviceType)
    {
        return null;
    }
}