using System;
using System.Collections.Generic;
using System.Text;

namespace SluiceGate
{
    public static class GlobalVar
    {
        public static List<Ship> ShipList = new List<Ship>();
        public static int Id;
        public static int LengthShipsInSluice = 0;
        public const int SluiceLength = 5 ;
    }
  
}
