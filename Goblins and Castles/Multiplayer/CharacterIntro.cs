using NStack;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace Goblins_and_Castles.Multiplayer
{
    public class CharacterIntro
    {
        public bool is_finished = false;
        // String declarations
        public string Username { get; set; }
        public string Class { get; set; }
        public string Race { get; set; }
        public string StartingEquipment { get; set; }
        public string InventoryPack { get; set; }

        // View declarations
        public ComboBox race_selection { get; set; }
        public ComboBox class_selection { get; set; }
        public ComboBox weapon_choice { get; set; }
        public ComboBox inventory_pack { get; set; }
        public ComboBox equipment_choice { get; set; }
        public TextView character_name_field { get; set; }
        public Button finish_creation { get; set; }

        public ColorScheme element_colorscheme = new ColorScheme()
        {
            Normal = Terminal.Gui.Attribute.Make(Terminal.Gui.Color.White, Terminal.Gui.Color.DarkGray),
            Focus = Terminal.Gui.Attribute.Make(Terminal.Gui.Color.White, Terminal.Gui.Color.DarkGray),
            HotNormal = Terminal.Gui.Attribute.Make(Terminal.Gui.Color.White, Terminal.Gui.Color.DarkGray),
            HotFocus = Terminal.Gui.Attribute.Make(Terminal.Gui.Color.White, Terminal.Gui.Color.DarkGray)
        };

        public int current_y = 2;
        public const int element_margin = 2;
        
        public void add_elements()
        {
            var character_creation_dialog = new Dialog()
            {
                Width = Dim.Percent(50),
                Height = 20,
            };

            character_creation_dialog.ColorScheme = new ColorScheme()
            {
                Normal = Terminal.Gui.Attribute.Make(Terminal.Gui.Color.White, Terminal.Gui.Color.DarkGray),
                Focus = Terminal.Gui.Attribute.Make(Terminal.Gui.Color.White, Terminal.Gui.Color.Black),
                HotNormal = Terminal.Gui.Attribute.Make(Terminal.Gui.Color.White, Terminal.Gui.Color.Black),
                HotFocus = Terminal.Gui.Attribute.Make(Terminal.Gui.Color.White, Terminal.Gui.Color.Black)
            };

            ustring[] available_races = {
                        "Human",
                        "Elf",
                        "Dwarf",
                        "Troll" };
            ustring[] available_classes = {
                        "Warrior",
                        "Archer",
                        "Druid",
                        "Wizard",
                        "Hunter",
                        "Engineer",
                        "Craftsman" };
            ustring[] default_weapon = { "Steel Dagger", "Iron Sword", "Bow and Arrows",
                        "Long Iron Spear", "Powerful Slingshot", "Crossbow and Arrows",
                        "Magical Orb Staff", "Iron Throwing Axe", "Iron Warhammer", };
            ustring[] inventory_packs = { "3 Gold Pieces, Health Potion, Health Potion, Health Potion, Smoked Meat, Smoked Meat, Fire Flint",
                        "3 Gold Pieces, Health Potion, Stealth Potion, Alertness Potion, Smoked Meat, Smoked Meat, Fire Flint",
                        "3 Gold Pieces, Health Potion, Smoked Meat, Carving Knife, Woodcutting Hatchet, Fire Flint",
                        "3 Gold Pieces, Health Potion, Smoked Meat, Carving Knife, Animal Trap Box, Animal Snare Kit",
                        "3 Gold Pieces, Health Potion, Smoked Meat, Hammer, Chisel, Garden Hoe, Rake, Seed Bag",
                        "3 Gold Pieces, Health Potion, Smoked Meat, Book of Schematics, Hammer, Chisel, Pickaxe",
                        "6 Gold Pieces, Health Potion, Smoked Meat" };
            ustring[] default_equipment = {"Heavy Chainmail Armor, Weapon Sheath",
                        "Heavy Iron Shield, Weapon Sheath, Backpack",
                        "Light Chainmail Armor, Arrow Quiver",
                        "Thick Leather Cloak, Black Hood, Large Backpack",
                        "Leather Work Apron, Leather Work Gloves, Tool Belt",
                        "Magical Robes, Wizard Hat",
                        "Peasant Outfit"};

            race_selection = new ComboBox()
            {
                Height = 4,
                Width = 14,
                X = Pos.Center(),
                Y = Pos.At(current_y)
            };
            race_selection.SetSource(available_races);
            current_y += element_margin;

            class_selection = new ComboBox()
            {
                Height = 4,
                Width = 14,
                X = Pos.Center(),
                Y = Pos.At(current_y)
            };
            class_selection.SetSource(available_classes);
            current_y += element_margin;

            weapon_choice = new ComboBox()
            {
                Height = 4,
                Width = 14,
                X = Pos.Center(),
                Y = Pos.At(current_y)
            };
            weapon_choice.SetSource(default_weapon);
            current_y += element_margin;

            inventory_pack = new ComboBox()
            {
                Height = 2,
                X = Pos.Center(),
                Y = Pos.At(current_y)
            };
            inventory_pack.SetSource(inventory_packs);
            current_y += element_margin;

            equipment_choice = new ComboBox()
            {
                Height = 4,
                Width = 14,
                X = Pos.Center(),
                Y = Pos.At(current_y),
                ReadOnly = true,
                IsInitialized = true,
                Visible = true,
            };
            equipment_choice.SetSource(default_equipment);
            current_y += element_margin;

            character_name_field = new TextView()
            {
                Height = 4,
                Width = 14,
                Multiline = false,
                ReadOnly = false,
                X = Pos.Center(),
                
            };

            finish_creation = new Button()
            {
                Text = "Finish",
                Width = 10,
                Height = 4,
                X = Pos.Center(),
                Y = Pos.At(current_y),

            };
            finish_creation.Clicked += () =>
            {
                Username = character_name_field.Text.ToString();
                Class = available_classes[class_selection.SelectedItem].ToString();
                Race = available_races[race_selection.SelectedItem].ToString();
                StartingEquipment = $"Starting Equipment - {default_weapon[weapon_choice.SelectedItem].ToString()}, {default_equipment[equipment_choice.SelectedItem].ToString()} \nInventory - {inventory_packs[inventory_pack.SelectedItem].ToString()}";
                is_finished = true;
                Application.RequestStop();
            };

            character_creation_dialog.Add(race_selection, class_selection, weapon_choice, inventory_pack, equipment_choice, character_name_field, finish_creation);
            Application.Run(character_creation_dialog);

        }



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
