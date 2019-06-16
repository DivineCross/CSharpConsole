using System.Globalization;

using FluentValidation;

namespace ConsoleApplication.Validator.Extensions
{
    public abstract class ValidatorBase<T> : AbstractValidator<T>
    {
        static ValidatorBase()
        {
            ValidatorOptions.LanguageManager.Culture = new CultureInfo("zh-TW");
        }
    }
}
