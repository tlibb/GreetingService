using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreetingService.Core.Entities;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GreetingService.Infrastructure.UserService
{
    public class BlobUserService : IUserService
    {

        public BlobContainerClient _container;

        private readonly string _pathToUsersBlob = "users/users.json";

        public BlobUserService(IConfiguration config)
        {
            _container = new BlobContainerClient(config["LoggingStorageAccount"], "users");
            _container.CreateIfNotExists();
            if (_container.GetBlobs().Count() == 0)
            {
                var mydict = new Dictionary<string, object>();
                mydict.Add(config["MyUserName"], config["MyPassWord"]); //this only works locally
                var binarycontent = new BinaryData(mydict, new JsonSerializerOptions { WriteIndented = true });
                _container.UploadBlob(_pathToUsersBlob, binarycontent);
            }
        }

        public async Task<bool> IsValidUserAsync(string username, string password)
        {
            try
            {
                var blobClient = _container.GetBlobClient(_pathToUsersBlob);
                var mycontent = blobClient.DownloadContent();
                Dictionary<string, string> mydict = mycontent.Value.Content.ToObjectFromJson<Dictionary<string, string>>();
                foreach (var key in mydict.Keys)
                {
                    if (key == username && mydict[key] == password)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;

        }

        public Task<User> GetUserAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task CreateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }
    }
}
