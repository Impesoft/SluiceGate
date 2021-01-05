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
               if (value < 1 ) { length = 1; } else { length = value; }
            } 
        }
        private int length;
        public DateTime ArrivalTime = DateTime.Now;
        public double Draft
        {
            get
            {
                return draft;
            }
            set
            {
                if (value < 0.25) { draft = 0.25; } else { draft = value; }
            }
        }
        


        private double draft;
        public bool isUpstream { get; set; }
        public Ship()
        {
            id++;
            Name = "Not Set";
            Length Length = (Length)2;
            ArrivalTime = DateTime.Now;
            Draft = 1.75;
            isUpstream = true; // true is up (bruikbaar voor tol te betalen)

        }
        public Ship(string name, int length, double draft, bool direction )
        {
            id++;
            Name = name;
            Length Length = (Length)length;
            ArrivalTime = DateTime.Now;
            Draft = draft;
            isUpstream = direction; // true is up (bruikbaar voor tol te betalen)

        }
    }
}
