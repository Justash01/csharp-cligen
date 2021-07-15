using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace item
{

    public class Description
    {

        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }
    }

    public class MinecraftIcon
    {

        [JsonPropertyName("texture")]
        public string Texture { get; set; }
    }

    public class MinecraftDisplayName
    {

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class MinecraftCreativeCategory
    {

        [JsonPropertyName("parent")]
        public string Parent { get; set; }
    }

    public class MinecraftFood
    {

        [JsonPropertyName("can_always_eat")]
        public bool CanAlwaysEat { get; set; }
    }

    public class MinecraftCooldown
    {

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("duration")]
        public float Duration { get; set; }
    }

    public class FirstPerson
    {

        [JsonPropertyName("scale")]
        public IList<int> Scale { get; set; }
    }

    public class ThirdPerson
    {

        [JsonPropertyName("scale")]
        public IList<int> Scale { get; set; }
    }

    public class MainHand
    {

        [JsonPropertyName("first_person")]
        public FirstPerson FirstPerson { get; set; }

        [JsonPropertyName("third_person")]
        public ThirdPerson ThirdPerson { get; set; }
    }

    public class MinecraftRenderOffsets
    {

        [JsonPropertyName("main_hand")]
        public MainHand MainHand { get; set; }
    }

    public class Components
    {

        [JsonPropertyName("minecraft:icon")]
        public MinecraftIcon MinecraftIcon { get; set; }

        [JsonPropertyName("minecraft:display_name")]
        public MinecraftDisplayName MinecraftDisplayName { get; set; }

        [JsonPropertyName("minecraft:max_stack_size")]
        public int MinecraftMaxStackSize { get; set; }

        [JsonPropertyName("minecraft:use_duration")]
        public int MinecraftUseDuration { get; set; }

        [JsonPropertyName("minecraft:stacked_by_data")]
        public bool MinecraftStackedByData { get; set; }

        [JsonPropertyName("minecraft:hand_equipped")]
        public bool MinecraftHandEquipped { get; set; }

        [JsonPropertyName("minecraft:creative_category")]
        public MinecraftCreativeCategory MinecraftCreativeCategory { get; set; }

        [JsonPropertyName("minecraft:food")]
        public MinecraftFood MinecraftFood { get; set; }

        [JsonPropertyName("minecraft:cooldown")]
        public MinecraftCooldown MinecraftCooldown { get; set; }

        [JsonPropertyName("minecraft:render_offsets")]
        public MinecraftRenderOffsets MinecraftRenderOffsets { get; set; }
    }

    public class MinecraftItem
    {

        [JsonPropertyName("description")]
        public Description Description { get; set; }

        [JsonPropertyName("components")]
        public Components Components { get; set; }
    }

    public class itemMain
    {

        [JsonPropertyName("format_version")]
        public string FormatVersion { get; set; }

        [JsonPropertyName("minecraft:item")]
        public MinecraftItem MinecraftItem { get; set; }
    }



    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Identifier:");
            string identifier = Console.ReadLine();

            Console.WriteLine("Display Name:");
            string displayName = Console.ReadLine();

            Console.WriteLine("Cooldown Duration:");
            string cooldownDur = Console.ReadLine();

            Console.WriteLine("Identifier is: " + identifier);
            Console.WriteLine("Display Name is: " + displayName);
            Console.WriteLine("Cooldown Duration: " + cooldownDur);


            string[] id = identifier.Split(":");
            string idName = id[1];
            string idNamespace = id[0];

            var itemMain = new itemMain
            {
                FormatVersion = "1.16.100",
                MinecraftItem = new MinecraftItem
                {
                    Description = new Description
                    {
                        Identifier = identifier,
                        Category = "Items"
                    },
                    Components = new Components
                    {
                        MinecraftIcon = new MinecraftIcon
                        {
                            Texture = string.Format("{0}.texture", idName)
                        },
                        MinecraftDisplayName = new MinecraftDisplayName
                        {
                            Value = string.Format("{0}.{1}", idNamespace, idName)
                        },
                        MinecraftMaxStackSize = 1,
                        MinecraftUseDuration = 999999999,
                        MinecraftStackedByData = true,
                        MinecraftHandEquipped = true,
                        MinecraftCreativeCategory = new MinecraftCreativeCategory
                        {
                            Parent = "itemGroup.name.tools"
                        },
                        MinecraftFood = new MinecraftFood
                        {
                            CanAlwaysEat = true
                        },
                        MinecraftCooldown = new MinecraftCooldown
                        {
                            Category = idNamespace,
                            Duration = float.Parse(cooldownDur)
                        },
                        MinecraftRenderOffsets = new MinecraftRenderOffsets
                        {
                            MainHand = new MainHand
                            {
                                FirstPerson = new FirstPerson
                                {
                                    Scale = new[] { 0, 0, 0 }
                                },
                                ThirdPerson = new ThirdPerson
                                {
                                    Scale = new[] { 0, 0, 0 }
                                }
                            }
                        }
                    }
                }
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            string itemJSONc = JsonSerializer.Serialize(itemMain, options);

            string dir = string.Format(@"..\..\..\output\{0}", idName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                string itemFile = string.Format(@"..\..\..\output\{0}\{0}.json", idName);
                string langFile = string.Format(@"..\..\..\output\{0}\en_US.lang", idName);
                using (StreamWriter sw = File.CreateText(itemFile))
                {
                    sw.Write(itemJSONc);
                }
                using (StreamWriter sw = File.CreateText(langFile))
                {
                    sw.Write(string.Format("{0}.{1}={2}", idNamespace, idName, displayName));
                }
                Console.WriteLine("done!");
            }
        }
    }
}
