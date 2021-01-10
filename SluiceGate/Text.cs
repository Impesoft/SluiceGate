using System;

namespace SluiceGate
{
    internal class Text
    {
        public static void Clearline(int relativeY)
        {
            int intendedY = Console.CursorTop + relativeY;
            if (intendedY < 0) { intendedY = 0; }
            if (intendedY > Console.WindowHeight) { intendedY = Console.WindowHeight; }
            Console.SetCursorPosition(0, intendedY);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, intendedY);
        }

        public static void WriteRight(string text, int top)
        {
            Console.SetCursorPosition(Console.WindowWidth - text.Length, top);
            Console.Write(text);
        }

        internal static void WriteinTime(string text)
        {
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.CursorTop = Console.CursorTop - 1;
            foreach (char c in text)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(75);
            }
            Console.ResetColor();
        }
    }
}