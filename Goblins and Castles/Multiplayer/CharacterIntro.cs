using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goblins_and_Castles.Multiplayer
{ 
    public class CharacterIntro
    {
        public string Username { get; set; }
        public string Class { get; set; }
        public string Race { get; set; }
        public string StartingEquipment { get; set; }
        public string InventoryPack { get; set; }

        public string GetInventory()
        {
            string current_inventory = "";

            var weapon = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose default [red]weapon.[/]")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down)[/]")
                    .AddChoices(new[] {
                        "Steel Dagger", "Iron Sword", "Bow and Arrows",
                        "Long Iron Spear", "Powerful Slingshot", "Crossbow and Arrows",
                        "Magical Orb Staff", "Iron Throwing Axe", "Iron Warhammer",
                }));

            var inv_pack = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose default [red]inventory pack.[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down)[/]")
                    .AddChoices(new[] {
                        "3 Gold Pieces, Health Potion, Health Potion, Health Potion, Smoked Meat, Smoked Meat, Fire Flint",
                        "3 Gold Pieces, Health Potion, Stealth Potion, Alertness Potion, Smoked Meat, Smoked Meat, Fire Flint",
                        "3 Gold Pieces, Health Potion, Smoked Meat, Carving Knife, Woodcutting Hatchet, Fire Flint",
                        "3 Gold Pieces, Health Potion, Smoked Meat, Carving Knife, Animal Trap Box, Animal Snare Kit",
                        "3 Gold Pieces, Health Potion, Smoked Meat, Hammer, Chisel, Garden Hoe, Rake, Seed Bag",
                        "3 Gold Pieces, Health Potion, Smoked Meat, Book of Schematics, Hammer, Chisel, Pickaxe",
                        "6 Gold Pieces, Health Potion, Smoked Meat"
                }));

            var armor_pack = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose default [red]equipment.[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down)[/]")
                    .AddChoices(new[] {
                        "Heavy Chainmail Armor, Weapon Sheath",
                        "Heavy Iron Shield, Weapon Sheath, Backpack",
                        "Light Chainmail Armor, Arrow Quiver",
                        "Thick Leather Cloak, Black Hood, Large Backpack",
                        "Leather Work Apron, Leather Work Gloves, Tool Belt",
                        "Magical Robes, Wizard Hat",
                        "Peasant Outfit"
                }));

            current_inventory = ($"Starting Equipment - {weapon}, {armor_pack} \nInventory - {inv_pack}");
            return current_inventory;
        }

        public string GetRace()
        {
            string character_race = "Human";

            var race_select = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose character [yellow]race[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down)[/]")
                    .AddChoices(new[] {
                        "Human",
                        "Elf",
                        "Dwarf",
                        "Troll"
                }));
            character_race = ($"{race_select}");
            return character_race;
        }

        public string GetClass()
        {
            string character_class = "Warrior";

            var class_select = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose character [yellow]class[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down)[/]")
                    .AddChoices(new[] {
                        "Warrior",
                        "Archer",
                        "Druid",
                        "Wizard",
                        "Hunter",
                        "Engineer",
                        "Craftsman"
                }));
            character_class = ($"{class_select}");
            return character_class;

        }

        public string GetName()
        {
            bool proper_name = false;
            bool contains_number = true;
            string name = "Toby";

            while (proper_name == false)
            {
                Console.Write("Character name: ");
                name = Console.ReadLine();
                contains_number = name.Any(char.IsDigit);
                if (contains_number == true)
                {
                    Console.WriteLine("Character name cannot contain numbers.");
                }
                if (name.Length <= 2)
                {
                    Console.WriteLine("Character name must be atleast 3 letters.");
                }
                if (name.Length >= 3 && contains_number == false)
                {
                    proper_name = true;
                }
            }
            return name;
        }

    }


}
