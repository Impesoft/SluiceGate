using System;
using System.Collections.Generic;

namespace SluiceGate
{
    internal class Menu
    {
        public static void MainMenu()
        {
            sluice sluice = sluice.GetSluice();
            bool quit = false;
            do
            {
                ShowMenu();
                char ManagersChoice = InputManagersChoice();
                switch (ManagersChoice)
                {
                    case '1':
                        sluice.AddShips();
                        break;

                    case '2':
                        sluice.ViewShips();
                        break;

                    case '3':
                        Console.Clear();
                        sluice.ViewShipsLog();
                        break;

                    case '4':
                        sluice.ClearShipsLog();
                        break;

                    case '5':
                        sluice.ChangeSluiceState();
                        break;

                    case 'Q':
                        quit = true;
                        break;

                    default:
                        break;
                }
            } while (!quit);
        }

        private static void ShowMenu()
        {
            Console.Clear();
            bool sluiceUp = (GlobalVar.SluiceState == StateOfSluice.Up);
            int indexSluice = Convert.ToInt32(sluiceUp);
            Console.WriteLine($"Welcome Sluice Manager");
            string text = $"[sluice = {GlobalVar.SluiceState} ({(GlobalVar.ShipsInStream[indexSluice].Count)} ships in {(sluiceUp ? "upstream" : "downstream")}cue / " +
                $"{(GlobalVar.ShipsInStream[1 - indexSluice].Count)} ships in {(!sluiceUp ? "upstream" : "downstream")}cue)]";
            Text.WriteRight(text, 0);
            Console.WriteLine("----------------------\n");
            Console.WriteLine("1) add Ships");
            Console.WriteLine("2) view ships (and their latest updates)");
            Console.WriteLine("3) view ShipsLog");
            Console.WriteLine("4) clear ShipsLog");
            Console.WriteLine("5) Send Sluice up/down");

            Console.WriteLine("\nQ) Quit Application");
        }



        private static char InputManagersChoice()
        {
            bool isInValidChoice = true;
            char choice;
            do
            {
                Console.CursorLeft = 0;
                choice = Char.ToUpper(Console.ReadKey().KeyChar);
                Text.Clearline(0);
                if (choice == '1' || choice == '2' || choice == '3' || choice == '4' || choice == '5' || choice == 'Q')
                {
                    isInValidChoice = false;
                }
            } while (isInValidChoice);
            return choice;
        }
    }
}