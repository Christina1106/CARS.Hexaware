using System;

namespace CrimeAnalysis.App.Entity
{
    public class Incident
    {
        private int incidentId;
        private string incidentType;
        private DateTime incidentDate;
        private double latitude;
        private double longitude;
        private string location;
        private string description;
        private string status;
        private int victimId;
        private int suspectId;
        private int agencyId;

        public Incident() { }

        public Incident(int incidentId, string incidentType, DateTime incidentDate,
                        double latitude, double longitude, string location,
                        string description, string status, int victimId,
                        int suspectId, int agencyId)
        {
            IncidentId = incidentId;
            IncidentType = incidentType;
            IncidentDate = incidentDate;
            Latitude = latitude;
            Longitude = longitude;
            Location = location;
            Description = description;
            Status = status;
            VictimId = victimId;
            SuspectId = suspectId;
            AgencyId = agencyId;
        }

        public int IncidentId { get => incidentId; set => incidentId = value; }
        public string IncidentType { get => incidentType; set => incidentType = value; }
        public DateTime IncidentDate { get => incidentDate; set => incidentDate = value; }
        public double Latitude { get => latitude; set => latitude = value; }
        public double Longitude { get => longitude; set => longitude = value; }
        public string Location { get => location; set => location = value; }
        public string Description { get => description; set => description = value; }
        public string Status { get => status; set => status = value; }
        public int VictimId { get => victimId; set => victimId = value; }
        public int SuspectId { get => suspectId; set => suspectId = value; }
        public int AgencyId { get => agencyId; set => agencyId = value; }
    }
}
