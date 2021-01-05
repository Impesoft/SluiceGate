using System;

namespace SluiceGate
{
    internal class sluice
    {
        private static sluice instance;

        private sluice()
        {
        }

        public static sluice GetSluice()
        {
            if (instance == null)
            {
                instance = new sluice();
            }
            return instance;
        }

        public int GetStateOfSluice()
        {
            int sluice = 0;
            return sluice;
        }

        public bool IsDraftOK(double draft)
        {
            bool draftIsOk = true;
            if (draft > 2.75) draftIsOk = false;
            return draftIsOk;
        }

        public CanBeAdded CheckLength(Length length, bool direction)
        {
            if ((int)length > GlobalVar.SluiceLength)
            {
                return CanBeAdded.NoCantFit;
            }
            else if ((int)length > (GlobalVar.SluiceLength - (direction ? GlobalVar.LengthShipsInSluiceUpStream : GlobalVar.LengthShipsInSluiceDownStream)))

            {
                return CanBeAdded.NoNotCurrently;
            }
            else
            {
                return CanBeAdded.Yes;
            }
        }

        public void AddShips()
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

        private void InputShip()
        {
            EnterNewShip();
            string path = GlobalVar.PathShipList;
            FileIO.WriteShipsToFile(path);
        }

        private void EnterNewShip()
        {
            Length length = Length.Special;
            Console.WriteLine("What's the shipsname?");

            string name = InputName();
            Console.WriteLine("What's the Draft of the ship? (in meters)");
            double draft = InputDraft();
            if (!IsDraftOK(draft))
            {
                Console.WriteLine("ship's draft is too deep");
                FileIO.WriteToLog($"Ship {name} refused reason: Ship's draft too deep ({draft} > 2.75m).");
                return;
            }

            Console.WriteLine("What's direction are we going? up? or down? type 1 for up 0 for down");

            bool isUpstream = InputDirection();

            //return newShip;

            Console.WriteLine("What's the length of the ship? (S)mall, (M)edium, (L)ong");
            length = InputLength();
            CanBeAdded canBeAdded = CheckLength(length, isUpstream);
            switch (canBeAdded)
            {
                case CanBeAdded.Yes:
                    Ship ship = new Ship(name, length, draft, isUpstream);
                    FileIO.WriteToLog($"ship {ship.Name} arrived at {ship.ArrivalTime} which is a" +
                                        $" {ship.Length} sized ship with a draft of {Math.Round(ship.Draft * 39.3701, 2)}inch going " +
                                        $" {(ship.IsUpstream ? "upstream" : "downstream")}");

                    GlobalVar.ShipList.Add(ship);
                    if (isUpstream)
                    {
                        GlobalVar.LengthShipsInSluiceUpStream += (int)ship.Length;
                    }
                    else
                    {
                        GlobalVar.LengthShipsInSluiceDownStream += (int)ship.Length;
                    }
                    Console.WriteLine($"space left in sluice {GlobalVar.SluiceLength - (isUpstream ? GlobalVar.LengthShipsInSluiceUpStream : GlobalVar.LengthShipsInSluiceDownStream)} units");
                    Console.ReadKey();
                    break;

                case CanBeAdded.NoCantFit:
                    Console.WriteLine($"Ships length is {length} which is longer than the sluice.");
                    Console.ReadKey();
                    break;

                case CanBeAdded.NoNotCurrently:
                    if (isUpstream)
                    {
                        Console.WriteLine($"Sorry this ship can't safely enter the sluice; current total length is " +
                            $"{30 * GlobalVar.LengthShipsInSluiceUpStream} meters");
                        FileIO.WriteToLog($"Ship {name} refused reason: Ship too long for current upstreamcue({30 * GlobalVar.LengthShipsInSluiceUpStream}m).");
                    }
                    else
                    {
                        Console.WriteLine($"Sorry this ship can't safely enter the sluice; current total length is " +
                            $"{30 * GlobalVar.LengthShipsInSluiceDownStream} meters");
                        FileIO.WriteToLog($"Ship {name} refused reason: Ship too long for current downstreamcue({30 * GlobalVar.LengthShipsInSluiceDownStream}m).");
                    }

                    Console.ReadKey();
                    break;

                default:
                    throw new Exception();
            }
        }

        private string InputName()
        {
            string name = "";
            bool isValidName;
            do
            {
                Console.CursorTop = 3;
                string inputName = Console.ReadLine();
                if (inputName.Length < 1)
                {
                    isValidName = false;
                }
                else
                {
                    isValidName = true;
                    name = inputName;
                }
            } while (!isValidName);
            return name;
        }

        private bool InputDirection()
        {
            bool isInValidDirection;
            bool direction = true;
            do
            {
                string userInputDirection = Console.ReadLine();

                if (userInputDirection == "1" || userInputDirection == "0")
                {
                    direction = Convert.ToBoolean((userInputDirection == "1") ? "True" : "False");
                    isInValidDirection = false;
                }
                else
                {
                    Console.WriteLine("I said 1 or 0!!");
                    isInValidDirection = true;
                }
            } while (isInValidDirection);

            return direction;
        }

        private double InputDraft()
        {
            double draft = 0.0;
            bool noValidDraft;
            do
            {
                string input = Console.ReadLine();
                try
                {
                    draft = Convert.ToDouble(input);
                    noValidDraft = false;
                }
                catch
                {
                    Console.WriteLine("Input not valid");
                    noValidDraft = true;
                }
            } while (noValidDraft);
            return draft;
        }

        private Length InputLength()
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
                        Console.Write("Invalid Length");
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