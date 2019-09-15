using ElMaitre.DTO;
using System.Collections.Generic;

namespace ElMaitre.Services
{
    public interface IReviewService
    {
        void Insert(ReviewDTO review);
        void InsertReply(ReplyDTO reply);
    }
}
