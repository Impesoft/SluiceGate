using System;
using System.Collections.Generic;
using System.Text;

namespace SluiceGate
{
    class Text
    {
        public static void Clearline(int relativeY)
        {
            int intendedY = Console.CursorTop + relativeY;
            if (intendedY<0) { intendedY = 0; }
            if (intendedY>Console.WindowHeight) { intendedY = Console.WindowHeight; }
            Console.SetCursorPosition(0, intendedY);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, intendedY);
        }
        public static void WriteRight (string text, int top)
        {
            Console.SetCursorPosition(Console.WindowWidth - text.Length, top);
            Console.Write(text);
        }
    }
}
