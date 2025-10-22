using Microsoft.EntityFrameworkCore;
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
            Invoice.Register();
        }
    }
}
