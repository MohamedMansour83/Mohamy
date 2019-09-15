using ElMaitre.DAL.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ElMaitre.DTO
{
    public class ReplyDTO
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
        [JsonProperty("UserId")]
        public string UserId { get; set; }
        [JsonProperty("ReviewId")]
        public int ReviewId { get; set; }
        public IEnumerable<ReviewDTO> Reviews { get; set; }

        public static ReviewReply ToReviewReply(ReplyDTO reply)
        {

            return new ReviewReply
            {
                Id = reply.Id,
                Title = reply.Title,
                UserId = reply.UserId,
                ReviewId= reply.ReviewId
            };
        }

        public static ReplyDTO ToReviewReplyDTO(ReviewReply reply)
        {

            return new ReplyDTO
            {
                Id = reply.Id,
                Title = reply.Title,
                UserId = reply.UserId,
                UserName=reply.User.UserName,
                Name=reply.User.Name,
                NameEn=reply.User.NameEn,
                ReviewId = reply.ReviewId
            };
        }

    }
}
