using CrimeAnalysis.App.Entity;
using CrimeAnalysisAndReportingSystem.entity;
using System.Collections.Generic;


namespace CrimeAnalysis.App.DAO
{
    namespace CrimeAnalysis.App.DAO
    {
        public interface ICrimeAnalysisService
        {
            // Create a new incident
            bool CreateIncident(Incident incident);

            // Update the status of an incident
            bool UpdateIncidentStatus(int incidentId, string newStatus);

            // Get a list of incidents within a date range
            List<Incident> GetIncidentsInDateRange(DateTime startDate, DateTime endDate);

            // Search for incidents based on incident type
            List<Incident> SearchIncidents(string incidentType);

            // Generate incident reports
            Report GenerateIncidentReport(Incident incident);

            // Create a new case and associate it with incidents
            Case CreateCase(string caseDescription, ICollection<Incident> incidents);

            // Get details of a specific case
            Case GetCaseDetails(int caseId);

            // Update case details
            bool UpdateCaseDetails(Case caseDetails);

            // Get a list of all cases
            List<Case> GetAllCases();
        }
    }
}
