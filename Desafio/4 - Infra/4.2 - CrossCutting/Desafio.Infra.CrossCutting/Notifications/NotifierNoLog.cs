﻿namespace Desafio.Infra.CrossCutting.Notifications
{
    public class NotifierNoLog : INotifier
    {
        private List<Notification> _notifications;

        public NotifierNoLog()
        {
            _notifications = new List<Notification>();
        }

        public List<Notification> GetNotifications()
        {
            return _notifications;
        }

        public void Handle(Notification notification)
        {
            _notifications.Add(notification);
        }

        public bool HasNotification()
        {
            return _notifications.Any();
        }
    }
}
