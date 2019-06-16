using System;
using System.Collections.Generic;
using System.Linq;

using FluentValidation.Results;
using FluentValidation.Validators;

namespace ConsoleApplication.Validator.Extensions.Internal
{
    public class SemiCustomValidator<T> : CustomValidator<T>
    {
        public SemiCustomValidator(Action<T, CustomContext> action) : base(action) {}

        public override IEnumerable<ValidationFailure> Validate(PropertyValidatorContext context)
        {
            if (Options.Condition != null && !Options.Condition(context))
                return Enumerable.Empty<ValidationFailure>();

            return base.Validate(context);
        }
    }
}
