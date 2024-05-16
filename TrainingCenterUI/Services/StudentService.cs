using System.Net.Http.Json;
using TrainingCenterUI.DTO;

namespace TrainingCenterUI.Services
{
    public class StudentService
    {
        private readonly HttpClient _httpClient;

        public StudentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<StudentDTO?>?> GetStudentsAsync()
        {
            var response = await _httpClient.GetAsync("api/students");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<StudentDTO?>>();
            }
            else
            {
                return new List<StudentDTO?>();
            }
        } 

        public async Task<StudentDTO?> GetStudentAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/students/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<StudentDTO>();
            }
            else
            {
                return null;
            }
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
