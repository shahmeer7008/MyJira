using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Text.Json;
using Jira_2._0.Interfaces;
using Microsoft.EntityFrameworkCore;
using Jira_2._0.Models.Context;
namespace Jira_2._0.Models.DatabaseRepositories
{
    public class ProjectRepo:IProjectRepo
    {
        //private readonly Database _database;

        //public ProjectRepo(Database database)
        //{
        //    _database = database;
        //}
        private readonly JiraDBContext _context;



    public ProjectRepo(JiraDBContext context)
        {
            _context = context;
           
        }

        public int Insert(ProjectModelWrapper wrapper)
        {
            try
            {
                // Create a new ProjectData entity



                if (wrapper != null)
                {
                    _context.ProjectsData.Add(wrapper);

                }
                // Add to context and save
                _context.SaveChanges();

                return wrapper.ProjectID;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
            //string query = "INSERT INTO ProjectsData (Data) OUTPUT INSERTED.ProjectID VALUES (@Data)";
            //SqlConnection connection = null;
            //SqlCommand command = null;

            //try
            //{
            //    connection = _database.makeConnection();
            //    if (connection == null) return 0;

            //    command = _database.makeCommand(query, connection);
            //    if (command == null) return 0;

            //    command.Parameters.Add("@Data", SqlDbType.NVarChar).Value = wrapper.Serialize();

            //    return (int)command.ExecuteScalar();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //    return 0;
            //}
            //finally
            //{
            //    if (command != null) command.Dispose();
            //    if (connection != null && connection.State == ConnectionState.Open)
            //    {
            //        _database.CloseConnection(connection);
            //    }
            //}
        }

        public ProjectModelWrapper GetProjectById(int id)
        {
            try
            {
                var projectData = _context.ProjectsData
                    .FirstOrDefault(p => p.ProjectID == id);
                return projectData;
                //if (projectData != null)
                //{
                //    return ProjectModelWrapper.Deserialize(id, projectData.ProjectData);
                //}

                //return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            //    SqlConnection connection = _database.makeConnection();
            //    string query = "SELECT ProjectID, Data FROM ProjectsData WHERE ProjectID = @id";
            //    SqlCommand command = _database.makeCommand(query, connection);
            //    command.Parameters.AddWithValue("@id", id);

            //    try
            //    {
            //        SqlDataReader reader = command.ExecuteReader();
            //        if (reader.Read())
            //        {
            //            int projectId = reader.GetInt32(0);
            //            string jsonData = reader.GetString(1);

            //            return ProjectModelWrapper.Deserialize(projectId, jsonData);
            //        }
            //        return null;
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //        return null;
            //    }
            //    finally
            //    {
            //        _database.CloseConnection(connection);
            //    }
        }

        public bool UpdateProject(ProjectModelWrapper wrapper)
        {

            try
            {
                // Retrieve the existing project record by ID
                var projectData = _context.ProjectsData
                    .FirstOrDefault(p => p.ProjectID == wrapper.ProjectID);

                if (projectData == null)
                {
                    // No existing record found to update
                    return false;
                }

                // Update the serialized data
                projectData.ProjectData = wrapper.ProjectData;


                // Mark the entity as modified (optional, EF Core often tracks this automatically)
                _context.ProjectsData.Update(projectData);

                // Save changes to the database
                return _context.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error updating project: {e.Message}");
                return false;
            }

            //SqlConnection connection = _database.makeConnection();

            //string query = "UPDATE ProjectsData SET Data = @data WHERE ProjectID = @id";
            //SqlCommand command = _database.makeCommand(query, connection);

            //// Correct parameters
            //command.Parameters.AddWithValue("@id", wrapper.ProjectID);
            //command.Parameters.AddWithValue("@data", wrapper.Serialize());

            //try
            //{
            //    return _database.ManipulateQuery(command);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //    return false;
            //}
            //finally
            //{
            //    _database.CloseConnection(connection);
            //}
        }

        public bool DeleteProject(int id)
        {

            try
            {
                var projectData = _context.ProjectsData
                    .FirstOrDefault(p => p.ProjectID == id);

                if (projectData == null)
                {
                    return false;
                }

                // Remove from context
                _context.ProjectsData.Remove(projectData);

                // Save changes
                return _context.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            //    SqlConnection connection = _database.makeConnection();
            //    string query = "DELETE FROM ProjectsData WHERE ProjectID = @id";
            //    SqlCommand command = _database.makeCommand(query, connection);
            //    command.Parameters.AddWithValue("@id", id);

            //    try
            //    {
            //        return _database.ManipulateQuery(command);
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //        return false;
            //    }
            //    finally
            //    {
            //        _database.CloseConnection(connection);
            //    }
        }


        public List<ProjectModelWrapper> GetAllProjects()
        {

            try
            {
                var projects = _context.ProjectsData
                    .OrderBy(p => p.ProjectID)
                    .Select(p => new ProjectModelWrapper
                    {
                        ProjectID = p.ProjectID,
                        ProjectData = p.ProjectData // EF auto-deserializes this
                    })
                    .ToList();

                return projects;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<ProjectModelWrapper>();
            }

            //var projects = new List<ProjectModelWrapper>();
            //string sql = "SELECT ProjectID, Data FROM ProjectsData ORDER BY ProjectID;";

            //SqlConnection connection = null;
            //SqlCommand command = null;

            //try
            //{
            //    connection = _database.makeConnection();
            //    if (connection == null) return projects;

            //    command = _database.makeCommand(sql, connection);
            //    if (command == null) return projects;

            //    SqlDataReader reader = command.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        int projectId = reader.GetInt32(0);
            //        string jsonData = reader.GetString(1);

            //        projects.Add(ProjectModelWrapper.Deserialize(projectId, jsonData));
            //    }

            //    return projects;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //    return projects;
            //}
            //finally
            //{
            //    if (command != null) command.Dispose();
            //    if (connection != null && connection.State == ConnectionState.Open)
            //    {
            //        _database.CloseConnection(connection);
            //    }
            //}
        }


        public ProjectDetailsViewModel GetProjectDetails(int projectId)
        {
            try
            {
                var project = _context.ProjectsData
                    .FirstOrDefault(p => p.ProjectID == projectId);

                if (project == null)
                    return null;

                var issues = _context.Issues
                    .AsEnumerable()
                    .Where(i => i.IssueData.ProjectID == projectId)
                    .Select(i => new IssueModelWrapper
                    {
                        IssueID = i.IssueID,
                        IssueData = i.IssueData
                    })
                    .ToList();

                return new ProjectDetailsViewModel
                {
                    Project = project,
                    Issues = issues
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetProjectDetails: {ex.Message}");
                return null;
            }
        }

        //public List<ProjectModelWrapper> ViewProject()
        //{

        //}

        //public void ResetProjectIds()
        //{
        //    string query = "DBCC CHECKIDENT ('ProjectsData', RESEED, 0)";
        //    SqlConnection connection = null;
        //    SqlCommand command = null;

        //    try
        //    {
        //        connection = _database.makeConnection();
        //        if (connection == null) return;

        //        command = _database.makeCommand(query, connection);
        //        if (command == null) return;

        //        connection.Open();
        //        command.ExecuteNonQuery();
        //    }
        //    catch (SqlException ex)
        //    {
        //        Console.WriteLine($"Error resetting IDs: {ex.Message}");
        //        throw;
        //    }
        //    finally
        //    {
        //        // Manually dispose resources
        //        if (command != null)
        //        {
        //            command.Dispose();
        //        }
        //        if (connection != null)
        //        {
        //            if (connection.State == ConnectionState.Open)
        //            {
        //                connection.Close();
        //            }
        //            connection.Dispose();
        //        }
        //    }
        //}
    }
}