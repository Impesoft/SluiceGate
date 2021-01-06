using System;
using System.Collections.Generic;

namespace SluiceGate
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            GlobalVar.ShipsInStream[0] = new List<Ship>();
            GlobalVar.ShipsInStream[1] = new List<Ship>();

            FileIO.CheckForIOFiles();

            Menu.MainMenu();

            Console.Clear();
        }
    }
}