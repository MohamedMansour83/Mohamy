using ElMaitre.DAL.Models;
using System.Collections;
using System.Collections.Generic;

namespace ElMaitre.DTO
{
    public class SessionNoteDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public int SessionId { get; set; }
        public string UserName { get; set; }
        public string UserNameEn { get; set; }


        public static SessionNote ToSessionNote(SessionNoteDTO note)
        {

            return new SessionNote
            {
                Id = note.Id,
                Title = note.Title,
                UserId = note.UserId,
                SessionId = note.SessionId,
            };
        }

        public static SessionNoteDTO ToSessionNoteDTO(SessionNote note)
        {

            return new SessionNoteDTO
            {
                Id = note.Id,
                Title = note.Title,
                UserId = note.UserId,
                SessionId = note.SessionId,
                UserName = note.User.Name,
                UserNameEn = note.User.NameEn
            };
        }
    }
}
