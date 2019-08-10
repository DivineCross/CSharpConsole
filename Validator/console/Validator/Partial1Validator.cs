using System.Collections.Generic;
using System.Linq;

using FluentValidation;

using ConsoleApplication.Validator.Extensions;

namespace ConsoleApplication.Validator
{
    public class Partial1Validator : ValidatorBase<World>
    {
        public Partial1Validator()
        {
            RuleFor(x => x.Capacity)
                .NotNull()
                .InclusiveBetween(1, 99999999)
                .When(x => x.IsNew);

            RuleFor(x => x.Mass)
                .NotNull()
                .InclusiveBetween(1, 9999)
                .When(x => x.IsNew);
        }
    }
}
