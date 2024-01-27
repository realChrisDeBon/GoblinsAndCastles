using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Chat;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
 
namespace Goblins_and_Castles.Multiplayer
{
    public class ServerSide
    {
        private TcpListener server;
        public List<ClientHandler> clients;
        private OpenAIAPI open_AIApi;
        private Conversation conversation;

        public ServerSide()
        {
            server = new TcpListener(IPAddress.Any, 8888); // Begin listening for client IPs
            clients = new List<ClientHandler>();
            open_AIApi = new OpenAIAPI("YOUR-API-KEY");

            // Create a new conversation with OpenAI
            conversation = open_AIApi.Chat.CreateConversation();

            // Initiate now OpenAIAPI with your API key, then send hard coded prompt to AI
            conversation.AppendSystemMessage($"You are a story teller for a fantasy game. The players will enter the world with a unique name, race, class, and inventory setup. There are other non-player characters in the game which are complex and can have intelligent conversations. Each location must have at least 3 sentence description. You will also keep track of time of the day, the weather, the natural environment and the passage of time and the changing of seasons, and any notable landmarks or points of interest in the game world, and any historical or cultural information that may be relevant to the adventure to make the game world feel more alive and realistic. You must track inventory for the player, time within the game world, and locations of characters in the game world. You must also handle any events, combat or challenges. You will always display in the end of your responses the player current health, which can go up when they eat food or drink a potion and go down when they fight, fall, or are exposed to rough weather with 'Health:' then the number as a percent. If a player's health becomes 0, they die and no longer can continue their journey.");
            conversation.AppendSystemMessage("This world is medieval themed. That means there are small towns and villages, as well as markets and kingdoms. There are multiple races that the player may choose, those are human, dwarf, elf, and troll. The player's race will have an effect on its skills and abilities. Dwarfs are shorter and slower, but are very good with engineering, metallurgy, and craftsmanship. They even understand gun powder. Dwarves typically prefer their kingdoms to be in mountains. Humans are about average in most ways, but are the best at farming and often have villages and markets with established order. Humans typically live in the plains. Elves are excellent at magic and archery, they are also very agile, but lack in physical strength and fighting abilities. They often live in remote areas other races rarely go. Trolls are very tall and big, so they are very strong. Trolls tend to live in their own villages in high altitudes. They have very little technology and mostly hunt and fish as farming requires too much patience.");
            conversation.AppendSystemMessage("You can not make decisions for the player or make the player's character do something unless the player says the character will do it. Only the player can control what actions the character does in game, this includes trades, combat, movement, and interactions with other in-game characters. When a player arrives or awakens in a destination, it is up to them what to do. The very first message may be up to a paragraph long, but try to keep most messages short and sweet (just a few sentences) unless there is something very complex going on.");
            conversation.AppendSystemMessage("There are 7 classes the player may choose. Warrior, archer, druid, wizard, hunter, engineer, and craftsman. Warriors excel in combat, strength, and tactics. Archers excel with bows and crossbows, stealth, and assassination. Druids and wizards are similar, however druids focus on healing and manipulating nature, where as wizards focus on the ability cast spells to fight or curse their enemies. Hunters excel at creating traps and laying traps to catch animals for food and pelts, and have excellent survival and foraging skills. Engineers are excellent builders of gadgets and blacksmithing, but tend to require access to materials and tools near cities and towns to do their work. Engineers can use schematics to create weapons, tools, small structures, gears, and much more. Craftsman excel at more refined things, like carpentry, farming and gardening, animal husbandry, fixing things, making furniture, and possess a broad range of useful knowledge.");
            conversation.AppendSystemMessage("There are often large stretches of plains and forests between towns and cities. There can often be infested with goblins, the most common nuisance in this world. Goblins can be nasty muggers who are willing to kill innocent passerby for their things and will watch and follow people from a distance. They sometimes roam in small packs which can be challenging at times and can sometimes present a combat scenario requiring the players to either fight or flee. Goblins only stand about 4 feet tall, but carry crude makeshift weapons which can lead to injury.");
            conversation.AppendSystemMessage("Not all characters in the game can give a quest, but most villages or towns should have at least 1 character able to give a quest to the player. A quest can just be a series of tasks within the village or town, or a quest can be a series of tasks stretching multiple kingdoms. Quests can sometimes be story line based and dramatic, or can be as simple as slaying a monster that's been terrorizing people nearby. All quests should offer handsome rewards of gold pieces, usually a minimum of 100 gold pieces. Quests that are longer and more complex, requiring multiple day's worth of travel for example, should pay much more in gold pieces.");

        }


