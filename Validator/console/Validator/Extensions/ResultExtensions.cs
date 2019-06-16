using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using FluentValidation.Results;

using ConsoleApplication.Validator.Extensions.Internal;

namespace ConsoleApplication.Validator.Extensions
{
    public static class ResultExtensions
    {
        public static string Display(this ValidationResult r, string instanceName)
        {
            if (!r.Errors.Any())
                return string.Empty;

            var title = $"'{instanceName}' 錯誤：";
            var lines = new[] { title }
                .Concat(Regex
                    .Split(Display(r), Environment.NewLine)
                    .Select(x => x.Indent()));

            return string.Join(Environment.NewLine, lines);
        }

        public static string Display(this ValidationResult r)
        {
            return string.Join(Environment.NewLine, r.Errors.Select(toString));

            string toString(ValidationFailure f)
            {
                var message = f.ErrorMessage;
                var propName = f.PropertyName;
                var name = f.FormattedMessagePlaceholderValues
                    ?.GetValueOrDefault("PropertyName") as string;
                if (name == null)
                    return message;

                var regexIndex = new Regex($@"\[(\d+)\]$");
                var matchIndex = regexIndex.Match(propName);
                if (!matchIndex.Success)
                    return message;

                var index = int.Parse(matchIndex.Groups[1].Value);
                var newName = $"'{name}{index + 1}'";
                var regexName = new Regex($"'{name}'");
                var newMessage = regexName.Replace(message, newName);

                return newMessage;
            }
        }
    }
}
