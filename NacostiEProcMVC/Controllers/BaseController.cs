using System.Web.Mvc;
using Enum = NacostiEProcMVC.Models.Enum;

namespace NacostiEProcMVC.Controllers
{
    public abstract class BaseController : Controller
    {
        public void Alert(string message, Enum.NotificationType notificationType)
        {
            var msg = "<script language='javascript'>swal('" + notificationType.ToString().ToUpper() + "', '" + message + "','" + notificationType + "')" + "</script>";
            TempData["notification"] = msg;
        }

        public void Message(string message, Enum.NotificationType notifyType)
        {
            TempData["Notification2"] = message;

            switch (notifyType)
            {
                case Enum.NotificationType.success:
                    TempData["NotificationCSS"] = "alert-box success";
                    break;
                case Enum.NotificationType.error:
                    TempData["NotificationCSS"] = "alert-box errors";
                    break;
                case Enum.NotificationType.warning:
                    TempData["NotificationCSS"] = "alert-box warning";
                    break;

                case Enum.NotificationType.info:
                    TempData["NotificationCSS"] = "alert-box notice";
                    break;
            }
        }
    }
}