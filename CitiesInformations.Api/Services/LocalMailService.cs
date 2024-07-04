namespace CitiesInformations.Api.Services
{
    public class LocalMailService : IMailService
    {

        private string _mailTo = string.Empty;

        private string _mailFrom = string.Empty;


        public LocalMailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
        }
        public void Send(string Subject, string message)
        {
            // send mail to console window

            Console.WriteLine($"mail from{_mailFrom} to {_mailTo} with {nameof(LocalMailService)}");

            Console.WriteLine($"Message{message}");
            Console.WriteLine($"Subject{Subject}");
        }
    }
}
