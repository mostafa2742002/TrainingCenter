using System;
using System.ComponentModel.DataAnnotations;

namespace TrainingCenterUI.DTO
{
    public class CourseDTO
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [CustomValidation(typeof(CourseDTO), nameof(ValidateStartDate))]
        public DateTime StartDate { get; set; }

        [Required]
        [CustomValidation(typeof(CourseDTO), nameof(ValidateEndDate))]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(1, 50, ErrorMessage = "Capacity must be between 1 and 50.")]
        public int Capacity { get; set; }

        [Required]
        public decimal Cost { get; set; }

        [Required]
        public string Status { get; set; }

        public static ValidationResult ValidateStartDate(DateTime startDate, ValidationContext context)
        {
            if (startDate < DateTime.Today)
            {
                return new ValidationResult("Start date cannot be in the past.");
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidateEndDate(DateTime endDate, ValidationContext context)
        {
            var instance = context.ObjectInstance as CourseDTO;
            if (instance != null && endDate <= instance.StartDate)
            {
                return new ValidationResult("End date must be after the start date.");
            }

            return ValidationResult.Success;
        }
    }
}
