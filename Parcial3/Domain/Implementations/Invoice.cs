using Parcial3.Domain.Interfaces;
using Parcial3.Modules;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int  ClientId { get; internal set; }


        public Invoice()
        {
            Items = new List<Item>();
        }
        public void RegisterTypeFactura(string inputType)
        {
            string invoiceType = default;
            bool isValidInput = false;

            do
            {                
                string validateInput = inputType;

                try
                {
                    if (string.IsNullOrWhiteSpace(inputType) || inputType.Length != 1)
                    {
                        throw new ArgumentException("Error: Debes ingresar el tipo de factura, no puede estar vacío");
                    }

                    char letter = char.ToUpper(inputType[0]);

                    if (letter != 'A' && letter != 'B' && letter != 'C' && letter != 'E')
                    {
                        throw new FormatException("Error: El tipo de factura debe ser A, B, C o E.");
                    }

                    invoiceType = letter.ToString();
                    isValidInput = true;
                }
                catch (ArgumentException ex)
                {
                    Presentator.WriteLine(ex.Message);
                    isValidInput = false;
                }
                catch (FormatException ex)
                {
                    Presentator.WriteLine(ex.Message);
                    isValidInput = false;
                }
                catch (Exception)
                {
                    Presentator.WriteLine("Un error inesperado ha ocurrido, intente nuevamente.");
                    isValidInput = false;
                }

            } while (!isValidInput);
            Type =  invoiceType;
        }
    }
}
