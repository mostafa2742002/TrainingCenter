using System.Net.Http.Json;
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

        public async Task<List<CourseDTO>> GetCoursesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CourseDTO>>("api/courses");
        }

        public async Task<CourseDTO> GetCourseAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<CourseDTO>($"api/courses/{id}");
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
