// En FacturadorARCA.Tests/InvoiceServiceTests.cs

using Xunit;
using Moq;
using Parcial3.Interfaces;
using Parcial3.Modules.Services; // O el namespace correcto
using Parcial3.Server;
using Parcial3.Modules.Repositorys;
using Parcial3;
using System.Collections.Generic;

public class InvoiceServiceTests
{
    // --- Miembros privados para guardar nuestros mocks ---
    private readonly Mock<IRepository<Invoice>> _mockInvoiceRepo;
    private readonly Mock<IRepository<Client>> _mockClientRepo;
    private readonly Mock<ApplicationDbContext> _mockDbContext;
    private readonly InvoiceService _invoiceService;

    // --- Constructor de la clase de pruebas ---
    // Este constructor se ejecuta ANTES de cada prueba, dándonos un entorno limpio.
    public InvoiceServiceTests()
    {
        // Creamos los "mocks" o simuladores de nuestras dependencias.
        _mockInvoiceRepo = new Mock<IRepository<Invoice>>();
        _mockClientRepo = new Mock<IRepository<Client>>();

        // Moq no puede mockear DbContext directamente sin algunos trucos.
        // Por ahora, lo mockearemos de forma simple. En un proyecto real,
        // podríamos usar una base de datos en memoria (In-Memory Database).
        _mockDbContext = new Mock<ApplicationDbContext>();

        // Creamos la instancia del servicio que vamos a probar,
        // pero le pasamos nuestros MOCKS en lugar de las dependencias reales.
        _invoiceService = new InvoiceService(
            _mockInvoiceRepo.Object,
            _mockClientRepo.Object,
            _mockDbContext.Object
        );
    }

    // --- LA PRUEBA ---
    [Fact] // Este atributo le dice a xUnit que esto es una prueba unitaria.
    public void CreateNewInvoice_WithValidData_ShouldAddInvoiceAndSaveChanges()
    {
        // --- 1. ARRANGE (Preparar) ---

        // Creamos un cliente falso que nuestro mock del repositorio devolverá.
        var fakeClient = new Client("20-12345678-9", "Cliente de Prueba", "Calle Falsa 123");

        // Creamos el borrador de la factura que le pasaremos al servicio.
        var draftInvoice = new Invoice
        {
            Client = fakeClient,
            Type = "A",
            Items = new List<Item> { new Item("Producto Test", 1, 100) }
        };

        // Configuramos el comportamiento de nuestro mock:
        // "Cuando se llame a GetByID con CUALQUIER entero, devuelve nuestro cliente falso".
        _mockClientRepo.Setup(repo => repo.GetByID(It.IsAny<int>())).Returns(fakeClient);

        // --- 2. ACT (Actuar) ---

        // Llamamos al método que estamos probando. ¡Esta es la única acción!
        _invoiceService.CreateNewInvoice(draftInvoice);

        // --- 3. ASSERT (Afirmar) ---

        // Verificamos que el método Add del repositorio de facturas fue llamado
        // exactamente UNA VEZ con el objeto draftInvoice que le pasamos.
        _mockInvoiceRepo.Verify(repo => repo.Add(draftInvoice), Times.Once);

        // Verificamos que el método SaveChanges de nuestro DbContext fue llamado
        // exactamente UNA VEZ. Esto confirma que nuestra transacción se completó.
        _mockDbContext.Verify(ctx => ctx.SaveChanges(), Times.Once);
    }
    // Dentro de la misma clase InvoiceServiceTests.cs

    [Fact]
    public void CreateNewInvoice_WhenClientDoesNotExist_ShouldThrowInvalidOperationException()
    {
        // --- 1. ARRANGE (Preparar) ---
        var fakeClient = new Client("20-11111111-1", "Cliente Fantasma", "Nuncajamas")
        {
            Id = 1
        };
        // Creamos un borrador con un cliente cualquiera.
        var draftInvoice = new Invoice
        {
            Client = fakeClient,
            Type = "A",
            Items = new List<Item> { new Item("Test", 1, 10) }
        };

        // ¡La configuración clave!
        // "Cuando se llame a GetByID, devuelve null, simulando que el cliente no fue encontrado".
        _mockClientRepo.Setup(repo => repo.GetByID(It.IsAny<int>())).Returns((Client)null);

        // --- 2. ACT & 3. ASSERT (Actuar y Afirmar) ---

        // Usamos Assert.Throws para afirmar que la ejecución del código dentro de la lambda
        // DEBE lanzar una excepción de un tipo específico. Si no la lanza, la prueba falla.
        Assert.Throws<InvalidOperationException>(() =>
        {
            _invoiceService.CreateNewInvoice(draftInvoice);
        });
    }
}
