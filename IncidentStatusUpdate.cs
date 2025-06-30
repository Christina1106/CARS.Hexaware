using NUnit.Framework;
using CrimeAnalysis.App.DAO;

namespace CrimeAnalysisApp.test
{
    public class IncidentStatusUpdate
    {
        CrimeAnalysisServiceImpl service;

        [SetUp]
        public void Setup()
        {
            service = new CrimeAnalysisServiceImpl();
        }

        [Test]
        public void Test_UpdateIncidentStatus_WithValidId_UpdatesSuccessfully()
        {
            int existingIncidentId = 1; // replace with an IncidentID that exists in your db
            string newStatus = "Closed";

            bool result = service.UpdateIncidentStatus(existingIncidentId, newStatus);

            Assert.IsTrue(result, $"Expected status update for IncidentID={existingIncidentId} to succeed.");
        }

        [Test]
        public void Test_UpdateIncidentStatus_WithInvalidId_FailsGracefully()
        {
            int invalidIncidentId = 9999; // Assumed not to exist
            string newStatus = "Closed";

            bool result = service.UpdateIncidentStatus(invalidIncidentId, newStatus);

            Assert.IsFalse(result, "Expected update to fail for non-existent IncidentID.");
        }
    }
}
