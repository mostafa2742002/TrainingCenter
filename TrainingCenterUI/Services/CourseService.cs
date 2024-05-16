using System.Net.Http.Json;
using System.Text.Json;
using TrainingCenterUI.DTO;

namespace TrainingCenterUI.Services
{
    public class CourseService
    {
        private readonly HttpClient _httpClient;

        public CourseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CourseDTO>?> GetCoursesAsync()
        {
            var response = await _httpClient.GetAsync("api/courses");
            if (response.IsSuccessStatusCode)
            {
                    return await response.Content.ReadFromJsonAsync<List<CourseDTO>>();   
            }
            else
            {
                return new List<CourseDTO>();
            }
        }

        public async Task<List<CourseDTO?>?> GetCoursesAvailableForStudentAsync(int studentId)
        {
            var response = await _httpClient.GetAsync($"api/courses/available/{studentId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CourseDTO>>();
            }
            else
            {
                return new List<CourseDTO>();
            }
        }

        public async Task<CourseDTO?> GetCourseAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/courses/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CourseDTO>();
            }
            else
            {
                return null;
            }
        }

        public async Task AddCourseAsync(CourseDTO course)
        {
            await _httpClient.PostAsJsonAsync("api/courses", course);
        }

        public async Task UpdateCourseAsync(int id, CourseDTO course)
        {
            await _httpClient.PutAsJsonAsync($"api/courses/{id}", course);
        }

        public async Task DeleteCourseAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/courses/{id}");
        }
    }
}
