using CrimeAnalysis.App.DAO;
using CrimeAnalysis.App.DAO.CrimeAnalysis.App.DAO;
using CrimeAnalysis.App.Entity;
using CrimeAnalysis.App.MyExceptions;
using CrimeAnalysisAndReportingSystem.entity;
using System;
using System.Collections.Generic;

namespace CrimeAnalysis.App.Main
{
    class MainModule
    {
        private readonly ICrimeAnalysisService service;

        public MainModule()
        {
            service = new CrimeAnalysisServiceImpl();
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n--- Crime Analysis Menu ---");
                Console.WriteLine("1. Create New Incident");
                Console.WriteLine("2. Update Incident Status");
                Console.WriteLine("3. View Incidents Between Dates");
                Console.WriteLine("4. Search Incidents by Type");
                Console.WriteLine("5. Generate Incident Report");
                Console.WriteLine("6. Create New Case");
                Console.WriteLine("7. View Case Details");
                Console.WriteLine("8. Update Case");
                Console.WriteLine("9. View All Cases");
                Console.WriteLine("0. Exit");
                Console.Write("Enter choice: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input! Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        CreateIncident();
                        break;
                    case 2:
                        UpdateIncidentStatus();
                        break;
                    case 3:
                        ViewIncidentsInDateRange();
                        break;
                    case 4:
                        SearchIncidentsByType();
                        break;
                    case 5:
                        GenerateReport();
                        break;
                    case 6:
                        CreateNewCase();
                        break;
                    case 7:
                        ViewCaseDetails();
                        break;
                    case 8:
                        UpdateCase();
                        break;
                    case 9:
                        ViewAllCases();
                        break;
                    case 0:
                        Console.WriteLine("Exiting... Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice! Try again.");
                        break;
                }
            }
        }

