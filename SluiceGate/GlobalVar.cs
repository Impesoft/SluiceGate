using System;
using System.Collections.Generic;
using System.Text;

namespace SluiceGate
{
    public static class GlobalVar
    {
        private static string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // Set required doc paths
        public static string PathShipList = $"{documents}\\ShipList.xml";

        public static string SluiceLogPath = $"{documents}\\ShipsLogged.xml";

        public static List<Ship> ShipList = new List<Ship>();
        public static int Id;
        public static int LengthShipsInSluiceUpStream = 0;
        public static int LengthShipsInSluiceDownStream = 0;
        public const int SluiceLength = 5 ;
    }
  
}
