namespace Desafio.Infra.CrossCutting.Notifications
{
    public class Notification
    {
        public string message { get; }

        public Notification(string message)
        {
            this.message = message;
        }

    }
}
