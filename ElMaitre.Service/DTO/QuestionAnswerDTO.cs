using ElMaitre.DAL.Models;
using System.Collections.Generic;

namespace ElMaitre.DTO
{
    public class QuestionAnswerDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public int QuestionId { get; set; }
        public string UserId { get; set; }
        public string ProfileImg { get; set; }


        public static QuestionAnswer ToAnswer(QuestionAnswerDTO quest)
        {

            return new QuestionAnswer
            {
                Id = quest.Id,
                Title = quest.Title,
                QuestionId = quest.QuestionId,
                UserId = quest.UserId,
            };
        }

        public static QuestionAnswerDTO ToAnswerDTO(QuestionAnswer quest)
        {

            return new QuestionAnswerDTO
            {
                Id = quest.Id,
                Title = quest.Title,
                QuestionId = quest.QuestionId,
                UserId = quest.UserId,
                UserName=quest.User.UserName,
                ProfileImg = quest.User == null || string.IsNullOrEmpty(quest.User.ProfileImg) ? "/images/personal-img.jpg" : quest.User.ProfileImg,
            };
        }

    }
}
