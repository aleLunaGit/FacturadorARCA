namespace Parcial3.Modules
{
    public static class Reader
    {

        public static string ReadString(string prompt)
        {
            string result;
            do
            {
                Presentator.Write($"{prompt}: ");
                result = Console.ReadLine();
                Presentator.WriteLine("");
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
                Presentator.Write($"{prompt}: ");
                if (int.TryParse(Console.ReadLine(), out result))
                {
                Presentator.WriteLine("");
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
                Presentator.Write($"{prompt} (entre {min} y {max}): ");

                if (!int.TryParse(Console.ReadLine(), out result))
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
                Presentator.WriteLine($"{prompt}: ");
                if (float.TryParse(Console.ReadLine(), out result))
                {
                Presentator.WriteLine("");
                    return result;
                }
                Presentator.WriteLine("Error: Formato de número inválido. Use ',' como separador decimal si es necesario.");
            }
        }

        public static DateTime ReadDate(string prompt)
        {
            DateTime result;
            while (true)
            {
                Presentator.WriteLine($"{prompt}: "); 
                if (DateTime.TryParse(Console.ReadLine(), out result))
                {
                Presentator.WriteLine("");
                    return result;
                }
                Presentator.WriteLine("Error: Formato de fecha inválido. Use un formato reconocido como dd/mm/aaaa.");
            }
        }

        public static char ReadChar(string prompt)
        {
            Presentator.WriteLine($"{prompt}: ");
            var character = Console.ReadKey().KeyChar;
            Presentator.WriteLine("");
            return character;
        }
    }
}
