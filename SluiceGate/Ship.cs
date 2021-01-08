using System;

namespace SluiceGate
{
    public class Ship
    {
        public double Toll { get; set; }


        public string Name { get; set; }

        public Length Length
        {
            get
            {
                return length;
            }
            set
            {
                if (value < Length.Small) { length = Length.Small; } else { length = value; }
            }
        }

        private Length length;
        public DateTime ArrivalTime { get; set; } = DateTime.Now;

        public bool IsUpstream { get; set; }

        public Ship() //constructor no arguments for XML parsing
        {
            Name = "Not Set";
            Length = Length.Medium;
            ArrivalTime = DateTime.Now;
            IsUpstream = true;
            Toll = 0;
        }

        public Ship(string name, Length length, bool direction, double toll) // constructor with arguments
        {
            Name = name;
            Length = (Length)length;
            ArrivalTime = DateTime.Now;
            IsUpstream = direction; // true is up (bruikbaar voor tol te betalen)
            Toll = toll;
        }
    }
}