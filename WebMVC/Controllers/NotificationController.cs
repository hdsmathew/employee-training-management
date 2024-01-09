using Core.Application.Models;
using Core.Application.Services;
using Core.Domain;
using System.Linq;
using System.Web.Mvc;

namespace WebMVC.Controllers
{
    public class NotificationController : SessionController
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetNotifications()
        {
            ResponseModel<EnrollmentNotification> response = _notificationService.GetUnSeenEnrollmentNotifications(AuthenticatedUser.EmployeeId);
            return Json(
                new
                {
                    Success = response.Success(),
                    Message = response.Success()
                    ? "Notifications retrieved successfully"
                    : response.GetErrors().FirstOrDefault()?.Message ?? "Notifications cannot be retrieved",
                    Result = response.Success() ? new { Notifications = response.Entities } : null
                },
                "application/json",
                System.Text.Encoding.UTF8,
                JsonRequestBehavior.AllowGet);
        }
    }
}