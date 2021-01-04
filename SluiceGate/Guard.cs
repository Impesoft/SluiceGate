using System;
using System.Collections.Generic;
using System.Text;

namespace SluiceGate
{
    class Guard
    {
 
        public Guard()
        {
            int LengthShipsInSluice =0;
            const double SluiceDepth = 3.00;

        }

 
        public Ship EnterNewShip()
        {
            Console.WriteLine("What's the shipsname?");
            string name=Console.ReadLine();
            Console.WriteLine("What's the length of the ship?");
            int length = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("What's the Draft of the ship? (in meters)");
            double draft = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("What's direction are we going? up? or down? type 1 for up 0 for down");
            bool direction = (Console.ReadLine()=="1");
            Ship newShip = new Ship(name,length,draft,direction);
            return newShip;
        }
        public int GetStateOfSluice()
        {
            int sluice=0;
            return sluice;
        }
    }
}
