using Parcial3.Domain.Interfaces;
using Parcial3.Modules;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Parcial3.Domain.Implementations
{
    public class Invoice : IInvoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public float AmountTotal { get; set; }
        public Client Client { get; set; }
        public List<Item> Items { get; set; }
        public int ClientId { get; internal set; }


        public Invoice()
        {
            Items = new List<Item>();
        }

        public void RegisterTypeFactura(string inputType)
        {
            string currentInput = inputType;
            bool isValidInput = false;

            do
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(currentInput) || currentInput.Length != 1)
                    {
                        throw new ArgumentException("Error: Debes ingresar el tipo de factura (A, B, C o E).");
                    }

                    char letter = char.ToUpper(currentInput[0]);

                    if (letter != 'A' && letter != 'B' && letter != 'C' && letter != 'E')
                    {
                        throw new FormatException("Error: El tipo de factura debe ser A, B, C o E.");
                    }

                    Type = letter.ToString();
                    isValidInput = true;
                }
                catch (ArgumentException ex)
                {
                    Presentator.WriteLine(ex.Message);
                    currentInput = Reader.ReadChar("Por favor, ingrese un tipo válido (A/B/C/E)").ToString();
                }
                catch (FormatException ex)
                {
                    Presentator.WriteLine(ex.Message);
                    currentInput = Reader.ReadChar("Por favor, ingrese un tipo válido (A/B/C/E)").ToString();
                }
                catch (Exception ex)
                {
                    Presentator.WriteLine($"Un error inesperado ha ocurrido: {ex.Message}");
                    currentInput = Reader.ReadChar("Por favor, ingrese un tipo válido (A/B/C/E)").ToString();
                }

            } while (!isValidInput);
        }
    }
}