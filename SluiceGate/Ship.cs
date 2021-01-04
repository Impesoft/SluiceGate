using System;
using System.Collections.Generic;
using System.Text;

namespace SluiceGate
{
    class Ship
    {

        private int id;
        public string Name { get; set; }
        public int Length {
            get 
            {
                return length;
            }
            set
            {
                if (value > 4) { length = 4; } else if (value < 1 ) { length = 1; }
            } 
        }
        private int length;
        private DateTime arrivalTime = DateTime.Now;
        public double Draft { get; set; }
        private double draft;
        public bool Direction { get; set; }
        public Ship()
        {
            id++;
            Name = "Not Set";
            Length = 2;
            arrivalTime = DateTime.Now;
            Draft = 1.75;
            Direction = true; // true is up (bruikbaar voor tol te betalen)

        }
        public Ship(string name, int length, double draft, bool direction )
        {
            id++;
            Name = name;
            Length = length;
            arrivalTime = DateTime.Now;
            Draft = draft;
            Direction = direction; // true is up (bruikbaar voor tol te betalen)

        }
    }
}
