using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace Goblins_and_Castles.WindowScenes
{
    public class main_GameScene
    {
        public string game_title { get; set; }
        public Window mainWindow;
        public TextView mainText;
        public TextView message_text;
        public main_GameScene(string game_name = "Solo campaign")
        {
            game_title = game_name; // Can be overridden if joining server
        }

        public void append_text(string input_text)
        {
            mainText.Text += input_text;
        }
        public string broadcast_text()
        {
            return message_text.Text.ToString();
        }
        public void draw_scene()
        {
            Terminal.Gui.Application.Init();
            mainWindow = new Window()
            {
                Title = game_title,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };
            mainWindow.ColorScheme = new ColorScheme()
            {
                Normal = Terminal.Gui.Attribute.Make(Terminal.Gui.Color.White, Terminal.Gui.Color.Black),
                Focus = Terminal.Gui.Attribute.Make(Terminal.Gui.Color.White, Terminal.Gui.Color.Black),
                HotNormal = Terminal.Gui.Attribute.Make(Terminal.Gui.Color.White, Terminal.Gui.Color.Black),
                HotFocus = Terminal.Gui.Attribute.Make(Terminal.Gui.Color.White, Terminal.Gui.Color.Black)
            };

            mainText = new TextView()
            {

                Width = Dim.Percent(90),
                Height = Dim.Percent(80),
                X = Pos.Center(),
                Y = 0,
                Multiline = true,
                ReadOnly = true,
                WordWrap = true,
                ColorScheme = new ColorScheme()
                {
                    Normal = Terminal.Gui.Attribute.Make(Color.Black, Color.Gray),
                    Focus = Terminal.Gui.Attribute.Make(Color.Black, Color.Gray),
                    HotNormal = Terminal.Gui.Attribute.Make(Color.Black, Color.Gray),
                    HotFocus = Terminal.Gui.Attribute.Make(Color.Black, Color.Gray)
                },
                Border = new Border()
                {
                    Effect3D = true,
                    
                }
            };
            message_text = new TextView()
            {
                Width = Dim.Percent(50),
                Height = 2,
                X = 5,
                Y = Pos.Bottom(mainWindow) - 5,
                Multiline = false,
                ReadOnly = false,
                ColorScheme = new ColorScheme()
                {
                    Normal = Terminal.Gui.Attribute.Make(Color.Black, Color.Gray),
                    Focus = Terminal.Gui.Attribute.Make(Color.Black, Color.Gray),
                    HotNormal = Terminal.Gui.Attribute.Make(Color.Black, Color.Gray),
                    HotFocus = Terminal.Gui.Attribute.Make(Color.Black, Color.Gray)
                },
            };

            Button send_button = new Button()
            {
                Text = "Send",
                X = Pos.Right(message_text),
                Y = Pos.Bottom(mainWindow) - 5
            };

            mainWindow.Add(mainText, message_text, send_button);
            Application.Top.Add(mainWindow);
            Application.Run();
        }


    }
}
