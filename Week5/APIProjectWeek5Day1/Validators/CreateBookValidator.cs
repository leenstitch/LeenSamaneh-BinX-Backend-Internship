/*
   this file is for validating the CreateBookDto using FluentValidation.
   It ensures that the book title is not empty, does not exceed 100 characters,
   the price is greater than 0, the quantity is non-negative, and the AuthorId is greater than 0.
*/
using APIProject.Dto_s.BookDto_s.BookDto_sWeek3;
using FluentValidation;

namespace APIProject.Validators
{
    /*
       This class defines a validator for the CreateBookDto using FluentValidation.
       abstract validator is a base class provided by FluentValidation
       that allows you to define validation rules for a specific type.
    */
    public class CreateBookValidator : AbstractValidator<CreateBookDto>
    {
        public CreateBookValidator()
        {
            // rule for validating the Title property of CreateBookDto
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Book title is required.")
                .MaximumLength(100)
                .WithMessage("Book title cannot exceed 100 characters.");

            // rule for validating the Price property of CreateBookDto
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Book price must be greater than 0.");

            // rule for validating the Quantity property of CreateBookDto
            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Book quantity cannot be negative.");

            // rule for validating the AuthorId property of CreateBookDto
            RuleFor(x => x.AuthorId)
                .GreaterThan(0)
                .WithMessage("AuthorId must be greater than 0.");
        }
    }
}
