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
            if ((int)length > (GlobalVar.SluiceLength - (direction ? GlobalVar.LengthShipsInSluiceUpStream : GlobalVar.LengthShipsInSluiceDownStream)))

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

                if ((GlobalVar.SluiceLength - ((GlobalVar.SluiceState == StateOfSluice.Down) ? GlobalVar.LengthShipsInSluiceUpStream : GlobalVar.LengthShipsInSluiceDownStream)) > 0)
                {
                    Console.WriteLine("add another ship or (S)end sluice enroute? (S to send enroute)");
                    keeprunning = char.ToUpper(Console.ReadKey().KeyChar) != 'S';
                }
                else
                {
                    Console.WriteLine("Sluice Full changing Direction");
                    // Thread.Sleep(2000);
                    keeprunning = false;
                }
            } while (keeprunning);
            ChangeSluiceState();
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
            if (GlobalVar.SluiceFull[(isUpstream ? 1 : 0)])
            {
                Console.WriteLine($"I'm sorry {(isUpstream ? "upstream" : "downstream")} cue is currently full.");
                return;
            }

            Console.WriteLine("What's the length of the ship? (S)mall, (M)edium, (L)ong");
            length = InputLength();
            CanBeAdded canBeAdded = CheckLength(length, isUpstream);
            switch (canBeAdded)
            {
                case CanBeAdded.Yes:
                    Ship ship = new Ship(name, length, draft, isUpstream);
                    FileIO.WriteToLog($"ship {ship.Name} arrived at {ship.ArrivalTime} which is a" +
                                        $" {ship.Length} sized ship with a draft of {10 * Math.Round(ship.Draft),2}cm going " +
                                        $"{(ship.IsUpstream ? "upstream" : "downstream")}.");

                    GlobalVar.ShipList.Add(ship);
                    GlobalVar.ShipsInStream[(isUpstream ? 1 : 0)].Add(ship);
                    if (isUpstream)
                    {
                        GlobalVar.LengthShipsInSluiceUpStream += (int)ship.Length;
                    }
                    else
                    {
                        GlobalVar.LengthShipsInSluiceDownStream += (int)ship.Length;
                    }
                    int spaceLeftInSluice = GlobalVar.SluiceLength - (isUpstream ? GlobalVar.LengthShipsInSluiceUpStream : GlobalVar.LengthShipsInSluiceDownStream);

                    Console.WriteLine($"space left in sluice {spaceLeftInSluice * 30}m");
                    break;

                case CanBeAdded.NoNotCurrently:
                    if (isUpstream)
                    {
                        Console.WriteLine($"Sorry this ship can't safely enter the sluice; current total length is " +
                            $"{30 * GlobalVar.LengthShipsInSluiceUpStream} meters");
                        FileIO.WriteToLog($"{DateTime.Now}: Ship {name} refused reason: Ship too long for current upstreamcue({30 * GlobalVar.LengthShipsInSluiceUpStream}m).");
                    }
                    else
                    {
                        Console.WriteLine($"Sorry this ship can't safely enter the sluice; current total length is " +
                            $"{30 * GlobalVar.LengthShipsInSluiceDownStream} meters");
                        FileIO.WriteToLog($"{DateTime.Now}: Ship {name} refused reason: Ship too long for current downstreamcue({30 * GlobalVar.LengthShipsInSluiceDownStream}m).");
                    }

                    System.Threading.Thread.Sleep(2000);
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
            int top = Console.CursorTop;
            do
            {
                Console.CursorLeft = 0;
                Console.CursorTop = top;
                string userInputDirection = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine();
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
                length = Length.Special;

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
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Invalid Length");
                        Console.ResetColor();
                        System.Threading.Thread.Sleep(500);
                        noValidInput = true;
                        break;
                }
            } while (noValidInput);
            return length;
        }

        public void ChangeSluiceState()
        {
            Console.Clear();
            StateOfSluice currentState = GlobalVar.SluiceState;
            if (currentState == StateOfSluice.Up)
            {
                Console.CursorTop = 0;
                Console.CursorLeft = 5;

                Console.WriteLine($"Decreasing water Level. Sluice is {GlobalVar.SluiceState}     ");
                for (int i = 0; i <= 5; i++)
                {
                    for (int a = 0; a <= i; a++)
                    {
                        Console.CursorTop = a;
                        Console.CursorLeft = 0;

                        Console.Write("    ");
                    }
                    for (int b = i; b < 5; b++)
                    {
                        Console.CursorTop = b;
                        Console.CursorLeft = 0;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("~~~~");
                    }
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.CursorTop = 0;
                    Console.CursorLeft = 5;

                    Console.WriteLine($"Decreasing water Level. Sluice is {GlobalVar.SluiceState}     ");
                    System.Threading.Thread.Sleep(500);
                    GlobalVar.SluiceState = StateOfSluice.EnRoute;
                }
                Console.ResetColor();
                Console.CursorTop = 0;
                Console.CursorLeft = 5;
                GlobalVar.SluiceState = StateOfSluice.Down;
                GlobalVar.ShipsInStream[0].Clear();
                GlobalVar.LengthShipsInSluiceDownStream = 0;
                Console.WriteLine($"                        Sluice is {GlobalVar.SluiceState}     ");
            }
            else
            {
                Console.CursorLeft = 5;
                Console.WriteLine($"Increasing water Level. Sluice is {GlobalVar.SluiceState}     ");
                for (int i = 5; i >= 0; i--)
                {
                    for (int a = 0; a < i; a++)
                    {
                        Console.CursorTop = a;
                        Console.CursorLeft = 0;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        if (i > 0)
                        {
                            Console.Write("     ");
                        }
                    }
                    for (int b = i; b < 5; b++)
                    {
                        Console.CursorTop = b;
                        Console.CursorLeft = 0;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("~~~~~");
                    }
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.CursorTop = 0;
                    Console.CursorLeft = 5;

                    Console.WriteLine($"Decreasing water Level. Sluice is {GlobalVar.SluiceState}     ");
                    System.Threading.Thread.Sleep(500);
                    GlobalVar.SluiceState = StateOfSluice.EnRoute;
                }
                Console.ResetColor();
                Console.CursorTop = 0;
                Console.CursorLeft = 5;
                GlobalVar.SluiceState = StateOfSluice.Up;
                GlobalVar.ShipsInStream[1].Clear();

                GlobalVar.LengthShipsInSluiceUpStream = 0;
                Console.WriteLine($"                        Sluice is {GlobalVar.SluiceState}     ");
                System.Threading.Thread.Sleep(1500);

                Console.Clear();
            }
        }
    }
}