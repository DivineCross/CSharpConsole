using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ConsoleApplication
{
    public class Person
    {
        [DisplayName("編號")]
        public int Id { get; set; }

        [DisplayName("名")]
        public string FirstName { get; set; }

        [DisplayName("姓")]
        public string LastName { get; set; }

        [DisplayName("年齡")]
        public decimal? Age { get; set; }

        [DisplayName("體重")]
        public decimal? Weight { get; set; }

        [DisplayName("存款")]
        public decimal? Balance { get; set; }

        [DisplayName("小名們")]
        public List<string> Nicknames { get ; set; }

        [DisplayName("寵物們")]
        public List<Pet> Pets { get ; set; }

        [DisplayName("朋友們")]
        public List<Person> Friends { get; set; }
    }

    public class Pet
    {
        [DisplayName("編號")]
        public int Id { get; set; }

        [DisplayName("寵物的名字")]
        public string Name { get; set; }

        [DisplayName("寵物的年齡")]
        public decimal? Age { get; set; }

        [DisplayName("寵物的朋友們")]
        public List<Person> Friends { get; set; }
    }
}
