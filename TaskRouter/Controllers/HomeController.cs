using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Twilio.TaskRouter;

namespace TaskRouter.Controllers
{
    [RoutePrefix("home")]
    public class HomeController : ApiController
    {
        private static string AccountSid = ConfigurationManager.AppSettings["AccountSid"];
        private static string AuthToken = ConfigurationManager.AppSettings["AuthToken"];
        private static string WorkspaceSid = ConfigurationManager.AppSettings["WorkspaceSid"];
        private static string WorkflowSid = ConfigurationManager.AppSettings["WorkflowSid"];

        private Twilio.TaskRouter.TaskRouterClient client = new Twilio.TaskRouter.TaskRouterClient(AccountSid, AuthToken);
        public class AssignmentCallback
        {
            public string TaskSid { get; set; }
            public string ReservationSid { get; set; }
        }
        [HttpGet]
        [Route("test")]
        public IHttpActionResult Test()
        {
            return Ok("Hello world");
        }

        [HttpPost]
        [Route("assignment_callback")]
        public IHttpActionResult Assignment_Callback(AssignmentCallback o)
        {
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, new { instruction = "accept" }));
        }

        [HttpGet]
        [Route("createTask")]
        public IHttpActionResult CreateTask()
        {
            Task task = client.AddTask(WorkspaceSid, "{\"selected_language\":\"es\"}", WorkflowSid, null, 0);
            if (task.RestException?.Message != null)
            {
                return BadRequest(task.RestException.Message);
            }
            return Ok($"Created task {task.Attributes + " " + task.Sid} - " + DateTime.Now);
        }

        public class ReservationCallback
        {
            public string TaskSid { get; set; }
            public string ReservationSid { get; set; }
        }
        [HttpPost]
        [Route("accept_reservation")]
        public IHttpActionResult AcceptReservation([FromUri] ReservationCallback callback)
        {
            var taskSid = callback.TaskSid;
            var reservationSid = callback.ReservationSid;
            Reservation reservation = client.UpdateReservation(WorkspaceSid, taskSid, reservationSid, "accepted", null);
            if(reservation.RestException?.Message != null)
            {
                return BadRequest(reservation.RestException.Message);
            }
            return Ok($"AcceptReservation: {reservation.ReservationStatus} " + DateTime.Now);
        }

    }
}
