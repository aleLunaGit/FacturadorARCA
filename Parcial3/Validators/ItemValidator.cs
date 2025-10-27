using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parcial3.Validators
{
    public class ItemValidator
    {
        public ValidationResult ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return ValidationResult.Failure("La descripción no puede estar vacía.");
            }

            if (description.Length < 3)
            {
                return ValidationResult.Failure("La descripción debe tener al menos 3 caracteres.");
            }

            if (description.Length > 200)
            {
                return ValidationResult.Failure("La descripción no puede exceder 200 caracteres.");
            }

            // Validar que no contenga solo números
            if (Regex.IsMatch(description, @"^\d+$"))
            {
                return ValidationResult.Failure("La descripción no puede contener solo números.");
            }

            return ValidationResult.Success();
        }

        public ValidationResult ValidateQuantity(float quantity)
        {
            if (quantity <= 0)
            {
                return ValidationResult.Failure("La cantidad debe ser mayor a 0.");
            }

            if (quantity > 10000)
            {
                return ValidationResult.Failure("La cantidad no puede exceder 10,000 unidades.");
            }

            return ValidationResult.Success();
        }

        public ValidationResult ValidatePrice(float price)
        {
            if (price <= 0)
            {
                return ValidationResult.Failure("El precio debe ser mayor a 0.");
            }

            if (price > 999999.99f)
            {
                return ValidationResult.Failure("El precio no puede exceder $999,999.99");
            }

            // Validar que no tenga más de 2 decimales
            if (Math.Round(price, 2) != price)
            {
                return ValidationResult.Failure("El precio solo puede tener hasta 2 decimales.");
            }

            return ValidationResult.Success();
        }

        public ValidationResult ValidateItem(Item item)
        {
            var descResult = ValidateDescription(item.Description);
            if (!descResult.IsValid)
                return descResult;

            var qtyResult = ValidateQuantity(item.Quantity);
            if (!qtyResult.IsValid)
                return qtyResult;

            var priceResult = ValidatePrice(item.Price);
            if (!priceResult.IsValid)
                return priceResult;

            return ValidationResult.Success();
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; private set; }
        public string ErrorMessage { get; private set; }

        private ValidationResult(bool isValid, string errorMessage = "")
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        public static ValidationResult Success() => new ValidationResult(true);

        public static ValidationResult Failure(string errorMessage) => new ValidationResult(false, errorMessage);
    }
}
