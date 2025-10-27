using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class ValidationResult
    {
        public bool IsValid { get; private set; }
        public string ErrorMessage { get; private set; }

        //Constructor privado
        private ValidationResult(bool isValid, string errorMessage)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        //Metodo para resultado favorable
        public static ValidationResult Success()
        {
            return new ValidationResult(true, null);
        }

        //metodo para resultado fallido
        public static ValidationResult Fail(string errorMessage)
        {
            return new ValidationResult(false, errorMessage);
        }

    }



