using Microsoft.EntityFrameworkCore;
using Parcial3.Interfaces;
using Parcial3.Modules;
using Parcial3.Modules.Repositorys;
using Parcial3.Server;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Parcial3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            context.Database.Migrate();
            Client.context = context;
            CrudService<Client> crudClient = new CrudService<Client>(new Repositories<Client>(context));

            InvoiceService InvServ = new InvoiceService(new Repositories<Invoice>(context));
            InvServ.Search(2);
            InvServ.test();
        }
    }
}
