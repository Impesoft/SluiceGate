using System;

namespace SluiceGate
{
    internal static class Sluice
    {
        public static int GetStateOfSluice()
        {
            int sluice = 0;
            return sluice;
        }

        public static bool IsDraftOK(double draft)
        {
            bool draftIsOk = true;
            if (draft > 2.75) draftIsOk = false;
            return draftIsOk;
        }

        public static CanBeAdded CheckLength(Length length)
        {
            if ((int)length > GlobalVar.SluiceLength)
            {
                return CanBeAdded.NoCantFit;
            }
            else if ((int)length > (GlobalVar.SluiceLength - GlobalVar.LengthShipsInSluice))
            {
                return CanBeAdded.NoNotCurrently;
            }
            else
            {
                return CanBeAdded.Yes;
            }
        }

        public static void AddShips()
        {
            bool keeprunning;

            do
            {
                Console.Clear();
                Console.WriteLine("Hello Skippers!");
                Console.WriteLine("Welcome to our Sluice");

                InputShip();
                Console.WriteLine("add another ship? or Quit? enter Q for quit or any other key to continue");

                keeprunning = char.ToUpper(Console.ReadKey().KeyChar) != 'Q';
            } while (keeprunning);
        }

        private static void InputShip()
        {
            EnterNewShip();
        }

        private static void EnterNewShip()
        {
            Length length = Length.Special;
            Console.WriteLine("What's the shipsname?");
            string name = Console.ReadLine();
            Console.WriteLine("What's the Draft of the ship? (in meters)");
            double draft = Convert.ToDouble(Console.ReadLine());
            if (!IsDraftOK(draft))
            {
                Console.WriteLine("ship's draft is too deep");
                return;
            }

            Console.WriteLine("What's direction are we going? up? or down? type 1 for up 0 for down");
            bool direction = (Console.ReadLine() == "1");

            //return newShip;

            Console.WriteLine("What's the length of the ship? (S)mall, (M)edium, (L)ong");
            length = InputLength();
            CanBeAdded canBeAdded = CheckLength(length);
            switch (canBeAdded)
            {
                case CanBeAdded.Yes:
                    Ship ship = new Ship(name, length, draft, direction);

                    Console.WriteLine($"ship arrived at {ship.ArrivalTime} was ship {ship.Name} which is a" +
                                        $" {ship.Length} size ship and has a draft of {Math.Round(ship.Draft * 39.3701, 2)}inch going " +
                                        $" {(ship.IsUpstream ? "up" : "down")}");

                    GlobalVar.ShipList.Add(ship);
                    GlobalVar.LengthShipsInSluice += (int)ship.Length;
                    Console.WriteLine($"space left in sluice {GlobalVar.SluiceLength - GlobalVar.LengthShipsInSluice} units");
                    Console.ReadKey();
                    break;

                case CanBeAdded.NoCantFit:
                    Console.WriteLine($"Ships length is {length} which is longer than the sluice.");
                    Console.ReadKey();
                    break;

                case CanBeAdded.NoNotCurrently:
                    Console.WriteLine($"Sorry this ship can't safely enter the sluice.Already {GlobalVar.ShipList.Count} lists in Sluice" +
                        $" for a total length of {30 * GlobalVar.LengthShipsInSluice} meters");
                    Console.ReadKey();
                    break;

                default:
                    throw new Exception();
            }
        }

        private static Length InputLength()
        {
            Length length;
            bool noValidInput;

            do
            {
                Console.CursorLeft = 0;
                Console.Write("                    ");
                Console.CursorLeft = 0;
                char size = char.ToUpper(Console.ReadKey().KeyChar);
                switch (size)
                {
                    case 'S':
                        length = Length.Small;
                        Console.WriteLine("mall");
                        noValidInput = false;
                        break;

                    case 'M':
                        length = Length.Medium;
                        Console.WriteLine("edium");
                        noValidInput = false; 
                        break;

                    case 'L':
                        length = Length.Long;
                        Console.WriteLine("ong");
                        noValidInput = false; 
                        break;

                    default:
                        Console.CursorLeft = 0;
                        Console.Write("unhandled exception");
                        Console.ReadKey();
                        length = Length.Special;
                        noValidInput = true; 
                        break;
                }
            } while (noValidInput);
            return length;
        }
    }
}