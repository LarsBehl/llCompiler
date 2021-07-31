using System;

namespace LL.Installer.Helper
{
    public static class ConsoleHelper
    {
        public static void WriteWarning(string message) => WriteColor(message, ConsoleColor.Yellow);

        public static void WriteError(string message) => WriteColor(message, ConsoleColor.Red);

        public static void WriteSuccess(string message) => WriteColor(message, ConsoleColor.DarkGreen);

        private static void WriteColor(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void WriteRTFM()
        {
            throw new NotImplementedException();
        }
    }
}