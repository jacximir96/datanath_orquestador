using domain.Entities;
using domain.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Repositories
{
    public class UserRepository : IUserRepository<domain.Entities.User>
    {
        private CosmosClient client;

        public async Task<domain.Entities.User> GetUser(string name, string password)
        {
            List<domain.Entities.User> users = new List<domain.Entities.User>();    
            try
            {
                using (CosmosClient client = GetObjectDataBase())
                {
                    var query = new QueryDefinition("SELECT * FROM c WHERE c.Name=@name").WithParameter("@name", name);
                    var container = client.GetDatabase(ResourceInfra.cosmosDatabase).GetContainer(ResourceInfra.cosmosUsersContainer);
                    using (var result = container.GetItemQueryIterator<domain.Entities.User>(query))
                    {

                        while (result.HasMoreResults)
                        {
                            var _response = await result.ReadNextAsync();
                            users.AddRange(_response.ToList());
                        }

                    }
                }
            }
            catch (Exception e) 
            {
                e.Message.ToString();
            }

            return users.FirstOrDefault();
        }

        public CosmosClient GetObjectDataBase()
        {
            if (client == null)
            {
                CosmosClientOptions options = new CosmosClientOptions()
                {
                    ConnectionMode = ConnectionMode.Gateway, // Required for Emulator
                    HttpClientFactory = () =>
                    {
                        HttpMessageHandler handler = new HttpClientHandler()
                        {
                            ServerCertificateCustomValidationCallback =
                                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                        };
                        return new HttpClient(handler);
                    },
                };
                
                client = new CosmosClient(ResourceInfra.cosmosServer, ResourceInfra.cosmosPassword, options);
            }

            return client;
        }
    }
}
