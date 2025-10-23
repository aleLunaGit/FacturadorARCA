using Parcial3.Interfaces;
using Parcial3.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3.Modules.Repositorys
{
    internal class InvoiceService : CrudService<Invoice>
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private readonly IRepository<Invoice> repository;
        public Invoice Invoice { get; set; }
        public InvoiceService (IRepository<Invoice> entity) : base(entity)
        {
        }

        Repositories<Invoice> RepoInvoice = new Repositories<Invoice>(
                new ApplicationDbContext()
            );

        // Read a invoice chossen by the user
        public void Search(int id)
        {
            this.Invoice = repository.GetByID(id);
            ShowPreviewInvoice();
        }
        // Register the invoice and associates to a client
        public override void Register()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            Console.WriteLine("Ingrese el id del cliente");
            int id = int.Parse(Console.ReadLine());
            Client comprador = context.Clients.Find(id);
            Console.WriteLine("Ingrese tipo de Factura: A - B - C");
            string type = Console.ReadLine();
            this.Invoice = new Invoice
            {
                Type = type.ToUpper(),
                Number = NumberGenerator(),
                Date = DateTime.Now,
                Client = comprador,
                Items = AddItems()
            };
            Console.WriteLine("Desea ver una vista previa de la factura?");
            Console.WriteLine("Presione X para ver la vista previa");
            Console.WriteLine("Presione cualquier tecla para omitir");
            char pressed = Console.ReadKey().KeyChar;
            if (pressed == 'X' || pressed == 'x')
            {
                ShowPreviewInvoice();
            }
            else CalculateTotalAmount();
        }
        // Calculates the total amount.
        private void CalculateTotalAmount()
        {
            float sumatorio = 0;
            foreach (var item in this.Invoice.Items)
            {
                float price = item.Price;
                float quantity = item.Quantity;
                sumatorio = sumatorio + (item.GetPrice() * item.GetQuantity());
            }
            this.Invoice.AmountTotal = sumatorio;
        }
        // Add Items until users press something diff than x and associates to an invoice
        private static List<Item> AddItems()
        {
            float totalAmount = 0;
            bool addItem = false;
            List<Item> itemList = new List<Item>();
            while (!addItem)
            {
                Console.WriteLine("Ingrese la descripcion del producto");
                string description = Console.ReadLine();
                Console.WriteLine("Ingrese el precio del producto");
                float quantity = float.Parse(Console.ReadLine());
                Console.WriteLine("Ingrese la cantidad del producto");
                float price = float.Parse(Console.ReadLine());
                Item item = new Item
                {
                    Description = description,
                    Quantity = quantity,
                    Price = price
                };
                totalAmount = totalAmount + (quantity * price);
                Console.WriteLine("Presione X para agregar mas objetos");
                Console.WriteLine("Presione cualquier tecla para dejar de agregar objetos");
                char pressed = Console.ReadKey().KeyChar;
                if (pressed != 'X' && pressed != 'x') addItem = true;
                itemList.Add(item);
            }
            return itemList;
        }
        // ShowInMenu shows the invoice information in console.
        public void ShowPreviewInvoice()
        {
            Console.WriteLine($"" +
                $"\nFactura tipo: {this.Invoice.Type}   " +
                $"|    Fecha: {this.Invoice.Date}\n" +
                $"{this.Invoice.Client.LegalName}\n" +
                $"{this.Invoice.Client.CuitCuil}\n" +
                $"{this.Invoice.Client.Address}");
            CalculateTotalAmount();
            // If Invoice type = A show in console the total amount, else show the iva discriminated
            foreach (var item in this.Invoice.Items)
            {
                Console.WriteLine($"Producto: {item.Description}\n{item.Quantity} * {item.Price} {item.Quantity * item.Price}");
            }
            Console.WriteLine($"Total: $r{this.Invoice.AmountTotal}");
        }
        public void test()
        {
            Console.WriteLine("Cliente desde invoice " + this.Invoice.Client.LegalName);
        }

        // Calculates the IVA and TotalAmount per separates
        private void TotalTypeA()
        {
            ShowPreviewInvoice();
            float IVA = this.Invoice.AmountTotal / (1 + 21);
            float DiscriminatedTotal = this.Invoice.AmountTotal - IVA;
            Console.WriteLine($"Total:{DiscriminatedTotal} | IVA: {IVA}");
        }
        // Generate an Invoices Number who follow a determinated format
        private static string NumberGenerator()
        {
            Random rnd = new Random();
            return DateTime.Now.ToString("ddd") + DateTime.Now.Year + "-" + rnd.Next(10000, 99999);
        }
    }
}
