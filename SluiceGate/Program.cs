using System;
using System.Collections.Generic;

namespace SluiceGate
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Skippers!");
            Console.WriteLine("Welcome to our Sluice");
            List<Ship> ShipList = new List<Ship>();

            bool keeprunning = true;
            Guard guard = new Guard();
            do
            {
              Ship ship = guard.EnterNewShip();
                Console.WriteLine(ship.Name+" "+ship.Length+" "+ship.Draft+ (ship.Direction ? " up" : " down"));

                ShipList.Add(ship);


                Console.WriteLine("add another ship? or Quit? enter Q for quit, other key to continue");
                keeprunning = char.ToUpper(Console.ReadKey().KeyChar) != 'Q';
            } while (keeprunning);
        }
    }
}
