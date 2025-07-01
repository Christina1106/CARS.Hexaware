using CrimeAnalysis.App.DAO.CrimeAnalysis.App.DAO;
using CrimeAnalysis.App.Entity;
using CrimeAnalysis.App.MyExceptions;
using CrimeAnalysis.App.Util;
using CrimeAnalysisAndReportingSystem.entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CrimeAnalysis.App.DAO
{
    public class CrimeAnalysisServiceImpl : ICrimeAnalysisService
    {
        private static SqlConnection connection;

        public CrimeAnalysisServiceImpl()
        {
            connection = DBConnUtil.GetConnection();
        }

        public bool CreateIncident(Incident incident)
        {
            try
            {
                connection.Open();
                string query = @"INSERT INTO Incidents (IncidentType, IncidentDate, Latitude, Longitude, Description, Status, VictimID, SuspectID, AgencyID)
                                 VALUES (@IncidentType, @IncidentDate, @Latitude, @Longitude, @Description, @Status, @VictimID, @SuspectID, @AgencyID)";
                using SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@IncidentType", incident.IncidentType);
                cmd.Parameters.AddWithValue("@IncidentDate", incident.IncidentDate);
                cmd.Parameters.AddWithValue("@Latitude", incident.Latitude);
                cmd.Parameters.AddWithValue("@Longitude", incident.Longitude);
                cmd.Parameters.AddWithValue("@Description", incident.Description);
                cmd.Parameters.AddWithValue("@Status", incident.Status);
                cmd.Parameters.AddWithValue("@VictimID", incident.VictimId);
                cmd.Parameters.AddWithValue("@SuspectID", incident.SuspectId);
                cmd.Parameters.AddWithValue("@AgencyID", incident.AgencyId);

                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Incident created successfully." : "Failed to create incident.");
                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating incident: {ex.Message}");
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool UpdateIncidentStatus(int incidentId, string newStatus)
        {
            try
            {
                connection.Open();
                string query = "UPDATE Incidents SET Status=@Status WHERE IncidentID=@IncidentID";
                using SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Status", newStatus);
                cmd.Parameters.AddWithValue("@IncidentID", incidentId);

                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new IncidentNumberNotFoundException($"Incident ID {incidentId} not found.");
                }
                Console.WriteLine($"Incident {incidentId} status updated to '{newStatus}'.");
                return true;
            }
            catch (IncidentNumberNotFoundException ex)
            {
                Console.WriteLine($"Custom Exception: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating incident status: {ex.Message}");
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public List<Incident> GetIncidentsInDateRange(DateTime startDate, DateTime endDate)
        {
            List<Incident> incidents = new List<Incident>();
            try
            {
                connection.Open();
                string query = "SELECT * FROM Incidents WHERE IncidentDate BETWEEN @StartDate AND @EndDate";
                using SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    incidents.Add(new Incident
                    {
                        IncidentId = (int)reader["IncidentID"],
                        IncidentType = reader["IncidentType"].ToString(),
                        IncidentDate = (DateTime)reader["IncidentDate"],
                        Latitude = Convert.ToDouble(reader["Latitude"]),
                        Longitude = Convert.ToDouble(reader["Longitude"]),
                        Description = reader["Description"].ToString(),
                        Status = reader["Status"].ToString(),
                        VictimId = (int)reader["VictimID"],
                        SuspectId = (int)reader["SuspectID"],
                        AgencyId = (int)reader["AgencyID"]
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving incidents: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return incidents;
        }

        public List<Incident> SearchIncidents(string incidentType)
        {
            List<Incident> incidents = new List<Incident>();
            try
            {
                connection.Open();
                string query = "SELECT * FROM Incidents WHERE IncidentType LIKE @IncidentType";
                using SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@IncidentType", $"%{incidentType}%");

                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    incidents.Add(new Incident
                    {
                        IncidentId = (int)reader["IncidentID"],
                        IncidentType = reader["IncidentType"].ToString(),
                        IncidentDate = (DateTime)reader["IncidentDate"],
                        Latitude = Convert.ToDouble(reader["Latitude"]),
                        Longitude = Convert.ToDouble(reader["Longitude"]),
                        Description = reader["Description"].ToString(),
                        Status = reader["Status"].ToString(),
                        VictimId = (int)reader["VictimID"],
                        SuspectId = (int)reader["SuspectID"],
                        AgencyId = (int)reader["AgencyID"]
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching incidents: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return incidents;
        }

        public Report GenerateIncidentReport(Incident incident)
        {
            try
            {
                connection.Open();
                Console.Write("Enter Reporting Officer ID: ");
                int officerId = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter Report Details: ");
                string details = Console.ReadLine();
                Console.Write("Enter Report Status: ");
                string status = Console.ReadLine();

                DateTime reportDate = DateTime.Now;
                string query = @"INSERT INTO Reports (IncidentID, ReportingOfficer, ReportDate, ReportDetails, Status) 
                                 VALUES (@IncidentID, @OfficerID, @ReportDate, @Details, @Status)";
                using SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@IncidentID", incident.IncidentId);
                cmd.Parameters.AddWithValue("@OfficerID", officerId);
                cmd.Parameters.AddWithValue("@ReportDate", reportDate);
                cmd.Parameters.AddWithValue("@Details", details);
                cmd.Parameters.AddWithValue("@Status", status);

                int rows = cmd.ExecuteNonQuery();
                return rows > 0 ? new Report(0, incident.IncidentId, officerId, reportDate, details, status) : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating report: {ex.Message}");
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

       public Case CreateCase(string caseDescription, ICollection<Incident> incidents)
       {
       try
       {
       connection.Open();

        string insertCaseQuery = @"
            INSERT INTO Cases (CaseDescription, CreatedDate, Status)
            VALUES (@CaseDescription, @CreatedDate, 'Open');
            SELECT SCOPE_IDENTITY();"; 

           using SqlCommand cmd = new SqlCommand(insertCaseQuery, connection);
           cmd.Parameters.AddWithValue("@CaseDescription", caseDescription);
           cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

           int caseId = Convert.ToInt32(cmd.ExecuteScalar());  // get new CaseID

        
           foreach (Incident incident in incidents)
           {
            string insertCaseIncidentQuery = "INSERT INTO Case_Incidents (CaseID, IncidentID) VALUES (@CaseID, @IncidentID)";
            using SqlCommand updateCmd = new SqlCommand(insertCaseIncidentQuery, connection);
            updateCmd.Parameters.AddWithValue("@CaseID", caseId);
            updateCmd.Parameters.AddWithValue("@IncidentID", incident.IncidentId);
            updateCmd.ExecuteNonQuery();
           }
           return new Case
           {
               CaseId = caseId,
               CaseDescription = caseDescription,
               CreatedDate = DateTime.Now,
               Status = "Open",
               Incidents = new List<Incident>(incidents)
            };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating case: {ex.Message}");
                 return null;
            }
            finally
            {
            connection.Close();
            }
       }


        public Case GetCaseDetails(int caseId)
        {
            Case caseDetails = null;
            try
            {
                connection.Open();
                string query = @"SELECT c.CaseID, c.CaseDescription, c.CreatedDate, c.Status, 
                                        i.IncidentID, i.IncidentType, i.IncidentDate
                                 FROM Cases c
                                 LEFT JOIN Case_Incidents ci ON c.CaseID = ci.CaseID
                                 LEFT JOIN Incidents i ON ci.IncidentID = i.IncidentID
                                 WHERE c.CaseID = @CaseID";
                using SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@CaseID", caseId);

                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (caseDetails == null)
                    {
                        caseDetails = new Case
                        {
                            CaseId = reader.GetInt32(0),
                            CaseDescription = reader.GetString(1),
                            CreatedDate = reader.GetDateTime(2),
                            Status = reader.GetString(3),
                            Incidents = new List<Incident>()
                        };
                    }
                    if (!reader.IsDBNull(4))
                    {
                        caseDetails.Incidents.Add(new Incident
                        {
                            IncidentId = reader.GetInt32(4),
                            IncidentType = reader.GetString(5),
                            IncidentDate = reader.GetDateTime(6)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving case: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return caseDetails;
        }

        public bool UpdateCaseDetails(Case caseDetails)
        {
            try
            {
                connection.Open();
                string query = "UPDATE Cases SET CaseDescription=@Desc, Status=@Status WHERE CaseID=@CaseID";
                using SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Desc", caseDetails.CaseDescription);
                cmd.Parameters.AddWithValue("@Status", caseDetails.Status);
                cmd.Parameters.AddWithValue("@CaseID", caseDetails.CaseId);

                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Case updated successfully." : "No case updated.");
                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating case: {ex.Message}");
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public List<Case> GetAllCases()
        {
            List<Case> cases = new List<Case>();
            Dictionary<int, Case> caseDict = new Dictionary<int, Case>();
            try
            {
                connection.Open();
                string query = @"SELECT c.CaseID, c.CaseDescription, c.CreatedDate, c.Status,
                                        i.IncidentID, i.IncidentType, i.IncidentDate
                                 FROM Cases c
                                 LEFT JOIN Case_Incidents ci ON c.CaseID = ci.CaseID
                                 LEFT JOIN Incidents i ON ci.IncidentID = i.IncidentID";
                using SqlCommand cmd = new SqlCommand(query, connection);

                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int caseId = reader.GetInt32(0);
                    if (!caseDict.ContainsKey(caseId))
                    {
                        Case newCase = new Case
                        {
                            CaseId = caseId,
                            CaseDescription = reader.GetString(1),
                            CreatedDate = reader.GetDateTime(2),
                            Status = reader.GetString(3),
                            Incidents = new List<Incident>()
                        };
                        caseDict.Add(caseId, newCase);
                        cases.Add(newCase);
                    }
                    if (!reader.IsDBNull(4))
                    {
                        caseDict[caseId].Incidents.Add(new Incident
                        {
                            IncidentId = reader.GetInt32(4),
                            IncidentType = reader.GetString(5),
                            IncidentDate = reader.GetDateTime(6)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving cases: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
            return cases;
        }
    }
}
