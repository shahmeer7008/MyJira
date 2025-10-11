using System.Text.Json.Serialization;
using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Jira_2._0.Models
{
    [Table("Issues")]
    public class IssueModelWrapper
    {
        [Key]
        public  int IssueID {  get; set; }
        public IssueModel IssueData{  get; set; }

        public string Serialize()
        {
            return JsonSerializer.Serialize(IssueData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });
        }

        public static IssueModelWrapper Deserialize(int issueId, string jsonData)
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
                var IssueData = JsonSerializer.Deserialize<IssueModel>(jsonData, options);
               
                return new IssueModelWrapper
                {
                    IssueID = issueId,
                    IssueData = IssueData ?? new IssueModel() // Fallback to empty object if null
                };
            }
            catch (JsonException ex)
            {
                // More specific exception handling for JSON errors
                Console.WriteLine($"JSON Error in Issue {issueId}: {ex.Message}");
                Console.WriteLine($"Error at position {ex.BytePositionInLine}");
                Console.WriteLine($"Problematic JSON: {jsonData}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error deserializing project {issueId}: {ex.Message}");
            }

            // Return a default object with the known project ID
            return new IssueModelWrapper
            {
                IssueID= issueId,
                IssueData = new IssueModel() // Empty/default object
            };
        }
    }
}
