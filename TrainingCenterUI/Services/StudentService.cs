using TrainingCenterUI.DTO;
using System.Net.Http.Json;
namespace TrainingCenterUI.Services
{
    public class StudentService
    {
        private readonly HttpClient _httpClient;

        public StudentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<List<StudentDTO>> GetStudentsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<StudentDTO>>("api/students") ?? new List<StudentDTO>();
        }

        public async Task<StudentDTO> GetStudentAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<StudentDTO>($"api/students/{id}");
        }

        public async Task AddStudentAsync(StudentDTO student)
        {
            await _httpClient.PostAsJsonAsync("api/students", student);
        }

        public async Task UpdateStudentAsync(int id, StudentDTO student)
        {
            await _httpClient.PutAsJsonAsync($"api/students/{id}", student);
        }

        public async Task DeleteStudentAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/students/{id}");
        }

    }
}
