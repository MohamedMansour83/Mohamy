using ElMaitre.DAL.Models;
using ElMaitre.DAL.Repositories;
using ElMaitre.DTO;
using ElMaitre.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ElMaitre.Services
{
    public class DocumentService : IDocumentService
    {
        private IRepository<Document> DocumentRepository;

        public DocumentService(IRepository<Document> DocumentRepository)
        {
            this.DocumentRepository = DocumentRepository;
        }

        public IEnumerable<DocumentDTO> Get(string userId, bool isSent)
        {
            Expression<Func<Document, bool>> filter = s => s.FromUserId == userId;

            if (!isSent)
            {
                filter = s => s.ToUserId == userId;
            }
            return DocumentRepository.Get(filter, includeProperties: "FromUser,ToUser").Select(s=> DocumentDTO.ToDocumentDTO(s));
        }

        public void Insert(DocumentDTO doc)
        {
            var docToAdd = DocumentDTO.ToDocument(doc);
            DocumentRepository.Insert(docToAdd);
        }
    }
}
