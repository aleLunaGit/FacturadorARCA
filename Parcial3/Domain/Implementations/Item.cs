using Parcial3.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Parcial3.Domain.Implementations
{
    public class Item : IItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        private string _description;
        private float _quantity;
        private float _price;

        [Required]
        public string Description
        {
            get => _description;
            set
            {
                ValidateDescription(value);
                _description = value;
            }
        }

        [Required]
        public float Quantity
        {
            get => _quantity;
            set
            {
                ValidateQuantity(value);
                _quantity = value;
            }
        }

        [Required]
        public float Price
        {
            get => _price;
            set
            {
                ValidatePrice(value);
                _price = value;
            }
        }

        public Invoice Invoice { get; set; }
        public int InvoiceId { get; internal set; }

        public Item() { }

        public Item(string description, float quantity, float price)
        {
            Description = description;  // Usa el setter que valida
            Quantity = quantity;        // Usa el setter que valida
            Price = price;              // Usa el setter que valida
        }

        //  VALIDACIONES 

        private void ValidateDescription(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("ERROR: La descripción no puede estar vacía.");
            }

            // Validación 1 Longitud mínima
            if (value.Trim().Length < 3)
            {
                throw new ArgumentException("ERROR: La descripción debe tener al menos 3 caracteres.");
            }

            // Validación 2 Longitud máxima
            if (value.Length > 200)
            {
                throw new ArgumentException("ERROR: La descripción no puede exceder los 200 caracteres.");
            }

            // Validación 3 No solo números
            if (value.All(char.IsDigit))
            {
                throw new ArgumentException("ERROR: La descripción no puede contener solo números.");
            }

            // Validación 4 Caracteres especiales no permitidos
            char[] caracteresNoPermitidos = { '<', '>', '|', '\\', '/', '"' };
            if (value.Any(c => caracteresNoPermitidos.Contains(c)))
            {
                throw new ArgumentException("ERROR: La descripción contiene caracteres no permitidos (< > | \\ / \").");
            }
        }

        private void ValidateQuantity(float value)
        {
            // Validación 1 No puede ser cero o negativo
            if (value <= 0)
            {
                throw new ArgumentException("ERROR: La cantidad debe ser un número positivo mayor a cero.");
            }

            // Validación 2 Límite máximo razonable
            if (value > 999999)
            {
                throw new ArgumentException("ERROR: La cantidad es excesivamente alta. Máximo permitido: 999,999.");
            }

            // Validación 3 Máximo 2 decimales
            decimal decimalValue = (decimal)value;
            decimal rounded = Math.Round(decimalValue, 2);
            if (decimalValue != rounded)
            {
                throw new ArgumentException("ERROR: La cantidad no puede tener más de 2 decimales.");
            }
        }

        private void ValidatePrice(float value)
        {
            // Validación 1 No puede ser cero o negativo
            if (value <= 0)
            {
                throw new ArgumentException("ERROR: El precio debe ser un número positivo mayor a cero.");
            }

            // Validación 2 Precio mínimo razonable
            if (value < 0.01f)
            {
                throw new ArgumentException("ERROR: El precio debe ser mayor o igual a $0.01");
            }

            // Validación 3 Precio máximo razonable
            if (value > 99999999.99f)
            {
                throw new ArgumentException("ERROR: El precio es excesivamente alto. Máximo: $99,999,999.99");
            }

            // Validación 4 Máximo 2 decimales
            decimal decimalValue = (decimal)value;
            decimal rounded = Math.Round(decimalValue, 2);
            if (decimalValue != rounded)
            {
                throw new ArgumentException("ERROR: El precio no puede tener más de 2 decimales.");
            }
        }

        //  MÉTODOS DE INTERFAZ 

        public string GetDescription() => Description;

        public float GetQuantity() => Quantity;

        public float GetPrice() => Price;

        public void SetDescription(string description) => Description = description;

        public void SetQuantity(float quantity) => Quantity = quantity;

        public void SetPrice(float price) => Price = price;
    }
}