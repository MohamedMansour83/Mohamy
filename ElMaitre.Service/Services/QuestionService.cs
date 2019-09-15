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
    public class QuestionService : IQuestionService
    {
        private IRepository<Lawyer> LawyerRepository;
        private IRepository<Question> QuestionRepository;
        private IRepository<QuestionCategory> QuestionCategoryRepository;
        private IRepository<QuestionAnswer> QuestionAnswerRepository;

        public QuestionService(IRepository<Lawyer> LawyerRepository, IRepository<Question> QuestionRepository, IRepository<QuestionAnswer> QuestionAnswerRepository,
            IRepository<QuestionCategory> QuestionCategoryRepository)
        {
            this.LawyerRepository = LawyerRepository;
            this.QuestionRepository = QuestionRepository;
            this.QuestionCategoryRepository = QuestionCategoryRepository;
            this.QuestionAnswerRepository = QuestionAnswerRepository;
        }

        public IEnumerable<QuestionDTO> Get(QuestionFilter questionfilter)
        {
            Expression<Func<Question, bool>> filter = s => s.Title != null;

            if (questionfilter.CategoryId.HasValue)
            {
                Expression<Func<Question, bool>> filterToAppend = s => s.CategoryId == questionfilter.CategoryId;
                filter = filter.And(filterToAppend);
            }
            if (questionfilter.LawyerQuestions)
            {
                Expression<Func<Question, bool>> filterToAppend = s => s.Answers.Any(w=>w.User.LawyerId== questionfilter.LawyerId);
                filter = filter.And(filterToAppend);
            }

            if (questionfilter.UnAnswerd)
            {
                Expression<Func<Question, bool>> filterToAppend = s => !s.Answers.Any();
                filter = filter.And(filterToAppend);
            }

            return QuestionRepository.Get(filter,includeProperties: "User,Answers,Answers.User").Select(s=>QuestionDTO.ToQuestionDTO(s));
        }
        public QuestionDTO GetById(int id)
        {
            var quest= QuestionRepository.Get(s=>s.Id==id,includeProperties: "User,Answers,Answers.User").FirstOrDefault();
            return QuestionDTO.ToQuestionDTO(quest);
        }

        public IEnumerable<KeyValueDTO> GetCategories()
        {
            return QuestionCategoryRepository.Get().Select(s => new KeyValueDTO { Id = s.Id, Value = s.Title, ValueEn=s.TitleEn });
        }

        public int Insert(QuestionDTO quest)
        {
            var questtoAdd = QuestionDTO.ToQuestion(quest);
            QuestionRepository.Insert(questtoAdd);

            return questtoAdd.Id;
        }

        public void InsertAnswer(QuestionAnswerDTO answer)
        {
            if (!QuestionAnswerRepository.Get(a => a.QuestionId == answer.QuestionId).Any())
                QuestionAnswerRepository.Insert(QuestionAnswerDTO.ToAnswer(answer));
        }
    }
}
