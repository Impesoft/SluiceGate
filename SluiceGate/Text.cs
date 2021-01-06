using System;
using System.Collections.Generic;
using System.Text;

namespace SluiceGate
{
    class Text
    {
        public static void clearline(int relativeY)
        {
            int intendedY = Console.CursorTop - relativeY;
            if (intendedY<0) { intendedY = 0; }
            if (intendedY>Console.WindowHeight) { intendedY = Console.WindowHeight; }
            Console.SetCursorPosition(0, intendedY);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, intendedY);
        }
    }
}
