using FluentValidation;

using ConsoleApplication.Validator.Extensions;

namespace ConsoleApplication.Validator
{
    public class PersonValidatorBase : ValidatorBase<Person>
    {
        public PersonValidatorBase()
        {
            RuleFor(x => x.FirstName)
                .CustomName();

            When(x => x.FirstName != null, () => {
                RuleFor(x => x.LastName)
                    .CustomName();
            }).Otherwise(() => {
                RuleFor(x => x.LastName)
                    .CustomName();
            });
        }
    }

    public class PersonValidatorAdvanced : PersonValidatorBase
    {
        public PersonValidatorAdvanced() : base()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty();

            RuleFor(x => x.LastName)
                .NotEmpty();

            RuleForEach(x => x.Nicknames)
                .NotEmpty();

            RuleFor(x => x.Pets)
                .CustomEach(new PetValidator())
                .When(x => x.Pets.Count > 2);
        }
    }
}
