using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyComicListApi.Models;
using MyComicListApi.Security;

namespace MyComicListApi.Services
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private static DocumentClient _client;
        private readonly PullListDbSettings _settings;
        private Uri _uri;

        public ApplicationUserRepository(IOptions<PullListDbSettings> settings)
        {
            _settings = settings.Value;
            _uri = new Uri(_settings.EndpointUrl);
        }

        public bool ValidateCredentials(string username, string password)
        {
            bool result = false;
            using (_client = new DocumentClient(_uri, _settings.AuthorizationKey))
            {
                string pathLink = string.Format("dbs/{0}/colls/{1}", _settings.DatabaseId, _settings.ApplicationUserCollectionId);

                ApplicationUser doc = _client.CreateDocumentQuery<ApplicationUser>(pathLink).Where(u => u.UserName == username).AsEnumerable().FirstOrDefault();

                if(doc != null)
                {
                    var encryptedPassword = Hasher.ComputeSHA256(password);
                    if(doc.Password == encryptedPassword)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        public async Task<string> RegisterNewUser(string username, string password, string email, string firstName, string lastName)
        {
            string result = null;

            ApplicationUser newUser = new ApplicationUser();
            newUser.UserName = username;
            newUser.Password = password;
            newUser.Email = email;
            newUser.FirstName = firstName;
            newUser.LastName = lastName;

            using (_client = new DocumentClient(_uri, _settings.AuthorizationKey))
            {
                string pathLink = string.Format("dbs/{0}/colls/{1}", _settings.DatabaseId, _settings.ApplicationUserCollectionId);

                dynamic existingUserDoc =  _client.CreateDocumentQuery<ApplicationUser>(pathLink).Where(u => u.UserName == username).AsEnumerable().Any();
                ResourceResponse<Document> doc = null;
                if (existingUserDoc == false)
                {
                    doc = await _client.CreateDocumentAsync(pathLink, newUser);
                    result = "Account Registered Successfully";
                }
                else
                {
                    result = "Username already exists";
                }
                
            }

            return result;
        }
    }
}
