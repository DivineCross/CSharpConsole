using System;
using System.Collections.Generic;

using ConsoleApplication.Validator;
using ConsoleApplication.Validator.Extensions;

namespace ConsoleApplication
{
    public static class Program
    {
        public static void Main()
        {
            var p1 = new Person {
                FirstName = "DC-longlonglong",
                LastName = null,
            };
            var p2 = new Person {
                FirstName = null,
                LastName = null,
                Nicknames = new List<string> {
                    null,
                    "DC-yoyo",
                    null,
                },
                Pets = new List<Pet> {
                    new Pet {
                        Name = null,
                    },
                    new Pet {
                        Name = null,
                    },
                    new Pet {
                        Name = "Dog",
                        Friends = new List<Person> {
                            new Person { FirstName = "DogF" },
                            new Person { FirstName = "DogF" },
                            new Person { FirstName = "DogF" },
                        },
                    }
                },
                Friends = new List<Person> {
                    new Person {
                    },
                },
            };

            var pv1 = new PersonValidatorBase();
            var pv2 = new PersonValidatorAdvanced();

            var r1 = pv1.Validate(p1);
            var r2 = pv2.Validate(p2);

            Console.WriteLine(r1.Display("Person1"));
            Console.WriteLine(r2.Display("Person2"));
        }
    }
}
