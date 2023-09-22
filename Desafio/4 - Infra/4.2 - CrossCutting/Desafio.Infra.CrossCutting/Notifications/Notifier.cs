using Microsoft.Extensions.Logging;

namespace Desafio.Infra.CrossCutting.Notifications
{
    public class Notifier : INotifier
    {
        readonly ILogger<Notifier> _log;
        private List<Notification> _notifications;

        public Notifier(ILogger<Notifier> log)
        {
            _log = log;
            _notifications = new List<Notification>();
        }

        public List<Notification> GetNotifications()
        {
            return _notifications;
        }

        public void Handle(Notification notification)
        {
            _log.LogInformation(notification.message);
            _notifications.Add(notification);
        }

        public bool HasNotification()
        {
            return _notifications.Any();
        }
    }
}
