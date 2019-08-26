namespace Msn.InteropDemo.Web.Emailing
{
    public interface IEmailGenerator
    {
        Communication.Emailing.EmailModel GenerateResetPasswordEmail(string resetPasswordCode, string firstName, string lastName, string fromEmailAddess, string toEmailAdrress);
    }
}