        private string GetLocalIPAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "IP Address Not Found";
        }

        public async Task Start()
        {
            server.Start();
            string server_ipaddress = GetLocalIPAddress();
            Console.WriteLine($"Server started.\nServer IP address: {server_ipaddress}");

            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                Console.WriteLine("New client connected.");

                // Create a new thread to handle the client
                ClientHandler clientHandler = new ClientHandler(client, this);
                clients.Add(clientHandler);
                Thread clientThread = new Thread(clientHandler.HandleClient);
                clientThread.Start();
            }
        }

        public async Task BroadcastMessageAsync(string message)
        {
            Console.WriteLine("Broadcasting message: " + message);

            foreach (var client in clients)
            {
                client.SendMessage(message);
            }

            conversation.AppendUserInput(message);
            var response = await conversation.GetResponseFromChatbotAsync();

            // Broadcast the AI response
            var aiResponse = "Storyteller: " + response;
            Console.WriteLine("Broadcasting AI response: " + aiResponse);

            foreach (var client in clients)
            {
                client.SendMessage(aiResponse);
            }
        }

        public async Task BroadcastCharacterIntroductionAsync(CharacterIntro characterIntro)
        {
            // When a new player joins, their character intro is broadcasted
            Console.WriteLine("Broadcasting character introduction:");
            Console.WriteLine($"Class: {characterIntro.Class}");
            Console.WriteLine($"Race: {characterIntro.Race}");
            Console.WriteLine($"Starting Equipment: {characterIntro.StartingEquipment}");
            Console.WriteLine($"Inventory Pack: {characterIntro.InventoryPack}");

            foreach (var client in clients)
            {
                client.SendCharacterIntroduction(characterIntro);
            }
        }
    }

    public class ClientHandler
    {
        private TcpClient client;
        private ServerSide server;
        private NetworkStream stream;

        /// <summary>
        /// The client handler handles incoming connections of clients
        /// </summary>


        public ClientHandler(TcpClient client, ServerSide server)
        {
            this.client = client;
            this.server = server;
            stream = client.GetStream();
        }

        private async Task<string> ReadMessageAsync()
        {
            byte[] buffer = new byte[4096];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }

        public async void HandleClient()
        {
            // Read the username from the client
            string username = await ReadMessageAsync();
            Console.WriteLine("Received username: " + username);

            // Broadcast the received username
            //await server.BroadcastMessageAsync($"Username: {username}");

            // Read the character introduction from the client
            string characterIntroduction = await ReadMessageAsync();
            Console.WriteLine("Received character introduction: " + characterIntroduction);

            try
            {
                // Deserialize the character introduction
                var intro = JsonConvert.DeserializeObject<Intro>(characterIntroduction);
                var characterIntro = intro.CharacterIntroduction;

                // Broadcast the received character introduction
                await server.BroadcastCharacterIntroductionAsync(characterIntro);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deserializing character introduction: " + ex.Message);
            }
            while (true)
            {
                try
                {
                    string message = await ReadMessageAsync();
                    Console.WriteLine("Received message: " + message);
                    ChatMessage newmsg = JsonConvert.DeserializeObject<ChatMessage>(message);
                    // Process the received message
                    string MessageToSend = ($"{newmsg.Username}: {newmsg.Content}");
                    await server.BroadcastMessageAsync(MessageToSend);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading message from client: " + ex.Message);
                    break;
                }
            }

            // Remove the client from the list when the loop breaks
            
            Console.WriteLine("Client disconnected.");

            // Close the client connection
            client.Close();
        }

        public async void SendCharacterIntroduction(CharacterIntro characterIntro)
        {
            string intromesg = ($"New adventurer {characterIntro.Username}, a {characterIntro.Race} {characterIntro.Class.ToLower()} has entered the world.\n{characterIntro.StartingEquipment}\n{characterIntro.InventoryPack}");
            await server.BroadcastMessageAsync(intromesg);
        }

        public async void SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}