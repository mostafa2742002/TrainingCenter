﻿using System.Net.Http.Json;
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

        public async Task<List<CourseDTO>> GetCoursesAsync()
        {
            var response = await _httpClient.GetAsync("api/courses");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<List<CourseDTO>>();
                }
                catch (JsonException ex)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Invalid JSON received: {responseBody}");
                    throw new InvalidOperationException("Received invalid JSON", ex);
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error fetching courses: {response.StatusCode} - {errorContent}");
            }
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
