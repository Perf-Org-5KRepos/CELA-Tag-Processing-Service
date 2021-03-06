using System;
using System.IO;
using CELA_Knowledge_Management_Data_Services.BusinessLogic;
using CELA_Knowledge_Management_Data_Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace CELA_Knowledge_Management_Data_Services.tests
{
    [TestClass]
    public class GraphTransactionTests
    {
        [TestMethod]
        public void ConformStringForQueryTest()
        {
            var stringTest = "Some string that has an invalid character like $";
            var result = GraphQueryBusinessLogic.ConformStringForQuery(stringTest);
            if (result.Length >= stringTest.Length)
            {
                Assert.Fail("Failed to remove disallowed character.");
            }
        }

        [TestMethod]
        public void CommunicatorInsertionTest()
        {
            using (var gremlinClient = CommunicationProcessingBusinessLogic.CreateGremlinClient(KnowledgeManagementDataServicesTestSettings.GraphDBHostName, KnowledgeManagementDataServicesTestSettings.GraphDBPort, KnowledgeManagementDataServicesTestSettings.GraphDBAuthKey, KnowledgeManagementDataServicesTestSettings.GraphDBDatabaseName, KnowledgeManagementDataServicesTestSettings.GraphDBCollectionName))
            {
                string emailAddress = "testemail@microsoft.com";

                //string deletionQuery = string.Format("g.V().hasLabel('communicator').has('name', '{0}').drop()", emailAddress);

                string ID1 = CommunicationProcessingBusinessLogic.AddCommunicatorAndOrganization(emailAddress, gremlinClient);

                string ID2 = CommunicationProcessingBusinessLogic.AddCommunicatorAndOrganization(emailAddress, gremlinClient);

                //We should not have duplicate insertions, so the first insertion should succeed, and the second query should return the ID of the first as an existing
                if (ID1.CompareTo(ID2) != 0)
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void ProcessCommunicationToGraphDBTest()
        {
            var testEmail = JsonConvert.DeserializeObject<TagsSearchByTagStartTokenOrdinal>(File.ReadAllText(@"[REPO_ROOT]\CELA-Tagulous_Parsing_Service\CELA-Tags_Parsing_ServiceTests\TestRequests\TestRequest11.json"));
            var result = CommunicationProcessingBusinessLogic.ProcessCommunicationToGraphDB(testEmail, KnowledgeManagementDataServicesTestSettings.GraphDBHostName, KnowledgeManagementDataServicesTestSettings.GraphDBPort, KnowledgeManagementDataServicesTestSettings.GraphDBAuthKey, KnowledgeManagementDataServicesTestSettings.GraphDBDatabaseName, KnowledgeManagementDataServicesTestSettings.GraphDBCollectionName);
            if (!result)
            {
                Assert.Fail();
            }
        }

        private string testContent = @"";
    }
}
