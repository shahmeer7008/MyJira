using Jira_2._0.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Jira_2._0.Models
{
    [Table("ProjectsData")]
    public class ProjectModelWrapper
    {
        [Key]
        public int ProjectID { get; set; }
        public ProjectModel ProjectData { get; set; }

        // Serialization methods if needed
        public string Serialize()
        {
            return JsonSerializer.Serialize(ProjectData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });
        }

        public static ProjectModelWrapper Deserialize(int projectId, string jsonData)
        {
            try
            {
                // Configure serializer options
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, // Makes property matching case-insensitive
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                // Deserialize using System.Text.Json
                var projectData = JsonSerializer.Deserialize<ProjectModel>(jsonData, options);

                return new ProjectModelWrapper
                {
                    ProjectID = projectId,
                    ProjectData = projectData ?? new ProjectModel() // Fallback to empty object if null
                };
            }
            catch (JsonException ex)
            {
                // More specific exception handling for JSON errors
                Console.WriteLine($"JSON Error in project {projectId}: {ex.Message}");
                Console.WriteLine($"Error at position {ex.BytePositionInLine}");
                Console.WriteLine($"Problematic JSON: {jsonData}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error deserializing project {projectId}: {ex.Message}");
            }

            // Return a default object with the known project ID
            return new ProjectModelWrapper
            {
                ProjectID = projectId,
                ProjectData = new ProjectModel() // Empty/default object
            };
        }
    }
}