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

        public async Task<string> AddStudentAsync(StudentDTO student)
        {
            var response = await _httpClient.PostAsJsonAsync("api/students", student);
            if (response.IsSuccessStatusCode)
            {
                return "Student added successfully.";
            }
            else
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> UpdateStudentAsync(int id, StudentDTO student)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/students/{id}", student);
            if (response.IsSuccessStatusCode)
            {
                return "Student updated successfully.";
            }
            else
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task DeleteStudentAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/students/{id}");
        }
    }
}
