using System;
using System.Collections.Generic;

namespace SluiceGate
{
    public static class GlobalVar
    {
        private static string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // Set required doc paths

        public static string PathShipList = $"{documents}\\ShipList.xml";
        public static string SluiceLogPath = $"{documents}\\ShipsLogged.txt";

        //declare global veriables

        public static StateOfSluice SluiceState = StateOfSluice.Up;
        public static bool[] SluiceFull = { false, false };
        public static List<Ship> ShipList = new List<Ship>();
        public static List<Ship>[] ShipsInStream = new List<Ship>[2];
        public static int LengthShipsInSluiceUpStream = 0;
        public static int LengthShipsInSluiceDownStream = 0;
        public const int SluiceLength = 5;
    }
}