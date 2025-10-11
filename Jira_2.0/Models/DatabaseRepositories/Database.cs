using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Text.Json;

namespace Jira_2._0.Models.DatabaseRepositories
{
    public class Database
    {
        public Database() { }
        public SqlConnection makeConnection()
        {
            try
            {
                string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;
                                            Initial Catalog=MyJira;
                                            Integrated Security=True;Connect Timeout=30;
                                            Encrypt=True;Trust Server Certificate=False;
                                            Application Intent=ReadWrite;
                                            Multi Subnet Failover=False";
                SqlConnection connection = new SqlConnection(connectionString);

                return connection;
            }
            catch (Exception e) { Console.WriteLine(e.Message); return null; }
        }

        //This function will take sql command and connection obj and form a sqlCommand
        public SqlCommand makeCommand(string sql, SqlConnection connection)
        {
            try
            {
                SqlCommand command = new SqlCommand(sql, connection);

                connection.Open();
                return command;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }
        //In order to avoid repetion in Insert,Delete,Update functions, this function is implemented to reuse
        public bool ManipulateQuery(SqlCommand command)
        {
            try
            {
                
                int count = command.ExecuteNonQuery();
                Console.WriteLine(count);
                if (count > 0) return true;

                return false;
            }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        //In order to avoid repetion in functions using Select query , this function is implemnted
        public void DisplayQuery(SqlCommand command)
        {
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                int columns = reader.FieldCount;
                for (int i = 0; i < columns; i++)
                {
                    string columnName = reader.GetName(i);
                    Console.Write(columnName + "\t");
                }
                Console.WriteLine();

                while (reader.Read())
                {
                    for (int i = 0; i < columns; i++)
                    {
                        Console.Write(reader.GetValue(i) + "\t");
                    }
                    Console.WriteLine();

                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); return; }
        }
        public List<ProjectModel> Read(SqlCommand command)
        {
            var projects = new List<ProjectModel>();
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                

                while (reader.Read())
                {
                   
                    string jsonData = reader["Data"].ToString();

                    try
                    {
                        var project = JsonSerializer.Deserialize<ProjectModel>(jsonData, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        if (project != null)
                        {
                            
                            projects.Add(project);

                        }
                    }
                    catch (JsonException ex)
                    {
                        // Log deserialization error
                        Console.WriteLine($"Error deserializing project ID {reader["ProjectID"]}: {ex.Message}");
                        // Continue processing other projects

                    }

                }
                return projects;

            }
            catch (Exception e) { Console.WriteLine(e.Message); return null; }
        }


      
        //A common reuseable function to close connection
        public void CloseConnection(SqlConnection connection)
        {
            try
            {
                connection.Close();

            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

    }
}
