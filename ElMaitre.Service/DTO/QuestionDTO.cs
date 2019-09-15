using ElMaitre.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace ElMaitre.DTO
{


    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
        public string AnonymousEmail { get; set; }
        public string AnonymousName { get; set; }
        public string Answer { get; set; }
        public bool IsAnswered { get; set; }
        public string ProfileImg { get; set; }


        public IEnumerable<QuestionAnswerDTO> Answers { get; set; }


        public static Question ToQuestion(QuestionDTO quest)
        {

            return new Question
            {
                Id = quest.Id,
                Title = quest.Title,
                CategoryId = quest.CategoryId,
                AnonymousEmail = quest.AnonymousEmail,
                AnonymousName = quest.AnonymousName,
                UserId = quest.UserId,
            };
        }

        public static QuestionDTO ToQuestionDTO(Question quest)
        {

            return new QuestionDTO
            {
                Id = quest.Id,
                Title = quest.Title,
                CategoryId = quest.CategoryId,
                UserId = quest.UserId,
                UserName = quest.User != null ? quest.User.UserName : quest.AnonymousName,
                AnonymousEmail = quest.AnonymousEmail,
                AnonymousName = quest.AnonymousName,
                Answers = quest.Answers.Select(s => QuestionAnswerDTO.ToAnswerDTO(s)),
                IsAnswered=quest.Answers.Any(),
                ProfileImg= quest.User==null || string.IsNullOrEmpty(quest.User.ProfileImg) ? "/images/personal-img.jpg" : quest.User.ProfileImg,
            };
        }

    }


    public class QuestionFilter
    {
        public int? CategoryId { get; set; }
        public int? LawyerId { get; set; }
        public bool LawyerQuestions { get; set; }
        public bool UnAnswerd { get; set; }
    }
}
