namespace CitiesInformations.Api.Services
{
    public class CloudMailService : IMailService
    {

        private string _mailTo = string.Empty;

        private string _mailFrom = string.Empty;

        public CloudMailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSettings: mailToAddress"];
            _mailFrom = configuration["mailSettings: mailFromAddress"];
        }
        public void Send(string Subject, string message)
        {
            Console.WriteLine($"mail from{_mailFrom} to {_mailTo} with {nameof(CloudMailService)}");

            Console.WriteLine($"Message{message}");
            Console.WriteLine($"Subject{Subject}");
        }
    }
}
