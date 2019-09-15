using ElMaitre.DTO;
using System.Collections.Generic;

namespace ElMaitre.Services
{
    public interface IQuestionService
    {
        int Insert(QuestionDTO review);
        void InsertAnswer(QuestionAnswerDTO answer);
        IEnumerable<KeyValueDTO> GetCategories();
        IEnumerable<QuestionDTO> Get(QuestionFilter filter);
        QuestionDTO GetById(int id);
    }
}
