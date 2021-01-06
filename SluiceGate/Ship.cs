using System;

namespace SluiceGate
{
    public class Ship
    {
        private int id;
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
        public bool IsUpstream { get; set; }

        public Ship() //constructor no arguments
        {
            id++;
            Name = "Not Set";
            Length = (Length)2;
            ArrivalTime = DateTime.Now;
            Draft = 1.75;
            IsUpstream = true; // true is up (bruikbaar voor tol te betalen)
        }

        public Ship(string name, Length length, double draft, bool direction) // constructor with arguments
        {
            id++;
            Name = name;
            Length = (Length)length;
            ArrivalTime = DateTime.Now;
            Draft = draft;
            IsUpstream = direction; // true is up (bruikbaar voor tol te betalen)
        }
    }
}