using FluentValidation;

using ConsoleApplication.Validator.Extensions;

namespace ConsoleApplication.Validator
{
    public class PetValidator : ValidatorBase<Pet>
    {
        public PetValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(2, 8);

            RuleFor(x => x.Age)
                .NotNull()
                .InclusiveBetween(0, 9);
        }
    }
}
