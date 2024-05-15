using System.Net.Http.Json;
using TrainingCenterUI.DTO;

namespace TrainingCenterUI.Services
{
    public class StudentCoursesService
    {
        private readonly HttpClient _httpClient;

        public StudentCoursesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<StudentCourseDTO>> GetStudentCoursesAsync(int studentId)
        {
            return await _httpClient.GetFromJsonAsync<List<StudentCourseDTO>>($"api/students/{studentId}/courses");
        }

        public async Task<StudentCourseDTO> GetStudentCourseAsync(int studentId, int courseId)
        {
            return await _httpClient.GetFromJsonAsync<StudentCourseDTO>($"api/students/{studentId}/courses/{courseId}");
        }

        public async Task AddStudentCourseAsync(int studentId, StudentCourseDTO studentCourse)
        {
            await _httpClient.PostAsJsonAsync($"api/students/{studentId}/courses", studentCourse);
        }

        public async Task UpdateStudentCourseAsync(int studentId, int courseId, StudentCourseDTO studentCourse)
        {
            await _httpClient.PutAsJsonAsync($"api/students/{studentId}/courses/{courseId}", studentCourse);
        }

        public async Task DeleteStudentCourseAsync(int studentId, int courseId)
        {
            await _httpClient.DeleteAsync($"api/students/{studentId}/courses/{courseId}");
        }
    }
}
