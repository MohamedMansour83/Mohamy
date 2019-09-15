using ElMaitre.DTO;
using System.Collections.Generic;

namespace ElMaitre.Services
{
    public interface IDocumentService
    {
        IEnumerable<DocumentDTO> Get(string userId,bool isSent);
        void Insert(DocumentDTO doc);
    }
}
