using FluentValidation;
using MediatR;

namespace DoItFast.Application.Features
{
    public abstract class BaseValidator<TQueryCommand> : AbstractValidator<TQueryCommand>
        where TQueryCommand : class, IBaseRequest
    {
        protected BaseValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
