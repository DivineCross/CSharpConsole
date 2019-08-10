using FluentValidation;

using ConsoleApplication.Validator.Extensions;

namespace ConsoleApplication.Validator
{
    public class PersonValidatorBase : ValidatorBase<Person>
    {
        public PersonValidatorBase()
        {
            RuleFor(x => x.Name)
                .Length(1, 10);

            RuleFor(x => x.Age)
                .InclusiveBetween(0, 99);

            RuleFor(x => x.KillCount)
                .InclusiveBetween(0, 999);

            RuleFor(x => x.Mana)
                .InclusiveBetween(0, 9999)
                .ScalePrecision(2, 6, true);

            RuleFor(x => x.Str)
                .InclusiveBetween(0, 99);

            RuleFor(x => x.Dex)
                .InclusiveBetween(0, 99);

            RuleFor(x => x.Int)
                .InclusiveBetween(0, 99);

            RuleFor(x => x.Vit)
                .InclusiveBetween(0, 99);

            Nicknames();
        }

        private void Nicknames()
        {
            RuleFor(x => x.Nicknames)
                .Count(0, 5);

            RuleForEach(x => x.Nicknames)
                .Length(2, 10);
        }
    }
}
