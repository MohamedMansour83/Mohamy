using ElMaitre.DAL.Models;
using ElMaitre.DAL.Repositories;
using ElMaitre.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ElMaitre.Services
{
    public class ReviewService : IReviewService
    {
        private IRepository<Lawyer> LawyerRepository;
        private IRepository<Review> ReviewRepository;
        private IRepository<ReviewReply> ReviewReplyRepository;

        public ReviewService(IRepository<Lawyer> LawyerRepository, IRepository<Review> ReviewRepository,
            IRepository<ReviewReply> ReviewReplyRepository)
        {
            this.LawyerRepository = LawyerRepository;
            this.ReviewRepository = ReviewRepository;
            this.ReviewReplyRepository = ReviewReplyRepository;
        }


        public void Insert(ReviewDTO review)
        {
            var revtoAdd = ReviewDTO.ToReview(review);
            ReviewRepository.Insert(revtoAdd);
        }

        public void InsertReply(ReplyDTO reply)
        {
            var repToAdd = ReplyDTO.ToReviewReply(reply);
            ReviewReplyRepository.Insert(repToAdd);
        }
    }
}
