using Parcial3.UI.Interfaces;
using System;
using System.Threading;

namespace Parcial3.UI.Implementations
{
    public static class Presentator
    {
        public static void Clear()
        {
            Console.Clear();
        }

        public static void WriteLine(string msg)
        {
            Console.WriteLine($"{msg}");
        }

        public static void Write(string msg)
        {
            Console.Write(msg);
        }

        public static string ReadLineWithCountDown(int seconds)
        {
            WriteLine($"Escriba 'si' para confirmar (tiene {seconds} segundos para elegir)");
            bool inputDetected = false;

            for (int i = seconds; i > 0; i--)
            {
                Write($"\rTiempo restante: {i}s...");

                for (int j = 0; j < 10; j++)
                {
                    if (Console.KeyAvailable)
                    {
                        inputDetected = true;
                        break;
                    }
                    Thread.Sleep(100);
                }

                if (inputDetected)
                {
                    break;
                }
            }

            Write("\r" + new string(' ', 40) + "\r");

            if (inputDetected)
            {
                Write("Confirmación: ");
                return Console.ReadLine();
            }
            else
            {
                WriteLine("\nTiempo agotado.");
                return null;
            }
        }
    }
}