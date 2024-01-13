using Core.Application.Models;
using Core.Application.Services;
using Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<JsonResult> GetNotifications()
        {
            ResultT<IEnumerable<EnrollmentNotification>> result = await _notificationService.GetUnSeenEnrollmentNotificationsAsync(AuthenticatedUser.EmployeeId);
            return Json(
                new
                {
                    Success = result.IsSuccess,
                    Message = result.IsSuccess
                    ? "Notifications retrieved successfully"
                    : result.Error.Message ?? "Notifications cannot be retrieved",
                    Result = result.IsSuccess ? new { Notifications = result.Value } : null
                },
                "application/json",
                System.Text.Encoding.UTF8,
                JsonRequestBehavior.AllowGet);
        }
    }
}