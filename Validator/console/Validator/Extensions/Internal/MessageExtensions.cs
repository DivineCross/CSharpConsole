using System;
using System.Collections.Generic;
using System.Linq;

using FluentValidation.Results;
using FluentValidation.Validators;

namespace ConsoleApplication.Validator.Extensions.Internal
{
    public static class MessageExtensions
    {
        private static int indentSize;

        static MessageExtensions()
        {
            IndentSize = 4;
        }

        public static int IndentSize
        {
            get => indentSize;
            set => Indentation = new string(' ', indentSize = value);
        }

        private static string Indentation { get; set; }

        public static string Indent(this string v) =>
            $"{Indentation}{v}";
    }
}
