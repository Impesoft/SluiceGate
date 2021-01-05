using System;
using System.IO;

namespace SluiceGate
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            sluice sluice = sluice.GetSluice();
            if (File.Exists(GlobalVar.PathShipList))
            {
                GlobalVar.ShipList = FileIO.ReadShipsFromFile(GlobalVar.PathShipList);
                //  todo set id to id of last ship in list
            }

            //base menu
            bool quit = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Welcome Sluice Manager");
                Console.WriteLine("----------------------\n");
                Console.WriteLine("1) add Ships");
                Console.WriteLine("2) view ships");
                Console.WriteLine("3) view ShipsLog");
                Console.WriteLine("4) clear ShipsLog");
                Console.WriteLine("Q) Quit Application");
                char ManagersChoice = InputManagersChoice();
                switch (ManagersChoice)
                {
                    case '1':
                        sluice.AddShips();
                        break;

                    case '2':
                        Console.Clear();
                        foreach (Ship ship in GlobalVar.ShipList)
                        {
                            Console.WriteLine($"arrived and added in sluice at {ship.ArrivalTime}" +
                                $" name:{ship.Name} length:{(int)ship.Length * 30}m going {(ship.IsUpstream ? "up" : "down")}");
                        }
                        Console.WriteLine("press any key to return to the menu");
                        Console.ReadKey();
                        break;



                    case '4':
                        FileIO.ClearShipsLogged();
                        Console.WriteLine("Log Cleared, Press any key to return to the main menu");
                        Console.ReadKey();
                        break;
                    case 'Q':
                        quit = true;
                        break;

                    default:
                        break;
                }
            } while (!quit);
            Console.Clear();
        }

        private static char InputManagersChoice()
        {
            bool isInValidChoice = true;
            char choice = 'Q';
            do
            {
                Console.CursorLeft = 0;
                choice = Char.ToUpper(Console.ReadKey().KeyChar);
                if (choice == '1' || choice == '2' || choice == '3' || choice == '4' || choice == 'Q')
                {
                    isInValidChoice = false;
                }
            } while (isInValidChoice);
            return choice;
        }
    }
}