using System;
using System.Collections.Generic;

namespace SluiceGate
{
    class Program
    {

        private static void Main(string[] args)
        {

            Sluice.AddShips();
            Console.Clear();
            foreach (Ship ship in GlobalVar.ShipList)
            {
                Console.WriteLine($"arrived and added in sluice at {ship.ArrivalTime}" +
                    $" name:{ship.Name} length:{(int)ship.Length * 30}m going {(ship.IsUpstream ? "up" : "down")}");
            }


        }
    }
}
