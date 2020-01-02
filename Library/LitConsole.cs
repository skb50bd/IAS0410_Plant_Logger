using System;
using System.Collections.Generic;
using System.Text;

namespace IAS04110
{
    public static class LitConsole
    {
        private static void Write(
            string message, 
            ConsoleColor color = default,
            ConsoleColor bgColor = default,
            bool printLine = false)
        {
            // Make Backup
            var colorBefore = Console.ForegroundColor;
            var bgColorBefore = Console.BackgroundColor;

            // Setup Colors
            if (color != default)
                Console.ForegroundColor = color;
            if (bgColor != default)   
                Console.BackgroundColor = bgColor;

            // Write Message
            Console.Write(message + (printLine ? "\n": string.Empty));

            // Revert Colors
            Console.BackgroundColor = bgColorBefore;
            Console.ForegroundColor = colorBefore;
        }

        public static void Print(string message) =>
            Write(message);

        public static void PrintLine(string message) =>
            Write(message, default, default, true);

        public static void Error(string message) =>
            Write(message, ConsoleColor.Red, default, true);

        public static void Warning(string message) =>
            Write(message, ConsoleColor.DarkYellow, default, true);

        public static void Success(string message) =>
            Write(message, ConsoleColor.Green, default, true);

        public static void Info(string message) =>
            Write(message, ConsoleColor.Blue, default, true);
    }
}
