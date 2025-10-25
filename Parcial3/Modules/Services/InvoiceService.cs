using Parcial3.Interfaces;
using Parcial3.Modules.Services;


namespace Parcial3.Modules.Repositorys
{
    public class InvoiceService : CrudService<Invoice>, IInvoiceService<Invoice>
    {
        
        private readonly IRepository<Client> _clientRepository;
        private readonly ItemService _itemService;

        
        public InvoiceService( IRepository<Invoice> invoiceRepository, IRepository<Client> clientRepository,   ItemService itemService): base(invoiceRepository) 
        {

            _clientRepository = clientRepository;
            _itemService = itemService;
        }
        // Register the invoice and associates to a client
        public override void Register()
        {
            int id = Reader.ReadInt("A que cliente le crearemos una factura? Segun ID");
            Client client = _clientRepository.GetByID(id);
            List<Item> listItems = AddItems();
            Invoice invoice = new Invoice
            {
                Type = RegisterTypeFactura(),
                Number = NumberGenerator(),
                Date = DateTime.Now,
                Client = client,
                Items = listItems,
                AmountTotal = CalculateTotalAmount(listItems)
            };

            string input = Reader.ReadChar("Presione X Si quiere modificar la Factura | presione cualquier tecla para cerrar la factura").ToString().ToLower();
            Presentator.WriteLine("");
            if (input == "x")
            {
                Invoice modifiedInvoice = Update(invoice);
                // modifiedInvoice.AmountTotal = CalculateTotalAmount(modifiedInvoice.Items);
                _repository.Add(modifiedInvoice);
            }
            _repository.Add(invoice);
        }
        public void Register(int id)
        {
            Client client = _clientRepository.GetByID(id);
            List<Item> listItems = AddItems();
            Invoice invoice = new Invoice
            {
                Type = RegisterTypeFactura(),
                Number = NumberGenerator(),
                Date = DateTime.Now,
                Client = client,
                Items = listItems,
                AmountTotal = CalculateTotalAmount(listItems)
            };

            string input = Reader.ReadChar("Presione X Si quiere modificar la Factura | presione cualquier tecla para cerrar la factura").ToString().ToLower();
            Presentator.WriteLine("");
            if (input == "x")
            { 
                Invoice modifiedInvoice = Update(invoice);
                modifiedInvoice.AmountTotal = CalculateTotalAmount(modifiedInvoice.Items);
                _repository.Add(modifiedInvoice);
            }
            _repository.Add(invoice);
        }
        public Invoice Update(Invoice modifyInvoice)
        {
             // Invoice modifyInvoice = _repository.GetByIdWithIncludes(id, i => i.Items);
            List<Item> listItems = modifyInvoice.Items;
            if (modifyInvoice == null) {
                Presentator.WriteLine($"No se encontro una factura con esa ID");
                // Validar
            }
            Client comprador = modifyInvoice.Client;
            Presentator.WriteLine($"1) {comprador.LegalName}");
            Presentator.WriteLine($"2) {modifyInvoice.Type}");

            if (modifyInvoice.Items != null && modifyInvoice.Items.Any())
            {
                Presentator.WriteLine($"3) Items: ");
                foreach (var item in listItems)
                {
                    Presentator.WriteLine($"Nombre: {item.Description}\nPrecio: ${item.Price} \nCantidad: {item.Quantity}");
                }
            }
            int input = Reader.ReadInt("Ingrese una opcion");
            if (input == 1) {
                var newClient = _clientRepository.GetByID(Reader.ReadInt("Ingrese el ID del Cliente para asignarle esta factura"));
                modifyInvoice.Client = newClient;
                modifyInvoice.ClientId = newClient.Id;
            }
            if (input == 2) modifyInvoice.Type = RegisterTypeFactura();
            if (input == 3) {
                _itemService.UpdateItem(modifyInvoice.Items);
                modifyInvoice.AmountTotal = CalculateTotalAmount(modifyInvoice.Items);
            }
            return modifyInvoice;
        }
        private string RegisterTypeFactura()
        {
            string invoiceType = default;
            bool isValidInput = false;

            do
            {
                // Present the prompt to the user
                Presentator.WriteLine("Enter the invoice type \n This can be: A, B or C");
                string inputType = Console.ReadLine();

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
            return invoiceType;
            // 'invoiceType' now holds the valid letter ('A', 'B', or 'C').
            Presentator.WriteLine($"Selected invoice type: {invoiceType}");
        }
        // Calculates the total amount.
        private float CalculateTotalAmount(List<Item> items)
        {
            float total = 0;
            foreach (var item in items)
            {
                float price = item.Price;
                float quantity = item.Quantity;
                total = total + (item.GetPrice() * item.GetQuantity());
            }
            return total;
        }
        // Add Items until users press something diff than x and associates to an invoice
        private List<Item> AddItems()
        {
            float totalAmount = 0;
            List<Item> itemList = new List<Item>();
            while (true)
            {
                var item = _itemService.ItemRegister();
                itemList.Add(item);

                char choice = Reader.ReadChar("Presione X para agregar mas | Cualquier otra tecla para finalizar");
                if (choice != 'X' && choice != 'x')
                {
                    break; // Salimos del bucle
                }
            }
            return itemList;
        }

        // Calculates the IVA and TotalAmount per separates
        /* private void TotalTypeA()
        {
            float IVA = this.Invoice.AmountTotal / (1 + 21);
            float DiscriminatedTotal = this.Invoice.AmountTotal - IVA;
            Console.WriteLine($"Total:{DiscriminatedTotal} | IVA: {IVA}");
        } */
        // Generate an Invoices Number who follow a determinated format
        private static string NumberGenerator(bool setDate = false)
        {
            string NumberGenerated = "";
            Random rnd = new Random();
            if (!setDate) {
                NumberGenerated = DateTime.Now.ToString("ddd") + DateTime.Now.Year + "-" + rnd.Next(10000, 99999);
            }
            else {
                DateTime date = Reader.ReadDate("Ingrese la fecha en la que creó la factura");
                NumberGenerated = date.ToString("ddd") + date.Year + "-" + rnd.Next(10000, 99999);
            }
            return NumberGenerated;
        }
    }
}
