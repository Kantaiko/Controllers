using System;

namespace Kantaiko.Controllers.Design
{
    public interface IDeconstructionValidator
    {
        bool CanDeconstruct(Type parameterType);
    }
}
