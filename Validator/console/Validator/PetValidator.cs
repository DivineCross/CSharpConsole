using FluentValidation;

using ConsoleApplication.Validator.Extensions;

namespace ConsoleApplication.Validator
{
    public class PetValidator : ValidatorBase<Pet>
    {
        public PetValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Age)
                .NotEmpty();

            RuleFor(x => x.Friends)
                .CustomEach(new PersonValidatorBase());
        }
    }
}
