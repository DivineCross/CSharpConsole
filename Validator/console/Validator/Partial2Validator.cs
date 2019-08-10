using System.Collections.Generic;
using System.Linq;

using FluentValidation;

using ConsoleApplication.Validator.Extensions;

namespace ConsoleApplication.Validator
{
    public class Partial2Validator : ValidatorBase<World>
    {
        public Partial2Validator()
        {
            RuleFor(x => x.Ruler)
                .SetValidator(new RulerValidator());
        }

        private class RulerValidator : PersonValidatorBase
        {
            public RulerValidator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .CustomName();

                RuleFor(x => x.Age)
                    .NotNull();

                RuleFor(x => x.KillCount)
                    .NotNull();

                RuleFor(x => x.Mana)
                    .NotNull();

                RuleFor(x => x.Nicknames)
                    .Count(1, 2);

                Pets();
            }

            private void Pets()
            {
                RuleFor(x => x.Pets)
                    .Count(1, 2);

                RuleFor(x => x.Pets)
                    .CustomEach(new PetValidator());
            }
        }
    }
}
