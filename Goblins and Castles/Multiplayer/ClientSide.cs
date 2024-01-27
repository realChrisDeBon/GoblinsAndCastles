using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace Goblins_and_Castles.Multiplayer
{
    public class ClientSide
    {
        private TcpClient client;
        private string username;
        private CharacterIntro characterIntro;

        public void Start(CharacterIntro characterIntro, string ipaddress)
        {
      
            this.characterIntro = characterIntro;
            this.username = characterIntro.Username;

            client = new TcpClient();
            client.Connect(ipaddress, 8888);
            NetworkStream stream = client.GetStream();

            // Send the character introduction and username to the server
            SendUsername(stream);
            SendCharacterIntroduction(stream);
            
            // Start a new thread to handle incoming messages from the server
            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();

            while (true)
            {
                Console.Write($"{characterIntro.Username}: ");
                string message = Console.ReadLine();

                // Send the message to the server
                SendMessage(username, message);
            }
        }

        private void SendMessage(string username, string content)
        {
            ChatMessage chatMessage = new ChatMessage
            {
                Username = this.username,
                Content = content
            };

            // Serialize the ChatMessage to JSON
            string jsonMessage = JsonConvert.SerializeObject(chatMessage);

            // Send the JSON message to the server
            byte[] buffer = Encoding.UTF8.GetBytes(jsonMessage);
            NetworkStream stream = client.GetStream();
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        private void ReceiveMessages()
        {
            NetworkStream stream = client.GetStream();

            while (true)
            {
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                try
                {
                    if (message.StartsWith("Username:"))
                    {
                        Console.WriteLine(message);
                    }
                    else if (message.StartsWith("Character Introduction:"))
                    {
                        // Extract the character introduction JSON string
                        string introJson = message.Substring("Character Introduction:".Length).Trim();

                        // Deserialize the character introduction
                        CharacterIntro characterIntro = JsonConvert.DeserializeObject<CharacterIntro>(introJson);

                        // Display the deserialized values
                        Console.WriteLine("Received character introduction:");
                        Console.WriteLine($"Username: {characterIntro.Username}");
                        Console.WriteLine($"Class: {characterIntro.Class}");
                        Console.WriteLine($"Race: {characterIntro.Race}");
                        Console.WriteLine($"Starting Equipment: {characterIntro.StartingEquipment}");
                        Console.WriteLine($"Inventory Pack: {characterIntro.InventoryPack}");

                    }
                    else
                    {
                        // Process the regular chat message
                        Console.WriteLine(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error deserializing JSON: " + ex.Message);
                }
            }
        }

        private void SendCharacterIntroduction(NetworkStream stream)
        {
            Intro intro = new Intro
            {
                CharacterIntroduction = characterIntro
            };
            string introMessage = JsonConvert.SerializeObject(intro);
            byte[] introBytes = Encoding.UTF8.GetBytes(introMessage);
            stream.Write(introBytes, 0, introBytes.Length);
        }

        private void SendUsername(NetworkStream stream)
        {
            byte[] usernameBytes = Encoding.UTF8.GetBytes(characterIntro.Username);
            stream.Write(usernameBytes, 0, usernameBytes.Length);
        }


    } 

    public class Intro
    {
        public CharacterIntro CharacterIntroduction { get; set; }
    }
}