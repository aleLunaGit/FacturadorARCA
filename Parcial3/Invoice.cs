using Microsoft.Identity.Client;
using Parcial3.Interfaces;
using Parcial3.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Parcial3
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
        [NotMapped]
        public Enum TyposFactura {  get; set; }



        public Invoice() { 
            Items = new List<Item>();
        }

        public Invoice(int id, string type, Client client)
        {
            Id = id;
            Type = type.ToUpper(); ;
            Number = NumberGenerator();
            Date = DateTime.Now;
            CalculateTotalAmount();
            Client = client;
            Items = AddItems();
        }

       


        // Read the a invoice chossen by the user
        public static void Search()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            Console.WriteLine("Ingrese la ID");
            int id = int.Parse(Console.ReadLine());
            Invoice invoice= context.Invoices.Find(id);
            invoice.ShowPreviewInvoice();
        }
        // Register the invoice and associates to a client
        public static void Register()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            Console.WriteLine("Ingrese el id del cliente");
            int id = int.Parse(Console.ReadLine());
            Client comprador = context.Clients.Find(id);
            Console.WriteLine("Ingrese tipo de Factura: A - B - C");
            string type = Console.ReadLine();
            Invoice facturaA = new Invoice
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
            if (pressed == 'X' || pressed == 'x') {
                facturaA.ShowPreviewInvoice();
            }else facturaA.CalculateTotalAmount();
                context.Invoices.Add(facturaA);
            context.SaveChanges();
        }
        // Calculates the total amount.
        private void CalculateTotalAmount() {
            float sumatorio = 0;
            foreach (var item in Items) {
                float price = item.Price;
                float quantity = item.Quantity;
                sumatorio = sumatorio + (item.GetPrice() * item.GetQuantity());
            }
            AmountTotal = sumatorio;
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
                Item item = new Item { 
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
        public void ShowPreviewInvoice() {
            Console.WriteLine($"" +
                $"\nFactura tipo: {this.Type}   " +
                $"|    Fecha: {this.Date}\n" +
                $"{Client.LegalName}\n" +
                $"{Client.CuitCuil}\n" +
                $"{Client.Address}");
            CalculateTotalAmount();
            // If Invoice type = A show in console the total amount, else show the iva discriminated
            foreach(var item in Items)
            {
                Console.WriteLine($"Producto: {item.Description}\n{item.Quantity} * {item.Price} {item.Quantity * item.Price}");
            }
            Console.WriteLine($"Total: $r{AmountTotal}");
        }

        // Calculates the IVA and TotalAmount per separates
        private void TotalTypeA()
        { 
            ShowPreviewInvoice();
            float IVA = AmountTotal / (1 + 21);
            float DiscriminatedTotal = AmountTotal - IVA;
            Console.WriteLine($"Total:{DiscriminatedTotal} | IVA: {IVA}");
        }
        // Generate an Invoices Number who follow a determinated format
        private static string NumberGenerator()
        {
            Random rnd = new Random();
            return DateTime.Now.ToString("ddd") + DateTime.Now.Year + "-" + rnd.Next(10000,99999);
        }
        public string GetType() => this.Type;

        public string GetNumber() => this.Number;

        public DateTime GetDate() => this.Date;

        public float GetAmountTotal() => this.AmountTotal;

        public void SetType(string type)=> this.Type = type;

        public void SetNumber()=> this.Number = NumberGenerator();

        public void SetDate(DateTime date)=> this.Date = date;

        public void SetAmountTotal(float amountTotal)=> this.AmountTotal = amountTotal;
    }
}
