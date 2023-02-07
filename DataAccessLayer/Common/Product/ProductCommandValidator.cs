using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Common.Product
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(c => c.Name)
                    .NotEmpty()
                    .WithErrorCode("Required")
                    .WithMessage("Name field should not be empty")
                    .Must(MustBeCharacters)
                    .WithErrorCode("MustBeCharacters")
                    .WithMessage("Enter a valid name");
            RuleFor(c => c.Description).NotEmpty();

        }

        private bool MustBeCharacters(string name)
        {
            bool isValid = Regex.IsMatch(name, "^[a-zA-Z0-9]{2,}([\\s][a-zA-Z0-9]+)*$");
            return isValid;
        }
    }
}
