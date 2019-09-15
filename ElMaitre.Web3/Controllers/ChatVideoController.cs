using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElMaitre.Service.DTO;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Jwt.AccessToken;
using Twilio.Rest.Video.V1;
using Twilio.Rest.Video.V1.Room;

namespace ElMaitre.Web.Controllers
{
    public class ChatVideoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Tokenization(string user)
        {
            // These values are necessary for any access token
            //const string twilioAccountSid = "ACe57633cc4cf60308ecd1b3446061cc2a";
            const string twilioApiKey = "SK4403ac62d53601fa6dfd2aaef5efcd14";

            //live
            const string twilioAccountSid = "AC42b1d323b12ebcfd750d1de681d47f80";
            //const string twilioApiKey = "d3fe58223f08177d7d822a74e2945882";

            const string twilioApiSecret = "eDK6QmxGnt1kSlAQe9yvg9lMmIPKeXJC";

            // These are specific to Video
            //const string identity = "user1";

            // Create a Video grant for this token
            var grant = new VideoGrant();
            grant.Room = "my-new-room";

            var grants = new HashSet<IGrant> { grant };

            // Create an Access Token generator
            var token = new Token(
                twilioAccountSid,
                twilioApiKey,
                twilioApiSecret,
                identity: user,
                grants: grants);

            Console.WriteLine(token.ToJwt());
            return Json(new TokenDTO { token = token.ToJwt() });
        }


        public IActionResult Tokenization2(string id, string id2)
        {
            const string twilioApiKey = "SK00736ea2aac54066cc80e230ace02aa1";
            const string twilioAccountSid = "AC42b1d323b12ebcfd750d1de681d47f80";
            const string twilioApiSecret = "GeI7uS6MUgeOx7hRDwZ7N387rtvCmrVf";
            // These are specific to Video
            //const string identity = "user1";
            // Create a Video grant for this token
            var grant = new VideoGrant();
            grant.Room = id + "_room";
            var grants = new HashSet<IGrant> { grant };
            // Create an Access Token generator
            var token = new Token(
                twilioAccountSid,
                twilioApiKey,
                twilioApiSecret,
                identity: id2,
                grants: grants);
            Console.WriteLine(token.ToJwt());
            return Json(new TokenDTO { token = token.ToJwt() });
        }

		public IActionResult Tokenization3(string id)
		{
			const string twilioApiKey = "SK00736ea2aac54066cc80e230ace02aa1";
			const string twilioAccountSid = "AC42b1d323b12ebcfd750d1de681d47f80";
			const string twilioApiSecret = "GeI7uS6MUgeOx7hRDwZ7N387rtvCmrVf";

			// These are specific to Video
			//const string identity = "user1";

			// Create a Video grant for this token
			var grant = new VideoGrant();
			grant.Room = "my-new-room";

			var grants = new HashSet<IGrant> { grant };

			// Create an Access Token generator
			var token = new Token(
				twilioAccountSid,
				twilioApiKey,
				twilioApiSecret,
				identity: id,
				grants: grants);

			Console.WriteLine(token.ToJwt());
			return Json(new TokenDTO { token = token.ToJwt() });
		}

        public IActionResult removeParticepient(List<ParticepirntData> Particepients)
        {
            if(Particepients.Count() != 0)
            {
                for (int i = 0; i < Particepients.Count(); i++)
                {
                    ParticipantResource participant = ParticipantResource.Update(Particepients[i].pathroomSid, Particepients[i].identity, ParticipantResource.StatusEnum.Disconnected);
                }
            }
            return null;
        }

        public JsonResult CreateGroupRoom()
        {
            const string accountSid = "AC42b1d323b12ebcfd750d1de681d47f80";
            const string authToken = "d3fe58223f08177d7d822a74e2945882";

            TwilioClient.Init(accountSid, authToken);

            var room = RoomResource.Create(
                recordParticipantsOnConnect: true,
                statusCallback: new Uri("http://localhost:30309"),
                type: RoomResource.RoomTypeEnum.GroupSmall,
                uniqueName: "my-new-room"
            );

            Console.WriteLine(room.Sid);
            return Json(room.Sid);
        }
    }

    public class ParticepirntData
        {
        public string pathroomSid { get; set; }
        public string identity { get; set; }
        }
}