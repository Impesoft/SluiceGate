using System;
using System.Linq;

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

        public bool IsDraftTooDeep(double draft, string name)
        {
            bool draftIsNotOk = false;
            if (draft > 2.75)
            {
                draftIsNotOk = true;
                Console.WriteLine("ship's draft is too deep");
                FileIO.WriteToLog($"{DateTime.Now}: ship {name} refused reason: Ship's draft too deep ({draft} > 2.75m).");
            }
            return draftIsNotOk;
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

        internal void ViewShips()
        {
            Console.Clear();
            if (GlobalVar.ShipList.Count > 0)
            {
                foreach (Ship ship in GlobalVar.ShipList)
                {
                    Console.WriteLine($"{ship.ArrivalTime}: :{ship.Name} arrived in {(ship.IsUpstream ? "upstream" : "downstream")} sluicecue " +
                        $" (length:{(int)ship.Length * 30}m draft:{ship.Draft})");
                }
            }
            else
            {
                Console.WriteLine("The Shiplist is currently empty.");
            }
            Console.WriteLine("press any key to return to the menu");
            Console.ReadKey();
        }

        public void AddShips()
        {
            bool keeprunning;

            do
            {
                HelloManager();

                InputShip();

                keeprunning = QueryAddAnotherShip();
            } while (keeprunning);
            ChangeSluiceState();
        }

        private bool QueryAddAnotherShip()
        {
            bool keeprunning;

            if ((GlobalVar.SluiceLength - ((GlobalVar.SluiceState == StateOfSluice.Down) ? GlobalVar.LengthShipsInSluiceUpStream : GlobalVar.LengthShipsInSluiceDownStream)) > 0)
            {
                Console.WriteLine("Press (S) to send sluice enroute, or another key to add another ship?");
                keeprunning = char.ToUpper(Console.ReadKey().KeyChar) != 'S';
            }
            else
            {
                Console.WriteLine("Sluice Full changing Direction");
                // Thread.Sleep(2000);
                keeprunning = false;
            }
            return keeprunning;
        }

        private void HelloManager()
        {
            Console.Clear();
            Console.WriteLine($"Welcome Sluice Manager (sluice={GlobalVar.SluiceState})");
            Console.WriteLine("----------------------");
        }

        private void InputShip()
        {
            EnterNewShip();
            string path = GlobalVar.PathShipList;
            FileIO.WriteShipsToFile(path);
        }

        private void EnterNewShip()
        {
             Length length;// = Length.Special;
            double draft;
            Console.WriteLine("What's the shipsname?");
            string name = InputName();
            bool toUpdate = false;
            if ((GlobalVar.ShipList.Any(ship => ship.Name == name)))
            {
                toUpdate = true;
                Ship ship = GetShipInfo(name);
                draft = ship.Draft;
                length = ship.Length;
            }
            else
            {
                Console.WriteLine("What's the Draft of the ship? (in meters)");
                draft = InputDraft();
                if (IsDraftTooDeep(draft, name)) { return; }
            
            Console.WriteLine("What's the length of the ship? (S)mall, (M)edium, (L)ong");
            length = InputLength();
            }
            Console.WriteLine("Going? up? or down? type 1 for up, 0 for down");
            bool isUpstream = InputDirection();
            if (IsSluiceFull(isUpstream)) { return; }
            CanBeAdded canBeAdded = CheckLength(length, isUpstream);
            switch (canBeAdded)
            {
                case CanBeAdded.Yes:
                    if (toUpdate) { UpdateShip(name, length, draft, isUpstream); } 
                    else { AddShip(name, length, draft, isUpstream); }
                    break;
                case CanBeAdded.NoNotCurrently:
                    CantBeAddedAtThisTime(name, isUpstream);
                    break;
            }
        }

        private Ship GetShipInfo(string name)
        {
            Ship ship = GlobalVar.ShipList.FirstOrDefault(ship => ship.Name == name);

            Console.WriteLine("ship already in database. reading info");
            System.Threading.Thread.Sleep(2000);
            Text.Clearline(-1);
            return ship;
        }

        private void CantBeAddedAtThisTime(string name, bool isUpstream)
        {
            if (isUpstream)
            {
                Console.WriteLine($"Sorry this ship can't safely enter the sluice; current total length is " +
                    $"{30 * GlobalVar.LengthShipsInSluiceUpStream} meters");
                FileIO.WriteToLog($"{DateTime.Now}: ship {name} refused reason: Ship too long for current upstreamcue({30 * GlobalVar.LengthShipsInSluiceUpStream}m only {30 * (GlobalVar.SluiceLength- GlobalVar.LengthShipsInSluiceUpStream)}).");
            }
            else
            {
                Console.WriteLine($"Sorry this ship can't safely enter the sluice; current total length is " +
                    $"{30 * GlobalVar.LengthShipsInSluiceDownStream} meters");
                FileIO.WriteToLog($"{DateTime.Now}: ship {name} refused reason: Ship too long for current downstreamcue({30 * GlobalVar.LengthShipsInSluiceDownStream}m).");
            }

            System.Threading.Thread.Sleep(2000);
        }
        private void UpdateShip(string name, Length length, double draft, bool isUpstream)
        {
            double toll = PayToll(isUpstream, length);
            Ship ship = new Ship(name, length, draft, isUpstream, toll);
            FileIO.WriteToLog($"{ship.ArrivalTime}: ship {ship.Name} arrived (size:" +
                                $" {ship.Length}, draft:{100 * Math.Round(ship.Draft),2}cm) going " +
                                $"{(ship.IsUpstream ? "upstream" : "downstream")}.");
            AddInLocalUpStream(isUpstream, ship);
        }


        private void AddShip(string name, Length length, double draft, bool isUpstream)
        {
            double toll = PayToll(isUpstream, length);
            Ship ship = new Ship(name, length, draft, isUpstream, toll);
            FileIO.WriteToLog($"{ship.ArrivalTime}: ship {ship.Name} arrived (size:" +
                                $" {ship.Length}, draft:{100 * Math.Round(ship.Draft),2}cm) going " +
                                $"{(ship.IsUpstream ? "upstream" : "downstream")}.");
            
                GlobalVar.ShipList.Add(ship);
            AddInLocalUpStream(isUpstream,ship);
        }

        private void AddInLocalUpStream(bool isUpstream, Ship ship)
        {
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

            Console.WriteLine($"space left in {(isUpstream ? "upstream" : "downstream")} cue {spaceLeftInSluice * 30}m");
        }

        private bool IsSluiceFull(bool isUpstream)
        {
            bool sluiceFull = false;
            if (GlobalVar.SluiceFull[(isUpstream ? 1 : 0)])
            {
                Console.WriteLine($"I'm sorry {(isUpstream ? "upstream" : "downstream")} cue is currently full.");
                sluiceFull = true;
            }
            return sluiceFull;
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
                Text.Clearline(0);
                string userInputDirection = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine();
                if (userInputDirection == "1" || userInputDirection == "0")
                {
                    direction = Convert.ToBoolean((userInputDirection == "1") ? "True" : "False");
                    isInValidDirection = false;
                    Text.Clearline(-1);
                    Console.WriteLine((direction ? "Upstream" : "Downstream"));
                }
                else
                {
                    Console.WriteLine("I said 1 or 0!!");
                    isInValidDirection = true;
                }
            } while (isInValidDirection);

            return direction;
        }

        public double PayToll(bool isUpstream, Length length)
        {
            int toll = 0;
            if (isUpstream)
            {
                toll = (int)length * 5;
                Console.WriteLine("Toll to Pay: " + toll + " Euro");
            }
            return toll;
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
            (bool,Length) noValidInput;
            do
            {
                Text.Clearline(0);
                char size = char.ToUpper(Console.ReadKey().KeyChar);
                noValidInput= CompleteInput(size);
            } while (noValidInput.Item1);
            return noValidInput.Item2;
        }

        private (bool, Length) CompleteInput(char size)
        {
            (bool, Length) noValidInput;

            switch (size)
            {
                case 'S':
                    noValidInput.Item2 = Length.Small;
                    Text.Clearline(0);
                    Console.WriteLine("Small");
                    noValidInput.Item1 = false;
                    break;

                case 'M':
                    noValidInput.Item2 = Length.Medium;
                    Text.Clearline(0); 
                    Console.WriteLine("Medium");
                    noValidInput.Item1 = false;
                    break;

                case 'L':
                    noValidInput.Item2 = Length.Long;
                    Text.Clearline(0); 
                    Console.WriteLine("Long");
                    noValidInput.Item1 = false;
                    break;

                default:
                    Console.CursorLeft = 0;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Invalid Length");
                    Console.ResetColor();                    
                    noValidInput.Item2 = Length.Special;
                    noValidInput.Item1 = true;
                    System.Threading.Thread.Sleep(500);
                    break;
            }
            return noValidInput;
            ;
        }

        public void ChangeSluiceState()
        {
            Console.Clear();
            StateOfSluice currentState = GlobalVar.SluiceState;
            if (currentState == StateOfSluice.Up)
            {
                MoveSluiceDown();
                LetDownStreamShipsLeave();
                Console.WriteLine($"                        Sluice is {GlobalVar.SluiceState}     ");
            }
            else
            {
                MoveSluiceUp();
                LetUpstreamShipsLeave();
                Console.WriteLine($"                        Sluice is {GlobalVar.SluiceState}     ");
                System.Threading.Thread.Sleep(1500);

                Console.Clear();
            }
        }

        private void LetUpstreamShipsLeave()
        {
            Console.ResetColor();
            Console.CursorTop = 0;
            Console.CursorLeft = 5;
            GlobalVar.SluiceState = StateOfSluice.Up;
            if (GlobalVar.ShipsInStream[1].Count > 0)
            {
                foreach (Ship ship in GlobalVar.ShipsInStream[1])
                {
                    FileIO.WriteToLog($"{DateTime.Now}: ship {ship.Name} left sluice (size:" +
                                           $" {ship.Length}, draft:{100 * Math.Round(ship.Draft),2}cm) leaving " +
                                           $"{(ship.IsUpstream ? "upstream" : "downstream")}.");
                }
            }
            GlobalVar.ShipsInStream[1].Clear();

            GlobalVar.LengthShipsInSluiceUpStream = 0;
        }

        private void MoveSluiceUp()
        {
            Console.CursorLeft = 5;
            Console.WriteLine($"Increasing water Level. Sluice is {GlobalVar.SluiceState}     ");
            for (int i = 5; i >= 0; i--)
            {
                for (int a = 0; a < i; a++)
                {
                    Console.CursorTop = a;
                    Console.CursorLeft = 0;

                    if (i > 0)
                    {
                        Text.Clearline(0);
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

                Console.WriteLine($"Increasing water Level. Sluice is {GlobalVar.SluiceState}     ");
                System.Threading.Thread.Sleep(500);
                GlobalVar.SluiceState = StateOfSluice.EnRoute;
            }
        }

        private void LetDownStreamShipsLeave()
        {
            Console.ResetColor();
            Console.CursorTop = 0;
            Console.CursorLeft = 5;
            GlobalVar.SluiceState = StateOfSluice.Down;
            if (GlobalVar.ShipsInStream[0].Count > 0)
            {
                foreach (Ship ship in GlobalVar.ShipsInStream[0])
                {
                    FileIO.WriteToLog($"{DateTime.Now}: ship {ship.Name} left sluice (size:" +
                                           $" {ship.Length}, draft:{100 * Math.Round(ship.Draft),2}cm) leaving " +
                                           $"{(ship.IsUpstream ? "upstream" : "downstream")}.");
                }
            }
            GlobalVar.ShipsInStream[0].Clear();
            GlobalVar.LengthShipsInSluiceDownStream = 0;
        }

        private void MoveSluiceDown()
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

                    Text.Clearline(0);
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
        }
    }
}