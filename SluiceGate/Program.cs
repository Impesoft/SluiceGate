using System;
using System.Collections.Generic;

namespace SluiceGate
{
    internal class Program
    {
        public static Guard guard = new Guard();

        private static void Main(string[] args)
        {
            AddShips();



        }
        public static void AddShips()
        {
            bool keeprunning;

            do
            {
                Console.Clear();
                Console.WriteLine("Hello Skippers!");
                Console.WriteLine("Welcome to our Sluice");

                AddShipsInput();
                Console.WriteLine("add another ship? or Quit? enter Q for quit, other key to continue");

                keeprunning = char.ToUpper(Console.ReadKey().KeyChar) != 'Q';
            } while (keeprunning);
        }
    public static void AddShipsInput()
        {
            
                Ship ship = guard.EnterNewShip();
                if (guard.CheckDraft(ship))
                {
                    if (ship.Length > GlobVar.SluiceLength)
                    {
                        Console.WriteLine($"Ships length is {guard.CheckLength(ship)} which is longer than the sluice.");
                        Console.ReadKey();
                        return;
                    }

    Console.WriteLine($"ship arrived at {ship.ArrivalTime} was ship {ship.Name} which is" +
                        $" {ship.Length}units long and has a draft of {Math.Round(ship.Draft * 39.3701, 2)}inch going" +
                        $" {(ship.isUpstream ? " up" : " down")}");
                    guard.ShipList.Add(ship);
                    guard.LengthShipsInSluice += ship.Length;
                    if (guard.LengthShipsInSluice <= GlobVar.SluiceLength) { 
                    Console.WriteLine($"space left in sluice {GlobVar.SluiceLength - guard.LengthShipsInSluice} units");
                    } else
                    {
                        Console.WriteLine(  "Sluice to small to add this ship at this time.");
                        Console.ReadKey();
                        return;
                    }
                }
                else
{
    Console.WriteLine("Sorry this ship can't safely enter the sluice.");
                    Console.ReadKey();
                    return;
}

            }
    }
}