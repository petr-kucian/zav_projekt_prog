using System.Collections.Generic;
using System.Threading.Tasks;
using Zav_projekt.Models;

namespace Zav_projekt.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAll();
}