/*
  this code is for a validator class that uses FluentValidation
  to validate the properties of an UpdateBookDto object.
  It checks that the Title is not empty and does not exceed 100 characters,
  the Price is greater than 0,
  the Quantity is not negative, and the AuthorId is greater than 0.
*/
using APIProject.Dto_s.BookDto_s.BookDto_sWeek3;
using FluentValidation;

namespace APIProjectWeek4Day4.Validators
{

    /*
       This class defines a validator for the UpdateBookDto using FluentValidation.
       abstract validator is a base class provided by FluentValidation
       that allows you to define validation rules for a specific type.
    */
    public class UpdateBookValidator : AbstractValidator<UpdateBookDto>
    {
        public UpdateBookValidator()
        {
            //rule for validating the Title property of the UpdateBookDto
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Book title is required.")
                .MaximumLength(100)
                .WithMessage("Book title cannot exceed 100 characters.");

            //rule for validating the Price property of the UpdateBookDto
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Book price must be greater than 0.");
            
            //rule for validating the Quantity property of the UpdateBookDto
            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Book quantity cannot be negative.");

            //rule for validating the AuthorId property of the UpdateBookDto
            RuleFor(x => x.AuthorId)
                .GreaterThan(0)
                .WithMessage("AuthorId must be greater than 0.");
        }
    }
}