using NUnit.Framework;
using CrimeAnalysis.App.DAO;
using CrimeAnalysis.App.Entity;

namespace CrimeAnalysisApp.test
{
    public class IncidentCreation
    {
        CrimeAnalysisServiceImpl service;

        [SetUp]
        public void Setup()
        {
            service = new CrimeAnalysisServiceImpl();
        }

        [Test]
        public void Test_CreateIncident_WithValidData_ReturnsTrue()
        {
            Incident incident = new Incident
            {
                IncidentType = "Robbery",
                IncidentDate = DateTime.Now,
                Latitude = 10.0,
                Longitude = 20.0,
                Description = "Unit Test Incident",
                Status = "Open",
                VictimId = 1,
                SuspectId = 1,
                AgencyId = 1
            };

            bool result = service.CreateIncident(incident);

            NUnit.Framework.Assert.IsTrue(result, "Expected CreateIncident() to return true for valid incident.");
        }
    }
}
