using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyComicListApi.Models;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using System.Net;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

namespace MyComicListApi.Services
{
    public class ShortBoxedRepository : IShortBoxedRepository
    {
        private readonly ShortBoxedSettings _settings;
        private Uri _uri;

        public ShortBoxedRepository(IOptions<ShortBoxedSettings> settings)
        {
            _settings = settings.Value;
            _uri = new Uri(_settings.EndpointUrl);
        }
        public async Task<List<Comic>> GetCurrentWeeksComicsAsync()
        {
            List<Comic> newComics = new List<Comic>();
            string address = _uri.ToString() + "comics/v1/new";

            try
            {
                using (var client = new HttpClient())
                {
                    var task =
                        await client.GetAsync(address);
                    if(task != null)
                    {
                        var jsonString = await task.Content.ReadAsStringAsync();
                        newComics = JsonConvert.DeserializeObject<ComicsJson>(jsonString).Comic;
                    } 
                    if(newComics != null)
                    {
                        newComics = newComics.OrderBy(o => o.Publisher).ToList();
                    }
                }
            }
            catch
            {
                throw new WebException("An error has occured while trying to get all the comics released for this week");
            }
            return newComics;
        }

        public async Task<List<Comic>> GetCurrentWeeksComicsByPublisher(string publisher)
        {
            List<Comic> newComics = new List<Comic>();
            string address = _uri.ToString() + "comics/v1/new";

            try
            {
                using (var client = new HttpClient())
                {
                    var task =
                        await client.GetAsync(address);
                    if (task != null)
                    {
                        var jsonString = await task.Content.ReadAsStringAsync();
                        newComics = JsonConvert.DeserializeObject<ComicsJson>(jsonString).Comic;
                    }
                }
            }
            catch
            {
                throw new WebException(string.Format("An error has occured while trying to get the comics by {0} released for this week", publisher));
            }
            return newComics;
        }
    }
}
