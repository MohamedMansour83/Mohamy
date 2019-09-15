using ElMaitre.DAL.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ElMaitre.DTO
{
    public class ReviewDTO
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("UserName")]
        public string UserName { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("NameEn")]
        public string NameEn { get; set; }
        [JsonProperty("Rate")]
        public int Rate { get; set; }
        [JsonProperty("UserId")]
        public string UserId { get; set; }
        [JsonProperty("ReplyMessage")]
        public string ReplyMessage { get; set; }
        [JsonProperty("Replies")]
        public IEnumerable<ReplyDTO> Replies { get; set; }
        public static Review ToReview(ReviewDTO review)
        {

            return new Review
            {
                Id = review.Id,
                Title = review.Title,
                Rate = review.Rate,
                UserId = review.UserId
            };
        }

        public static ReviewDTO ToReviewDTO(Review review)
        {

            return new ReviewDTO
            {
                Id = review.Id,
                Title = review.Title,
                Rate = review.Rate,
                UserId = review.UserId,
                UserName=review.User.UserName,
                Name=review.User.Name,
                NameEn=review.User.NameEn,
            };
        }

    }
}
