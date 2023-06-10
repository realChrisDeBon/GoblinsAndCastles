using Goblins_and_Castles.Multiplayer;
using OpenAI_API;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Goblins_and_Castles
{
    internal class SoloPlay
    {
        public SoloPlay() { }

   
        public async Task Start()
        {
            // Create local new character
            CharacterIntro new_character = new CharacterIntro();
            string char_name = new_character.GetName();
            string char_class = new_character.GetClass();
            string char_race = new_character.GetRace();

            string character_inventory = new_character.GetInventory();
            string[] invslots = character_inventory.Split('\n');
            new_character.StartingEquipment = invslots[0];
            new_character.InventoryPack = invslots[1];
            Console.Clear();

            // Take character intro data into a string to feed to storyteller AI
            string character_intro = ($"A new adventurer {char_name}, a {char_race} {char_class.ToLower()} has entered the world.\n{new_character.StartingEquipment}\n{new_character.InventoryPack}\n");
            string character_summary = ($"{char_name}, their race is a {char_race} and their skill classification is {char_class.ToLower()}.");

            // Initiate now OpenAIAPI with your API key, then send hard coded prompt to AI
            OpenAIAPI api = new OpenAIAPI(new APIAuthentication("YOUR-API-KEY"));
            var chat = api.Chat.CreateConversation();
            chat.AppendSystemMessage($"You are a story teller for a fantasy game. The player has made a character named {char_name}. There are other characters in the game which are complex and can have intelligent conversations. Each location must have at least 3 sentence description. You will also keep track of time of the day, the weather, the natural environment and the passage of time and the changing of seasons, and any notable landmarks or points of interest in the game world, and any historical or cultural information that may be relevant to the adventure to make the game world feel more alive and realistic. You must track inventory for the player, time within the game world, and locations of characters in the game world. You must also handle any events, combat or challenges. You will always display in the end of your responses the player current health, which can go up when they eat food or drink a potion and go down when they fight, fall, or are exposed to rough weather with 'Health:' then the number as a percent. If a player's health becomes 0, they die and no longer can continue their journey.");
            chat.AppendSystemMessage("This world is medieval themed. That means there are small towns and villages, as well as markets and kingdoms. The landmass is not limitless, and is about the size of the continent of Australia. You should keep track of distances and directions to keep the world confined and realistic. There are multiple races that the player may choose, those are human, dwarf, elf, and troll. The player's race will have an effect on its skills and abilities. Dwarfs are shorter and slower, but are very good with engineering, metallurgy, and craftsmanship. They even understand gun powder. Dwarves typically prefer their kingdoms to be in mountains. Humans are about average in most ways, but are the best at farming and often have villages and markets with established order. Humans typically live in the plains. Elves are excellent at magic and archery, they are also very agile, but lack in physical strength and fighting abilities. They often live in remote areas other races rarely go. Trolls are very tall and big, so they are very strong. Trolls tend to live in their own villages in high altitudes. They have very little technology and mostly hunt and fish as farming requires too much patience.");
            chat.AppendSystemMessage("You can not make decisions for the player or make the player's character do something unless the player says the character will do it. Only the player can control what actions the character does in game, this includes trades, combat, movement, and interactions with other in-game characters. When a player arrives or awakens in a destination, it is up to them what to do. The very first message may be up to a paragraph long, but try to keep most messages short and sweet (just a few sentences) unless there is something very complex going on.");
            chat.AppendSystemMessage("There are 7 classes the player may choose. Warrior, archer, druid, wizard, hunter, engineer, and craftsman. Warriors excel in combat, strength, and tactics. Archers excel with bows and crossbows, stealth, and assassination. Druids and wizards are similar, however druids focus on healing and manipulating nature, where as wizards focus on the ability cast spells to fight enemies. Hunters excel at creating traps and laying traps to catch animals for food and pelts, and have excellent survival and foraging skills. Engineers are excellent builders of gadgets and blacksmithing, but tend to require access to materials and tools near cities and towns to do their work. Engineers can use schematics to create weapons, tools, small structures, gears, and much more. Craftsman excel at more refined things, like carpentry, farming and gardening, animal husbandry, fixing things, making furniture, and possess a broad range of useful knowledge.");
            chat.AppendSystemMessage("There are often large stretches of plains and forests between towns and cities. There can often be infested with goblins, the most common nuisance in this world. Goblins can be nasty muggers who are willing to kill innocent passerby for their things and will watch and follow people from a distance. They sometimes roam in small packs which can be challenging at times and can sometimes present a combat scenario requiring the players to either fight or flee.");
            chat.AppendSystemMessage("Not all characters in the game can give a quest, but most villages or towns should have at least 1 character able to give a quest to the player. A quest can just be a series of tasks within the village or town, or a quest can be a series of tasks stretching multiple kingdoms. Quests can sometimes be story line based and dramatic, or can be as simple as slaying a monster that's been terrorizing people nearby. All quests should offer handsome rewards of gold pieces, usually a minimum of 100 gold pieces. Quests that are longer and more complex, requiring multiple day's worth of travel for example, should pay much more in gold pieces.");
         
            chat.AppendUserInput($"{character_intro}");
            Console.WriteLine($"{character_intro}");
            string entering_game = await chat.GetResponseFromChatbotAsync();
            Console.WriteLine(entering_game);

            bool test = false;
            while (test == false)
            {
                string user_input = Console.ReadLine();
                chat.AppendUserInput(user_input);
                string response = await chat.GetResponseFromChatbotAsync();
                Console.WriteLine(response + "\n");
            }
        }

    }

}