using Microsoft.Data.SqlClient;
using System.Data;
using Jira_2._0.Interfaces;
using Jira_2._0.Models.Context;
using Microsoft.EntityFrameworkCore;
using Jira_2._0.Data;
using Microsoft.AspNetCore.Identity;
using Jira_2._0.Models.CustomisedUserModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
namespace Jira_2._0.Models.DatabaseRepositories
{
    public class IssueRepo:IIssueRepo
    {
        //private readonly Database _database;
        private readonly JiraDBContext _context;
        private readonly ApplicationDbContext Acontext;
        
        public IssueRepo(JiraDBContext context, ApplicationDbContext acontext)
        {
            _context = context;
            Acontext = acontext;
          
        }

        //public int Insert(IssueModelWrapper wrapper)
        //{

        //    try
        //    {




        //        if (wrapper != null)
        //        {
        //            _context.Issues.Add(wrapper);

        //        }
        //        // Add to context and save
        //        _context.SaveChanges();

        //        return wrapper.IssueID;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return 0;
        //    }
        //    //string query = "INSERT INTO Issues (Data) OUTPUT INSERTED.IssueID VALUES (@Data)";
        //    //SqlConnection connection = null;
        //    //SqlCommand command = null;

        //    //try
        //    //{
        //    //    connection = _database.makeConnection();
        //    //    if (connection == null) return 0;

        //    //    command = _database.makeCommand(query, connection);
        //    //    if (command == null) return 0;

        //    //    command.Parameters.Add("@Data", SqlDbType.NVarChar).Value = wrapper.Serialize();

        //    //    return (int)command.ExecuteScalar();
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    Console.WriteLine(e.Message);
        //    //    return 0;
        //    //}
        //    //finally
        //    //{
        //    //    if (command != null) command.Dispose();
        //    //    if (connection != null && connection.State == ConnectionState.Open)
        //    //    {
        //    //        _database.CloseConnection(connection);
        //    //    }
        //    //}
        //}
        public bool AssignIssueToUser(int issueId, int projectId, string assigneeName)
        {
            try
            {
                // Use Acontext to get the Identity user
                var assignedUser = Acontext.Users.FirstOrDefault(u => u.UserName == assigneeName);

                if (assignedUser == null)
                {
                    Console.WriteLine($"Assigned user '{assigneeName}' not found.");
                    return false;
                }

                var newAssignment = new AssignedIssues
                {
                    IssueID = issueId,
                    AssignedUserId = assignedUser.Id,
                    ProjectID = projectId
                };

                _context.AssignedIssues.Add(newAssignment);
                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error assigning issue: {e.Message}");
                return false;
            }
        }


        public int Insert(IssueModelWrapper wrapper)
        {
            try
            {
                if (wrapper != null)
                {
                    _context.Issues.Add(wrapper);
                    _context.SaveChanges();

                    // After successfully inserting the issue, assign it
                    bool assignmentSuccess = AssignIssueToUser(
                        wrapper.IssueID,
                        wrapper.IssueData.ProjectID,
                        wrapper.IssueData.Assigned);

                    if (!assignmentSuccess)
                    {
                        Console.WriteLine("Issue was created but assignment failed.");
                    }

                    return wrapper.IssueID;
                }
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }
        public IssueModelWrapper GetIssueById(int id)
        {

            try
            {
                var issueData = _context.Issues
                    .FirstOrDefault(p => p.IssueID == id);
                return issueData;
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
            //Console.WriteLine($"Issue #{id}");
            //SqlConnection connection = _database.makeConnection();
            //string query = "SELECT IssueID, Data FROM Issues WHERE IssueID = @id";
            //SqlCommand command = _database.makeCommand(query, connection);
            //command.Parameters.AddWithValue("@id", id);

            //try
            //{
            //    SqlDataReader reader = command.ExecuteReader();
            //    if (reader.Read())
            //    {
            //        int issueId = reader.GetInt32(0);
            //        string jsonData = reader.GetString(1);

            //        return IssueModelWrapper.Deserialize(issueId, jsonData);
            //    }
            //    return null;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //    return null;
            //}
            //finally
            //{
            //    _database.CloseConnection(connection);
            //}
        }

        //public bool UpdateIssue(IssueModelWrapper wrapper)
        //{

        //    try
        //    {
        //        // Retrieve the existing project record by ID
        //        var issueData = _context.Issues
        //            .FirstOrDefault(p => p.IssueID == wrapper.IssueID);

        //        if (issueData == null)
        //        {
        //            // No existing record found to update
        //            return false;
        //        }

        //        // Update the serialized data
        //        issueData.IssueData = wrapper.IssueData;


        //        // Mark the entity as modified (optional, EF Core often tracks this automatically)
        //        _context.Issues.Update(issueData);

        //        // Save changes to the database
        //        return _context.SaveChanges() > 0;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"Error updating issue: {e.Message}");
        //        return false;
        //    }

        //    //SqlConnection connection = _database.makeConnection();

        //    //string query = "UPDATE Issues SET Data = @data WHERE IssueID = @id";
        //    //SqlCommand command = _database.makeCommand(query, connection);

        //    //// Correct parameters
        //    //command.Parameters.AddWithValue("@id", wrapper.IssueID);
        //    //command.Parameters.AddWithValue("@data", wrapper.Serialize());

        //    //try
        //    //{
        //    //    Console.WriteLine("uri");
        //    //    return _database.ManipulateQuery(command);
        //    //}
        //    //catch (Exception e)
        //    //{

        //    //    Console.WriteLine(e.Message);
        //    //    return false;
        //    //}
        //    //finally
        //    //{
        //    //    _database.CloseConnection(connection);
        //    //}
        //}

        public bool UpdateIssue(IssueModelWrapper wrapper)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                // Retrieve the existing issue record by ID
                var issueData = _context.Issues
                    .FirstOrDefault(p => p.IssueID == wrapper.IssueID);

                if (issueData == null)
                {
                    return false;
                }

                // Update the issue data
                issueData.IssueData = wrapper.IssueData;
                _context.Issues.Update(issueData);

                // Handle assignment update if needed
                if (!string.IsNullOrEmpty(wrapper.IssueData.Assigned))
                {
                    var assignment = _context.AssignedIssues
                        .FirstOrDefault(a => a.IssueID == wrapper.IssueID);

                    var assignedUser = Acontext.Users
                        .FirstOrDefault(u => u.Name == wrapper.IssueData.Assigned);

                    if (assignedUser == null)
                    {
                        Console.WriteLine($"Assigned user '{wrapper.IssueData.Assigned}' not found.");
                        transaction.Rollback();
                        return false;
                    }

                    if (assignment != null)
                    {
                        assignment.AssignedUserId = assignedUser.Id;
                        assignment.ProjectID = wrapper.IssueData.ProjectID;
                        _context.AssignedIssues.Update(assignment);
                    }
                    else
                    {
                        AssignIssueToUser(wrapper.IssueID, wrapper.IssueData.ProjectID, wrapper.IssueData.Assigned);
                    }
                }


                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                Console.WriteLine($"Error updating issue: {e.Message}");
                Console.WriteLine($"Inner Exception: {e.InnerException?.Message}");
                Console.WriteLine($"Full Exception: {e}");
                return false;
            }
        }


