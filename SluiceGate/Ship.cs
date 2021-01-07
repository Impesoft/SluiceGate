using System;

namespace SluiceGate
{
    public class Ship
    {
        private int id;
        public double Toll { get; set; }
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                if (value > 0)
                {
                    id = value;
                }
            }
        }
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
        public DateTime ArrivalTime = DateTime.Now;

        public bool IsUpstream { get; set; }

        public Ship() //constructor no arguments for XML parsing
        {
            GlobalVar.Id++;
            id = GlobalVar.Id;
            Name = "Not Set";
            Length = (Length)2;
            ArrivalTime = DateTime.Now;
            IsUpstream = true; 
            Toll = 0;
        }

        public Ship(string name, Length length, bool direction, double toll) // constructor with arguments
        {
            GlobalVar.Id++;
            id = GlobalVar.Id;
            Name = name;
            Length = (Length)length;
            ArrivalTime = DateTime.Now;
            IsUpstream = direction; // true is up (bruikbaar voor tol te betalen)
            Toll = toll;
        }
    }
}