namespace CitiesInformations.Api.Services
{
    public interface IMailService
    {
        public void Send(string Subject, string message);
    }
}
