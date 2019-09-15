using ElMaitre.DTO;
using System.Collections.Generic;

namespace ElMaitre.Services
{
    public interface IBlogService
    {
        IEnumerable<BlogDTO> Get();
        BlogDTO Get(int Id);
    }
}
