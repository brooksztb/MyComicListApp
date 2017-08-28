using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyComicListApi.Models;

namespace MyComicListApi.Services
{
    public interface IComicRepository
    {
        Comic Add(Comic comic);
        IEnumerable<Comic> GetAll();
        Comic GetById(int id);
        void Delete(Comic comic);
        void Update(Comic comic);
    }
}
