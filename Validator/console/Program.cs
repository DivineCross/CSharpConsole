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
            var w = new World {
                Status = WorldStatus.New,
                Capacity = -1,
                Mass = -1,
                Ruler = new Person {
                    Name = "D-God",
                    Age = 18,
                    KillCount = -1,
                    Mana = 6666.999m,
                    Nicknames = new List<string> {
                        "A",
                        "B",
                    },
                    Pets = new List<Pet> {
                        new Pet {
                            Name = "Leo",
                            Age = 8,
                        },
                        new Pet {
                            Name = "L",
                            Age = null,
                        },
                    },
                },
                Wizards = new List<Person> {
                    new Person {
                        Str = null,
                        Element = Element.Fire,
                    },
                    new Person {
                        Element = Element.Air,
                    },
                    new Person {
                        Element = Element.Water,
                    },
                    new Person {
                        Element = Element.Earth,
                    },
                    new Person {
                        Element = Element.Fire,
                    },
                    new Person {
                        Element = Element.Air,
                    },
                    new Person {
                        Element = Element.Water,
                    },
                    new Person {
                        Element = Element.Earth,
                    },
                },
            };

            var p1 = new Partial1Validator();
            var r1 = p1.Validate(w);

            var p2 = new Partial2Validator();
            var r2 = p2.Validate(w);

            var p3 = new Partial3Validator();
            var r3 = p3.Validate(w);

            Console.WriteLine(r1.Display("世界的開端"));
            Console.WriteLine(r2.Display("統治者情報"));
            Console.WriteLine(r3.Display("巫師的領土"));
        }
    }
}
