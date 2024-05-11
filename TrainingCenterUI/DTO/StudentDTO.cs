namespace TrainingCenterUI.DTO
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Governorate { get; set; }
        public required DateTime BirthDate { get; set; }
    }
}
