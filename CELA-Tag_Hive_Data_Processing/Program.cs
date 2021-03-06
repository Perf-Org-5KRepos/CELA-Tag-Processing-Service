using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Exceptions;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;

using System.Threading;
using System.Linq;
using CELA_Knowledge_Management_Data_Services.BusinessLogic;
using CELA_Knowledge_Management_Data_Services.Models;
using CELA_Tags_Parsing_Service.Storage;

namespace CELA_Tagulous_Data_Processing
{
    /// <summary>
    /// Sample program that shows how to get started with the Graph (Gremlin) APIs for Azure Cosmos DB using the open-source connector Gremlin.Net
    /// </summary>
    class Program
    {
        // Azure Cosmos DB Configuration variables
        // Replace the values in these variables to your own.
        private static string hostname = "[GRAPH_HOSTNAME]";
        private static int port = 443;
        private static string authKey = "[GRAPH_KEY]";
        private static string database = "[GRAPH_NAME]";
        private static string collection = "[GRAPH_COLLECTION]";

        private const bool TestMode = false;
        private const int TestModeCount = 10; 

        // Starts a console application that executes every Gremlin query in the gremlinQueries dictionary. 
        static void Main(string[] args)
        {
            Console.WriteLine(string.Format("Retrieving tagged communications from {0}.", CommunicationProcessingBusinessLogic.GetStorageEndpoint()));

            var communications = CommunicationProcessingBusinessLogic.RetrieveEmailsAsync().Result;

            Console.WriteLine(string.Format("Processing communications to {0}", StorageUtility.GetInstance().GetHostname()));

            int counter = 1;
            foreach (var communication in communications)
            {
                Console.WriteLine(String.Format("Processing communication {0} of {1}.", counter, communications.Count));
                StorageUtility.GetInstance().StoreEmailToGraphStorage(communication);
                counter++; 
            }

            // Exit program
            Console.WriteLine("Done. Press any key to exit...");
            Console.ReadLine();
        }


        private static List<EmailSearch> LimitCommunicationsForTestMode(List<EmailSearch> communications)
        {
            // Reverse the order to process more recent 
            communications.Reverse();

            if (communications.Count > TestModeCount)
            {
                communications = communications.Take<EmailSearch>(TestModeCount).ToList<EmailSearch>();
            }
            Console.WriteLine(String.Format("TestMode: Limiting communications to be processed to {0} elements.", communications.Count));
            return communications;
        }
    }
}
