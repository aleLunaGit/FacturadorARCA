using Parcial3.UI.Implementations;
using System;
using System.Text;

namespace Parcial3.Modules
{
    public static class Reader
    {
        private static string ReadLineWithEscape(string prompt)
        {
            Presentator.Write($"{prompt}: ");
            StringBuilder input = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    throw new OperationCanceledException("Operación cancelada por el usuario.");
                }
                if (key.Key == ConsoleKey.Enter)
                {
                    Presentator.WriteLine("");
                    return input.ToString();
                }
                if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                    Presentator.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    input.Append(key.KeyChar);
                    Presentator.Write(key.KeyChar.ToString());
                }
            }
        }

        public static string ReadString(string prompt)
        {
            string result;
            do
            {
                result = ReadLineWithEscape(prompt);
                if (string.IsNullOrWhiteSpace(result))
                {
                    Presentator.WriteLine("Error: El valor no puede estar vacío. Intente de nuevo.");
                }
            } while (string.IsNullOrWhiteSpace(result));
            return result;
        }

        public static int ReadInt(string prompt)
        {
            int result;
            while (true)
            {
                string input = ReadLineWithEscape(prompt);
                if (int.TryParse(input, out result))
                {
                    return result;
                }
                Presentator.WriteLine("Error: Formato de número inválido. Por favor, ingrese solo dígitos.");
            }
        }

        public static int ReadInt(string prompt, int min, int max)
        {
            int result;
            while (true)
            {
                string input = ReadLineWithEscape($"{prompt} (entre {min} y {max})");

                if (!int.TryParse(input, out result))
                {
                    Presentator.WriteLine("Error: Formato inválido. Por favor, ingrese solo dígitos numéricos.");
                    continue;
                }
                if (result >= min && result <= max)
                {
                    return result;
                }
                else
                {
                    Presentator.WriteLine($"Error: La opción debe ser un número entre {min} y {max}.");
                }
            }
        }

        public static float ReadFloat(string prompt)
        {
            float result;
            while (true)
            {
                string input = ReadLineWithEscape(prompt);
                if (float.TryParse(input.Replace('.', ','), out result))
                {
                    return result;
                }
                Presentator.WriteLine("Error: Formato de número inválido. Use ',' como separador decimal si es necesario.");
            }
        }

        public static char ReadChar(string prompt)
        {
            Presentator.Write($"{prompt}: ");
            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Escape)
            {
                throw new OperationCanceledException("Operación cancelada por el usuario.");
            }

            Presentator.WriteLine(key.KeyChar.ToString());
            return key.KeyChar;
        }

        public static void WaitForKey(string prompt)
        {
            Presentator.Write(prompt);
            Console.ReadKey(true);
        }
    }
}