        public bool DeleteIssue(int id)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                // First delete from AssignedIssues table
                var assignments = _context.AssignedIssues
                    .Where(a => a.IssueID == id)
                    .ToList();

                if (assignments.Any())
                {
                    _context.AssignedIssues.RemoveRange(assignments);
                }

                // Then delete from Issues table
                var issueData = _context.Issues
                    .FirstOrDefault(p => p.IssueID == id);

                if (issueData == null)
                {
                    return false;
                }

                _context.Issues.Remove(issueData);
                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                Console.WriteLine(e.Message);
                return false;
            }
        }

        //public bool DeleteIssue(int id)
        //{

        //    try
        //    {
        //        var issueData = _context.Issues
        //            .FirstOrDefault(p => p.IssueID == id);

        //        if (issueData == null)
        //        {
        //            return false;
        //        }

        //        // Remove from context
        //        _context.Issues.Remove(issueData);

        //        // Save changes
        //        return _context.SaveChanges() > 0;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return false;
        //    }
        //    //Console.WriteLine(id);
        //    //SqlConnection connection = _database.makeConnection();
        //    //string query = "DELETE FROM Issues WHERE IssueID = @id";
        //    //SqlCommand command = _database.makeCommand(query, connection);
        //    //command.Parameters.AddWithValue("@id", id);

        //    //try
        //    //{
        //    //    Console.WriteLine("heheh");
        //    //    return _database.ManipulateQuery(command);
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    Console.WriteLine("yo");
        //    //    Console.WriteLine(e.Message);
        //    //    return false;
        //    //}
        //    //finally
        //    //{
        //    //    _database.CloseConnection(connection);
        //    //}
        //}
        public List<IssueModelWrapper> GetAllIssuesForManager(string name)
        {
            try
            {
                // Step 1: Get manager name using userId
                
               

               

                // Step 2: Load all issues
                var allIssues = _context.Issues.ToList();

                // Step 3: Filter based on whether the issue belongs to a project led by the manager
                var filteredIssues = allIssues
                    .Where(issue =>
                    {
                        // Step 4: Extract ProjectID from IssueData (assumed to be a deserialized object)
                        int projectId = issue.IssueData.ProjectID;

                        // Step 5: Get project
                        var project = _context.ProjectsData.FirstOrDefault(p => p.ProjectID == projectId);

                        // Step 6: Compare leader name
                        return project != null && project.ProjectData.Leader == name;
                    })
                    .OrderBy(i => i.IssueID)
                    .Select(i => new IssueModelWrapper
                    {
                        IssueID = i.IssueID,
                        IssueData = i.IssueData
                    })
                    .ToList();

                return filteredIssues;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<IssueModelWrapper>();
            }
        }



        public List<IssueModelWrapper> GetAllIssues()
        {
            try
            {
                var issues = _context.Issues
                    .OrderBy(p => p.IssueID)
                    .Select(p => new IssueModelWrapper
                    {
                        IssueID = p.IssueID,
                        IssueData = p.IssueData // EF auto-deserializes this
                    })
                    .ToList();

                return issues;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<IssueModelWrapper>();
            }

            //var issues = new List<IssueModelWrapper>();
            //string sql = "SELECT IssueID, Data FROM Issues ORDER BY IssueID;";

            //SqlConnection connection = null;
            //SqlCommand command = null;

            //try
            //{
            //    connection = _database.makeConnection();
            //    if (connection == null) return issues;

            //    command = _database.makeCommand(sql, connection);
            //    if (command == null) return issues;

            //    SqlDataReader reader = command.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        int issueId = reader.GetInt32(0);
            //        string jsonData = reader.GetString(1);

            //        issues.Add(IssueModelWrapper.Deserialize(issueId, jsonData));
            //    }

            //    return issues;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //    return issues;
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

        //public List<ProjectModelWrapper> ViewProject()
        //{

        //}

        //public void ResetIssueIds()
        //{
        //    string query = "DBCC CHECKIDENT ('issues', RESEED, 0)";
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
        public List<IssueModelWrapper> SearchIssues(string projectId, string priority, string status)
        {
            try
            {
                var query = _context.Issues.AsEnumerable();

                if (int.TryParse(projectId, out int pid))
                {
                    query = query.Where(i => i.IssueData.ProjectID == pid);
                }


                if (!string.IsNullOrEmpty(priority))
                {
                    query = query.Where(i => i.IssueData.Priority.ToLower() == priority.ToLower());
                }

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(i => i.IssueData.Status.ToLower() == status.ToLower());
                }

                return query.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<IssueModelWrapper>();
            }
        }

        public async Task<List<IssueModelWrapper>> GetAssignedIssuesForUserAsync(string userName)
        {
            try
            {
                var resolvedIssueIds = _context.ResolvedIssues
            .Select(ri => ri.IssueID)
            .ToHashSet(); // Efficient for lookup

                var assignedIssues = _context.Issues
                    .AsEnumerable() // Bring all records to memory
                    .Where(i => i.IssueData != null &&
                                i.IssueData.Status == "Assigned" &&
                                i.IssueData.Assigned == userName &&
                        !resolvedIssueIds.Contains(i.IssueID))
                    .ToList();
                foreach (var item in assignedIssues)
                {
                    Console.WriteLine(item.IssueData.Assigned);
                }
                return assignedIssues;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving assigned issues: {ex.Message}");
                return new List<IssueModelWrapper>();
            }
        }



        public async Task<bool> UpdateIssueStatusAndPriorityAsync(int IssueID, string Status, string priority, string Title, int ProjectID, string Id)
        {
            var issueWrapper = await _context.Issues.FirstOrDefaultAsync(i => i.IssueID == IssueID);

            if (issueWrapper != null && issueWrapper.IssueData != null)
            {
                issueWrapper.IssueData.Title = Title;
                issueWrapper.IssueData.Status = Status;
                issueWrapper.IssueData.Priority = priority;
                issueWrapper.IssueData.LastUpdated = DateTime.Now.ToString();
               
                await _context.SaveChangesAsync();

                // If resolved, add to ResolvedIssues and remove from AssignedIssues
                if (Status == "Resolved")
                {
                    // Check if already resolved
                    bool alreadyResolved = await _context.ResolvedIssues.AnyAsync(r => r.IssueID == IssueID);
                    if (!alreadyResolved)
                    {
                        var resolved = new ResolvedIssues
                        {
                            IssueID = IssueID,
                            ProjectID = ProjectID,
                            ResolvedUserId = Id
                        };
                        _context.ResolvedIssues.Add(resolved);
                    }

                    // Remove from AssignedIssues
                    var assignedIssue = await _context.AssignedIssues.FirstOrDefaultAsync(a => a.IssueID == IssueID);
                    if (assignedIssue != null)
                    {
                        _context.AssignedIssues.Remove(assignedIssue);
                    }

                    await _context.SaveChangesAsync();
                }

                return true;
            }

            return false;
        }

        public async Task<List<ResolvedIssueViewModel>> GetResolvedIssuesByUserAsync(string userId)
        {
            Console.WriteLine("kasa");
            return await _context.ResolvedIssues
                .Where(r => r.ResolvedUserId == userId)
                .Join(_context.Issues,
                      resolved => resolved.IssueID,
                      issue => issue.IssueID,
                      (resolved, issue) => new ResolvedIssueViewModel
                      {
                          IssueID = issue.IssueID,
                          Title = issue.IssueData.Title,
                          Priority = issue.IssueData.Priority,
                          ResolvedBy = resolved.ResolvedUserId,
                          DateResolved = issue.IssueData.LastUpdated.ToString(),
                      })
                .ToListAsync();
        }


    }




}


 