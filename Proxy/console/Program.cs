using System;

namespace Application
{
    using DataAccessor;

    public static class Program
    {
        public static void Main()
        {
            var owner = Accessor.Create<Owner>();
            var cat = Accessor.Create<Cat>();
            var dog = Accessor.Create<Dog>();
            var lion = Accessor.Create<Lion>();

            owner.Select(94, 87).Show();
            owner.Delete(94).Show();
            owner.Delete(87).Show();
            owner.Update(94, "I am the owner of these pets.").Show();

            cat.Select(33).Show();
            cat.Delete(333).Show();

            dog.Select(44).Show();
            dog.Delete(444).Show();

            lion.Select(1, 2, 3, 4, 5).Show();
            lion.Delete(1, 2, 3).Show();
        }

        private static void Show(this int[] rows) =>
            Show(rows?.Length);

        private static void Show(this int? count) =>
            Console.WriteLine(count.HasValue
                ? $"[  Out  ] {count} rows are affected.\n"
                : $"[  Out  ] NO OPERATION!\n");
    }
}
