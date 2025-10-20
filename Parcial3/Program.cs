using Microsoft.EntityFrameworkCore;
using Parcial3.Server;

namespace Parcial3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            context.Database.Migrate();
            Client.context = context;
            Client.Register();
        }
    }
}
