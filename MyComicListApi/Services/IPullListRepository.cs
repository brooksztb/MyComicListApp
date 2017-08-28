using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyComicListApi.Models;

namespace MyComicListApi.Services
{
    public interface IPullListRepository
    {
        Task<string> Add(PullList PullList);
        Task<string> AddComicToList(string Name, Comic Comic);
        Task<string> AddBulkComicsToList(string Name, List<Comic> ComicsToAdd);
        Task<string> UpdateListStatus(string Name, bool Status);
        Task<string> DeleteList(string Name);
        Task<string> DeleteComicFromList(string NameofComic, string Name);
        Task<string> DeleteBulkComicsFromList(List<string> NameofComics, string Name);
        IEnumerable<PullList> GetAllActiveLists();
        PullList FindListByName(string Name);
    }
}