        private void CreateIncident()
        {
            try
            {
                Incident incident = new Incident();

                Console.Write("Incident Type: ");
                incident.IncidentType = Console.ReadLine();

                Console.Write("Incident Date (yyyy-MM-dd): ");
                incident.IncidentDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Latitude: ");
                incident.Latitude = Convert.ToDouble(Console.ReadLine());

                Console.Write("Longitude: ");
                incident.Longitude = Convert.ToDouble(Console.ReadLine());

                Console.Write("Description: ");
                incident.Description = Console.ReadLine();

                Console.Write("Status: ");
                incident.Status = Console.ReadLine();

                Console.Write("Victim ID: ");
                incident.VictimId = Convert.ToInt32(Console.ReadLine());

                Console.Write("Suspect ID: ");
                incident.SuspectId = Convert.ToInt32(Console.ReadLine());

                Console.Write("Agency ID: ");
                incident.AgencyId = Convert.ToInt32(Console.ReadLine());

                bool created = service.CreateIncident(incident);
                Console.WriteLine(created ? "Incident created successfully!" : "Failed to create incident.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void UpdateIncidentStatus()
        {
            try
            {
                Console.Write("Incident ID: ");
                int incidentId = Convert.ToInt32(Console.ReadLine());

                Console.Write("New Status: ");
                string status = Console.ReadLine();

                bool updated = service.UpdateIncidentStatus(incidentId, status);
                Console.WriteLine(updated ? "Incident status updated successfully!" : "Failed to update incident status.");
            }
            catch (IncidentNumberNotFoundException ex)
            {
                Console.WriteLine($"Custom Exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void ViewIncidentsInDateRange()
        {
            try
            {
                Console.Write("Start Date (yyyy-MM-dd): ");
                DateTime startDate = DateTime.Parse(Console.ReadLine());

                Console.Write("End Date (yyyy-MM-dd): ");
                DateTime endDate = DateTime.Parse(Console.ReadLine());

                List<Incident> incidents = service.GetIncidentsInDateRange(startDate, endDate);
                if (incidents.Count == 0)
                {
                    Console.WriteLine("No incidents found in the given date range.");
                    return;
                }

                Console.WriteLine("\n--- Incidents in Date Range ---");
                foreach (Incident inc in incidents)
                {
                    Console.WriteLine($"ID: {inc.IncidentId}, Type: {inc.IncidentType}, Date: {inc.IncidentDate:yyyy-MM-dd}, Status: {inc.Status}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void SearchIncidentsByType()
        {
            Console.Write("Incident Type to search: ");
            string type = Console.ReadLine();

            List<Incident> incidents = service.SearchIncidents(type);

            if (incidents.Count == 0)
            {
                Console.WriteLine("No incidents found for the given type.");
            }
            else
            {
                Console.WriteLine("\n--- Search Results ---");
                foreach (Incident inc in incidents)
                {
                    Console.WriteLine($"ID: {inc.IncidentId}, Type: {inc.IncidentType}, Date: {inc.IncidentDate:yyyy-MM-dd}, Status: {inc.Status}");
                }
            }
        }

        private void GenerateReport()
        {
            try
            {
                Console.Write("Incident ID: ");
                int incidentId = Convert.ToInt32(Console.ReadLine());

                Incident incident = new Incident { IncidentId = incidentId };
                Report report = service.GenerateIncidentReport(incident);

                if (report != null)
                {
                    Console.WriteLine($"Report generated successfully for Incident ID {incidentId}.");
                }
                else
                {
                    Console.WriteLine("Failed to generate report.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void CreateNewCase()
        {
            try
            {
                Console.Write("Case Description: ");
                string description = Console.ReadLine();

                Console.Write("Incident IDs to associate (comma-separated): ");
                string input = Console.ReadLine();
                string[] incidentIds = input.Split(',');

                List<Incident> incidents = new List<Incident>();
                foreach (string idStr in incidentIds)
                {
                    if (int.TryParse(idStr.Trim(), out int id))
                    {
                        incidents.Add(new Incident { IncidentId = id });
                    }
                    else
                    {
                        Console.WriteLine($"Invalid ID ignored: {idStr}");
                    }
                }

                Case newCase = service.CreateCase(description, incidents);
                if (newCase != null)
                {
                    Console.WriteLine($"Case created successfully! Case ID: {newCase.CaseId}");
                }
                else
                {
                    Console.WriteLine("Failed to create case.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void ViewCaseDetails()
        {
            try
            {
                Console.Write("Case ID: ");
                int caseId = Convert.ToInt32(Console.ReadLine());

                Case caseDetails = service.GetCaseDetails(caseId);
                if (caseDetails == null)
                {
                    Console.WriteLine("Case not found.");
                    return;
                }

                Console.WriteLine($"\nCase ID: {caseDetails.CaseId}, Description: {caseDetails.CaseDescription}, Status: {caseDetails.Status}, Created: {caseDetails.CreatedDate:yyyy-MM-dd}");
                Console.WriteLine("--- Associated Incidents ---");
                foreach (Incident inc in caseDetails.Incidents)
                {
                    Console.WriteLine($"Incident ID: {inc.IncidentId}, Type: {inc.IncidentType}, Date: {inc.IncidentDate:yyyy-MM-dd}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void UpdateCase()
        {
            try
            {
                Console.Write("Case ID: ");
                int caseId = Convert.ToInt32(Console.ReadLine());

                Console.Write("New Case Description: ");
                string desc = Console.ReadLine();

                Console.Write("New Status: ");
                string status = Console.ReadLine();

                Case caseUpdate = new Case
                {
                    CaseId = caseId,
                    CaseDescription = desc,
                    Status = status
                };

                bool updated = service.UpdateCaseDetails(caseUpdate);
                Console.WriteLine(updated ? "Case updated successfully." : "Failed to update case.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void ViewAllCases()
        {
            List<Case> cases = service.GetAllCases();

            if (cases.Count == 0)
            {
                Console.WriteLine("No cases found.");
                return;
            }

            Console.WriteLine("\n--- All Cases ---");
            foreach (Case c in cases)
            {
                Console.WriteLine($"Case ID: {c.CaseId}, Description: {c.CaseDescription}, Status: {c.Status}, Created: {c.CreatedDate:yyyy-MM-dd}");
                if (c.Incidents.Count > 0)
                {
                    Console.WriteLine("  Associated Incidents:");
                    foreach (Incident inc in c.Incidents)
                    {
                        Console.WriteLine($"    - Incident ID: {inc.IncidentId}, Type: {inc.IncidentType}, Date: {inc.IncidentDate:yyyy-MM-dd}");
                    }
                }
            }
        }
    }
}

