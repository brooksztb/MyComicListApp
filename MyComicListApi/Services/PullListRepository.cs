using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;
using MyComicListApi.Models;
using Microsoft.Azure.Documents;

namespace MyComicListApi.Services
{
    public class PullListRepository : IPullListRepository
    {
        private static DocumentClient _client;
        private readonly PullListDbSettings _settings;
        private Uri _uri;

        public PullListRepository(IOptions<PullListDbSettings> settings)
        {
            _settings = settings.Value;
            _uri = new Uri(_settings.EndpointUrl);
        }

        public async Task<string> Add(PullList list)
        {
            string id = null;

            PullList l = new PullList();

            l.Name = list.Name;
            l.Comics = list.Comics;
            l.Active = true;

            using (_client = new DocumentClient(_uri, _settings.AuthorizationKey))
            {
                string pathLink = string.Format("dbs/{0}/colls/{1}", _settings.DatabaseId, _settings.CollectionId);

                ResourceResponse<Document> doc = await _client.CreateDocumentAsync(pathLink, l);

                id = doc.Resource.Id;
            }

            return id;
        }

        public async Task<string> UpdateListStatus(string name, bool status)
        {
            PullList list = FindListByName(name);
            string result = null;

            using (_client = new DocumentClient(_uri, _settings.AuthorizationKey))
            {
                string pathLink = string.Format("dbs/{0}/colls/{1}", _settings.DatabaseId, _settings.CollectionId);

                dynamic doc = _client.CreateDocumentQuery<Document>(pathLink).Where(l => l.Id == list.Id).AsEnumerable().FirstOrDefault();

                if (doc != null)
                {
                    PullList newList = doc;
                    newList.Active = status;

                    ResourceResponse<Document> x = await _client.ReplaceDocumentAsync(doc.SelfLink, newList);

                    result = x.StatusCode.ToString();
                }
            }

            return result;
        }
        public async Task<string> AddComicToList(string name, Comic comic)
        {
            PullList list = FindListByName(name);
            string result = null;

            using (_client = new DocumentClient(_uri, _settings.AuthorizationKey))
            {
                string pathLink = string.Format("dbs/{0}/colls/{1}", _settings.DatabaseId, _settings.CollectionId);

                dynamic doc = _client.CreateDocumentQuery<Document>(pathLink).Where(l => l.Id == list.Id).AsEnumerable().FirstOrDefault();

                if(doc != null)
                {
                    PullList newList = doc;
                    list.Comics.Add(comic);
                    newList.Comics = list.Comics;

                    ResourceResponse<Document> x = await _client.ReplaceDocumentAsync(doc.SelfLink, newList);

                    result = x.StatusCode.ToString();
                }
            }

            return result;
        }

        public async Task<string> AddBulkComicsToList(string name, List<Comic> comicsToAdd)
        {
            PullList list = FindListByName(name);
            string result = null;

            using (_client = new DocumentClient(_uri, _settings.AuthorizationKey))
            {
                string pathLink = string.Format("dbs/{0}/colls/{1}", _settings.DatabaseId, _settings.CollectionId);

                dynamic doc = _client.CreateDocumentQuery<Document>(pathLink).Where(l => l.Id == list.Id).AsEnumerable().FirstOrDefault();

                if (doc != null)
                {
                    PullList newList = doc;
                    foreach(var comic in comicsToAdd)
                    {
                        if(comic != null)
                        {
                            list.Comics.Add(comic);
                        }
                    }
                    
                    newList.Comics = list.Comics;

                    ResourceResponse<Document> x = await _client.ReplaceDocumentAsync(doc.SelfLink, newList);

                    result = x.StatusCode.ToString();
                }
            }

            return result;
        }
        public async Task<string> DeleteList(string name)
        {
            string result = null;
            string id = FindListByName(name).Id;

            using(_client = new DocumentClient(_uri, _settings.AuthorizationKey))
            {
                var docLink = string.Format("dbs/{0}/colls/{1}/docs/{2}", _settings.DatabaseId, _settings.CollectionId, id);

                var x = await _client.DeleteDocumentAsync(docLink);

                if(x != null)
                {
                    result = x.StatusCode.ToString();
                }
            }

            return result;
        }
        public async Task<string> DeleteComicFromList(string nameofComic, string name)
        {
            PullList list = FindListByName(name);
            string result = null;

            using (_client = new DocumentClient(_uri, _settings.AuthorizationKey))
            {
                string pathLink = string.Format("dbs/{0}/colls/{1}", _settings.DatabaseId, _settings.CollectionId);

                dynamic doc = _client.CreateDocumentQuery<Document>(pathLink).Where(l => l.Id == list.Id).AsEnumerable().FirstOrDefault();

                if (doc != null)
                {
                    PullList newList = doc;
                    var comicToRemove = list.Comics.Where(y => y.Title == nameofComic).First();
                    if (comicToRemove != null)
                    {
                        list.Comics.Remove(comicToRemove);
                    }
                    newList.Comics = list.Comics;

                    ResourceResponse<Document> x = await _client.ReplaceDocumentAsync(doc.SelfLink, newList);

                    result = x.StatusCode.ToString();
                }
            }

            return result;
        }

        public async Task<string> DeleteBulkComicsFromList(List<string> nameofComics, string name)
        {
            PullList list = FindListByName(name);
            string result = null;

            using (_client = new DocumentClient(_uri, _settings.AuthorizationKey))
            {
                string pathLink = string.Format("dbs/{0}/colls/{1}", _settings.DatabaseId, _settings.CollectionId);

                dynamic doc = _client.CreateDocumentQuery<Document>(pathLink).Where(l => l.Id == list.Id).AsEnumerable().FirstOrDefault();

                if (doc != null)
                {
                    PullList newList = doc;
                    foreach(string cname in nameofComics)
                    {
                        var comicToRemove = list.Comics.Where(y => y.Title == cname).First();
                        if(comicToRemove != null)
                        {
                            list.Comics.Remove(comicToRemove);
                        }
                    }
                    newList.Comics = list.Comics;

                    ResourceResponse<Document> x = await _client.ReplaceDocumentAsync(doc.SelfLink, newList);

                    result = x.StatusCode.ToString();
                }
            }

            return result;
        }
        public IEnumerable<PullList> GetAllActiveLists()
        {
            List<PullList> lists = null;

            using (_client = new DocumentClient(_uri, _settings.AuthorizationKey))
            {
                string pathLink = string.Format("dbs/{0}/colls/{1}", _settings.DatabaseId, _settings.CollectionId);

                dynamic docs = _client.CreateDocumentQuery<PullList>(pathLink).Where(a => a.Active == true).AsEnumerable().ToList();

                if (docs != null)
                {
                    lists = docs;
                }

            }

            return lists;
        }
        public PullList FindListByName(string name)
        {
            PullList list = null;

            using (_client = new DocumentClient(_uri, _settings.AuthorizationKey))
            {
                string pathLink = string.Format("dbs/{0}/colls/{1}", _settings.DatabaseId, _settings.CollectionId);

                dynamic doc = _client.CreateDocumentQuery<PullList>(pathLink).Where(n => n.Name == name).AsEnumerable().FirstOrDefault();

                if(doc != null)
                {
                    list = doc;
                }
                
            }

            return list;
        }
    }
}
