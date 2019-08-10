using System.Collections.Generic;
using System.ComponentModel;

namespace ConsoleApplication
{
    public enum WorldStatus
    {
        New,
        Growing,
        Destroyed,
    }

    public enum Element
    {
        Fire,
        Air,
        Water,
        Earth,
    }

    public class World
    {
        [DisplayName("世界狀態")]
        public WorldStatus Status { get; set; }

        [DisplayName("人口")]
        public int? Capacity { get; set; }

        [DisplayName("質量")]
        public int? Mass { get; set; }

        [DisplayName("統治者")]
        public Person Ruler { get; set; }

        [DisplayName("巫師")]
        public List<Person> Wizards { get; set; }

        public bool IsNew => Status == WorldStatus.New;
    }

    public class Person
    {
        [DisplayName("編號")]
        public int Id { get; set; }

        [DisplayName("名字")]
        public string Name { get; set; }

        [DisplayName("年齡")]
        public int? Age { get; set; }

        [DisplayName("討伐數")]
        public int? KillCount { get; set; }

        [DisplayName("魔力量")]
        public decimal? Mana { get; set; }

        [DisplayName("元素別")]
        public Element? Element { get; set; }

        [DisplayName("力量(火)")]
        public int? Str { get; set; }

        [DisplayName("敏捷(風)")]
        public int? Dex { get; set; }

        [DisplayName("智慧(水)")]
        public int? Int { get; set; }

        [DisplayName("體力(地)")]
        public int? Vit { get; set; }

        [DisplayName("小名")]
        public List<string> Nicknames { get ; set; }

        [DisplayName("寵物")]
        public List<Pet> Pets { get ; set; }
    }

    public class Pet
    {
        [DisplayName("編號")]
        public int Id { get; set; }

        [DisplayName("寵物名字")]
        public string Name { get; set; }

        [DisplayName("寵物年齡")]
        public int? Age { get; set; }
    }
}
