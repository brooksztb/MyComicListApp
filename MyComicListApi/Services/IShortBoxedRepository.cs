using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyComicListApi.Models;

namespace MyComicListApi.Services
{
    public interface IShortBoxedRepository
    {
        Task<List<Comic>> GetCurrentWeeksComicsAsync();

        List<Comic> GetCurrentWeeksComicsByPublisher(string Publisher);
    }
}
