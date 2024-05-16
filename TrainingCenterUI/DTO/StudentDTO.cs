using System;
using System.ComponentModel.DataAnnotations;

namespace TrainingCenterUI.DTO
{
    public class StudentDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        public string Governorate { get; set; }

        [Required]
        [CustomValidation(typeof(StudentDTO), nameof(ValidateBirthDate))]
        public DateTime BirthDate { get; set; }

        public static ValidationResult ValidateBirthDate(DateTime birthDate, ValidationContext context)
        {
            if (birthDate >= DateTime.Today)
            {
                return new ValidationResult("Birth date must be in the past.");
            }

            return ValidationResult.Success;
        }
    }
}
