using System.Security.Cryptography.X509Certificates;
using Goblins_and_Castles.Multiplayer;
using OpenAI_API;
using OpenAI_API.Chat;
using Spectre.Console;

namespace Goblins_and_Castles
{

     
    internal class Program
    {

        static async Task Main(string[] args)
        {
            // Set environment of console
            Console.Title = ("Goblins and Castles");
            Console.WriteLine("Welcome to Goblins and Castles, the text-based fantasy RPG game!\n");

            // Prompt user to play 1. Solo 2. Join a server 3. Host a server
            Console.Write("\nPlease choose one of the following:\n1. Solo campaign\n2. Multiplayer campaign (Join)\n3. Multiplayer campaign (Host)\n");
            bool valid_choice = false;
            while (valid_choice == false)
            {
                ConsoleKeyInfo userInput;
                userInput = Console.ReadKey(true);
                switch (userInput.KeyChar)
                {
                    case (char)ConsoleKey.D1:
                        Console.Clear(); // Clear console
                        SoloPlay solo_campaign = new SoloPlay(); // Initiate solo game
                        solo_campaign.Start().GetAwaiter().GetResult();
                        valid_choice = true;
                        break;
                    case (char)ConsoleKey.D2:
                        Console.Clear();
                        Console.Write("Please enter server IP address: "); // Prompt user for server IP address
                        string _ip = Console.ReadLine();
                        ClientSide multi_join = new ClientSide();
                        // User must create a character before entering a server

                        Console.Clear();
                       // With a valid character intro and a name, we enter the client side to make connection with server side
                        CharacterIntro newchar = new CharacterIntro();
                        newchar.Username = newchar.GetName();
                        newchar.Race = newchar.GetRace();
                        newchar.Class = newchar.GetClass();
                        string character_inventory = newchar.GetInventory();
                        string[] invslots = character_inventory.Split('\n');
                        newchar.StartingEquipment = invslots[0];
                        newchar.InventoryPack = invslots[1];
                        multi_join.Start(newchar, _ip);
                        valid_choice = true;
                        break;
                    case (char)ConsoleKey.D3:
                        Console.Clear();
                        ServerSide multi_host = new ServerSide(); // Initiate server side
                        multi_host.Start().GetAwaiter().GetResult();
                        valid_choice = true;
                        break;
                }
                if (valid_choice == false) { Console.WriteLine("Invalid selection."); }

            }
        }
    }
}