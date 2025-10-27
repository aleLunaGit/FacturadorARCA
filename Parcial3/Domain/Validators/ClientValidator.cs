using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Parcial3.Domain.Implementations;

namespace Parcial3.Domain.Validators
{
    public static class ClientValidator
    {
        public static ValidationResult Validate(Client client)
        {
            if(client == null)
            {
                return ValidationResult.Failure("No se puede");
            }
            // Regla 1: Nombre Legal (LegalName)
            if (string.IsNullOrWhiteSpace(client.LegalName))
            {
                return ValidationResult.Failure("La Razón Social es obligatoria.");
            }
            if (client.LegalName.Length < 3)
            {
                return ValidationResult.Failure("La Razón Social debe tener al menos 3 caracteres.");
            }

            // Regla 2: Domicilio (Address)
            if (string.IsNullOrWhiteSpace(client.Address))
            {
                return ValidationResult.Failure("El Domicilio es obligatorio.");
            }

            // Regla 3: CUIT/CUIL
            var cuitValidation = ValidateCuit(client.CuitCuil);
            if (!cuitValidation.IsValid)
            {
                return cuitValidation; // Devuelve el error específico del CUIT
            }

            // Si todo está bien
            return ValidationResult.Success();
        }
        // Puedes crear métodos privados para reglas complejas
        private static ValidationResult ValidateCuit(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit))
            {
                return ValidationResult.Failure("El CUIT/CUIL es obligatorio.");
            }

            // Limpia el CUIT de guiones o espacios
            var cuitLimpio = Regex.Replace(cuit, @"[^\d]", "");

            if (cuitLimpio.Length != 11)
            {
                return ValidationResult.Failure("El CUIT/CUIL debe tener 11 dígitos.");
            }
            if (!long.TryParse(cuitLimpio, out _))
            {
                return ValidationResult.Failure("El CUIT/CUIL debe contener solo números.");
            }

            // Aquí podrías agregar el algoritmo completo de Módulo 11 si quisieras

            return ValidationResult.Success();
        }
    }
}
