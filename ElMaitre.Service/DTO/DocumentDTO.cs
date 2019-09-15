using ElMaitre.DAL.Models;
using System.Collections;
using System.Collections.Generic;

namespace ElMaitre.DTO
{
    public class DocumentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string Date { get; set; }


        public static Document ToDocument(DocumentDTO doc)
        {

            return new Document
            {
                Id = doc.Id,
                Name = doc.Name,
                Path = doc.Path,
                ToUserId = doc.ToUserId,
                FromUserId = doc.FromUserId
            };
        }

        public static DocumentDTO ToDocumentDTO(Document doc)
        {

            return new DocumentDTO
            {
                Id = doc.Id,
                Name = doc.Name,
                Path = doc.Path,
                FromUserName = doc.FromUser.UserName,
                ToUserName = doc.ToUser.UserName,
                ToUserId = doc.ToUserId,
                FromUserId = doc.FromUserId,
                Date=doc.AddedDate.Date.ToString("dddd, dd MMMM yyyy")
            };
        }
    }
}
