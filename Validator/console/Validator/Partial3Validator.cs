using System.Collections.Generic;
using System.Linq;

using FluentValidation;

using ConsoleApplication.Validator.Extensions;

namespace ConsoleApplication.Validator
{
    public class Partial3Validator : ValidatorBase<World>
    {
        private List<Person> ws;

        private ILookup<Element?, Person> wizardLookup;

        public Partial3Validator()
        {
            RuleFor(x => x)
                .Must(x => {
                    ws = x.Wizards;
                    wizardLookup = ws.ToLookup(w => w.Element);
                    return true;
                });

            RuleFor(x => x.Wizards)
                .Count(1, 8);

            Fire();
            Air();
            Water();
            Earth();
        }

        private IEnumerable<Person> FiresWs => wizardLookup[Element.Fire];

        private IEnumerable<Person> AirWs => wizardLookup[Element.Air];

        private IEnumerable<Person> WaterWs => wizardLookup[Element.Water];

        private IEnumerable<Person> EarthWs => wizardLookup[Element.Earth];

        private void Fire()
        {
            var v = CreateBasicWizard();
            v.RuleFor(x => x.Str)
                .NotNull();

            RuleFor(x => FiresWs)
                .CustomEach(v)
                .OverridePropertyName("火巫師");
        }

        private void Air()
        {
            var v = CreateBasicWizard();
            v.RuleFor(x => x.Dex)
                .NotNull();

            RuleFor(x => AirWs)
                .CustomEach(v)
                .OverridePropertyName("風巫師");
        }

        private void Water()
        {
            var v = CreateBasicWizard();
            v.RuleFor(x => x.Int)
                .NotNull();

            RuleFor(x => WaterWs)
                .CustomEach(v)
                .OverridePropertyName("水巫師");
        }

        private void Earth()
        {
            var v = CreateBasicWizard();
            v.RuleFor(x => x.Vit)
                .NotNull();

            RuleFor(x => EarthWs)
                .CustomEach(v)
                .OverridePropertyName("地巫師");
        }

        private ValidatorBase<Person> CreateBasicWizard()
        {
            var v = new PersonValidatorBase();

            return v;
        }
    }
}
