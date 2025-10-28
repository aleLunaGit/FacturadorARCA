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
                // Present the prompt to the user
                
                string validateInput = inputType;

                try
                {
                    // Validation 1: Input Length Check
                    if (string.IsNullOrWhiteSpace(inputType) || inputType.Length != 1)
                    {
                        // Throw an exception if it's not a single letter, or if it's empty.
                        throw new ArgumentException("ERROR: You must enter exactly one single letter and it cannot be empty.");
                    }

                    // Convert to uppercase to simplify the comparison
                    char letter = char.ToUpper(inputType[0]);

                    // Validation 2: Letter Value Check
                    if (letter != 'A' && letter != 'B' && letter != 'C')
                    {
                        // Throw an exception if the character is valid in length, but invalid in value.
                        throw new FormatException("ERROR: The invoice type must be A, B or C.");
                    }

                    // If execution reaches here, the input is valid
                    invoiceType = letter.ToString();
                    isValidInput = true; // Set flag to exit the loop
                }
                catch (ArgumentException ex)
                {
                    // Catch errors related to length or empty input (Validation 1)
                    Presentator.WriteLine(ex.Message);
                    isValidInput = false;
                }
                catch (FormatException ex)
                {
                    // Catch errors related to invalid value (Validation 2)
                    Presentator.WriteLine(ex.Message);
                    isValidInput = false;
                }
                catch (Exception)
                {
                    // Catch any other unexpected error
                    Presentator.WriteLine("An unexpected error occurred. Please try again.");
                    isValidInput = false;
                }

            } while (!isValidInput);
            Presentator.WriteLine($"Selected invoice type: {invoiceType}");
            Type =  invoiceType;
            // 'invoiceType' now holds the valid letter ('A', 'B', or 'C').
        }
        public void NumberGenerator()
        {
            string NumberGenerated = "";
            Random rnd = new Random();
                NumberGenerated = DateTime.Now.ToString("ddd") + DateTime.Now.Year + "-" + rnd.Next(100000, 999999);
            this.Number = NumberGenerated;
        }
        public void CalculateTotalAmount()
        {
            float total = 0;
            foreach (var item in Items)
            {
                float price = item.Price;
                float quantity = item.Quantity;
                total = total + item.GetPrice() * item.GetQuantity();
            }
            AmountTotal = total;
        }


        // Setters / Getters 
        public string GetType() => Type;

        public string GetNumber() => Number;

        public DateTime GetDate() => Date;

        public float GetAmountTotal() => AmountTotal;

        public void SetType(string type)=> Type = type;

        // public void SetNumber()=> this.Number = NumberGenerator();

        public void SetDate(DateTime date)=> Date = date;

        public void SetAmountTotal(float amountTotal)=> AmountTotal = amountTotal;
    }
}
