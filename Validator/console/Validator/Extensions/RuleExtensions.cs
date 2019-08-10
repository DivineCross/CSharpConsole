using System;
using System.Collections.Generic;
using System.Linq;

using FluentValidation;
using FluentValidation.Validators;

using ConsoleApplication.Validator.Extensions.Internal;

namespace ConsoleApplication.Validator.Extensions
{
    public static class RuleExtensions
    {
        public static IRuleBuilderOptions<T, IEnumerable<TItem>> Count<T, TItem>(
            this IRuleBuilder<T, IEnumerable<TItem>> ruleBuilder,
            int min,
            int max)
        {
            return ruleBuilder
                .Must(x => {
                    var c = x?.Count() ?? 0;
                    return min <= c && c <= max;
                })
                .WithMessage($"'{{PropertyName}}' 數量必須介於 {min} 與 {max}之間。");
        }

        public static IRuleBuilderOptions<T, string> CustomName<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .MinimumLength(2)
                .MaximumLength(10)
                .Matches(@"^DC\-")
                .WithMessage("'{PropertyName}' 必須是'DC-'開頭。");
        }

        public static IRuleBuilderOptions<T, IEnumerable<TElement>> CustomEach<T, TElement>(
            this IRuleBuilder<T, IEnumerable<TElement>> ruleBuilder,
            AbstractValidator<TElement> validator)
        {
            return ruleBuilder.SemiCustom((xs, ctx) => {
                if (xs?.Any() != true)
                    return;

                var sourceName = ctx.DisplayName;
                xs.Select((x, i) => {
                    var r = validator.Validate(x);
                    var name = $"{sourceName}{i + 1}";

                    if (r.Errors.Any())
                        ctx.AddFailure(r.Display(name));

                    return 0;
                }).ToList();
            });
        }

        private static IRuleBuilderOptions<T, TProperty> SemiCustom<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            Action<TProperty, CustomContext> action)
        {
            return ruleBuilder.SetValidator(new SemiCustomValidator<TProperty>(action));
        }
    }
}
