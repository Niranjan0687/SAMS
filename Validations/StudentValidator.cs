using FluentValidation;
using SmsAPI.Models;

namespace SmsAPI.Validations
{
    public class StudentValidator: AbstractValidator<Student>
    {
        public StudentValidator()
        {
            RuleFor(s => s.FullName)
                .NotEmpty().WithMessage("Full Name is required.")
                .MaximumLength(100).WithMessage("Full Name cannot exceed 100 characters.");

            RuleFor(s => s.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(s => s.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .LessThan(DateTime.Now).WithMessage("Date of Birth must be in the past.");

            //RuleFor(s => s.Course)
            //    .NotEmpty().WithMessage("Course is required.")
            //    .MaximumLength(50).WithMessage("Course cannot exceed 50 characters.");

            RuleFor(s => s.AdmissionDate)
                .NotEmpty().WithMessage("Admission Date is required.")
                .GreaterThan(s => s.DateOfBirth).WithMessage("Admission Date must be after Date of Birth.");
        }
    }
}
