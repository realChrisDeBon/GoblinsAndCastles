using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using Goblins_and_Castles.Multiplayer;
using Goblins_and_Castles.WindowScenes;
using OpenAI_API;
using OpenAI_API.Chat;
using Spectre.Console;
using Terminal.Gui;

namespace Goblins_and_Castles
{
    internal class Program
    {
        static void clear_all(Window thiswindow)
        {
            foreach(View view_ in thiswindow.Subviews)
            {
                thiswindow.Remove(view_);
            }
            try
            {
                Application.Shutdown();
                Application.RequestStop();
            } catch
            {
                // expected exception thrown
            }
        }


        static async Task Main(string[] args)
        {
            // Set new character intro
            CharacterIntro newchar = new CharacterIntro();

            // Set environment of console
            Console.Title = ("Goblins and Castles");
            Terminal.Gui.Application.Init();
            Window mainWindow = new Window()
            {
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

            // Create a welcome label
            Label welcome_label = new Label()
            {
                Text = "Welcome to Goblins and Castles, the text-based fantasy RPG game!",
                X = Pos.Center(),
                Y = Pos.Top(mainWindow),
                Height = 1
            };

            // Create our menu buttons (4 in total)
            const int button_space = 4;
            int buttonspacer = 8;
            Button choice_solo = new Button()
            {
                Text = "Solo campaign",
                X = Pos.Center(),
                Y = buttonspacer,
            };
            buttonspacer += button_space;
            choice_solo.Clicked += () =>
            {
                newchar.add_elements();
                while (newchar.is_finished == false)
                {
                    // wait to proceed
                }
                
                clear_all(mainWindow);
                main_GameScene newscene = new main_GameScene();
                newscene.draw_scene();
            };

            Button choice_joinmulti = new Button()
            {
                Text = "Join server",
                X = Pos.Center(),
                Y = buttonspacer,
            };
            buttonspacer += button_space;
            choice_joinmulti.Clicked += () =>
            {
                clear_all(mainWindow);
            };

            Button choice_hostmulti = new Button()
            {
                Text = "Host server",
                X = Pos.Center(),
                Y = buttonspacer,
            };
            buttonspacer += button_space;
            choice_hostmulti.Clicked += () =>
            {
                clear_all(mainWindow);
            };

            Button choice_settings = new Button()
            {
                Text = "Settings",
                X = Pos.Center(),
                Y = buttonspacer,
            };
            buttonspacer += button_space;
            choice_settings.Clicked += () =>
            {
                clear_all(mainWindow);
            };

            mainWindow.Add(welcome_label, choice_solo, choice_joinmulti, choice_hostmulti, choice_settings);

            try
            {
                Terminal.Gui.Application.Top.Add(mainWindow);
                Application.Run();
            } catch
            {
                // Expected exception thrown
            }

            
        }
    }
}