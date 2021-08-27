using BusinessLogic.BindingModels;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace BusinessLogic.Logic
{
    public class MailLogic
    {
        public MailLogic()
        {
        }

        public void sendMailToClient(ClientBindingModel recipient)
        {
            SmtpClient client = CreateClient();
            string basis = "Отчет по затратам на Ваши запросы";

            MailMessage message = CreateMsg(basis, recipient.ClientLogin);

            string path = "c:/kurs/clientReport.pdf";
            Attachment attach = CreateAtt(path);

            message.Attachments.Add(attach);

            client.Send(message);
        }

        public void sendMailToWorker(WorkerBindingModel recipient)
        {
            SmtpClient client = CreateClient();

            string basis = "Отчет по оплатам за работы";

            MailMessage message = CreateMsg(basis, recipient.WorkerLogin);

            string path = "c:/kurs/workerReport.pdf";
            Attachment attach = CreateAtt(path);

            message.Attachments.Add(attach);

            client.Send(message);
        }

        private MailMessage CreateMsg(string info, string address)
        {
            MailMessage msg = new MailMessage();

            msg.Subject = info;
            msg.Body = info;
            //выша почта
            msg.From = new MailAddress("shovkanyanforlab@gmail.com");
            msg.To.Add(address);
            msg.IsBodyHtml = true;

            return msg;
        }

        private Attachment CreateAtt(string path)
        {
            Attachment attach = new Attachment(path, MediaTypeNames.Application.Octet);

            ContentDisposition disposition = attach.ContentDisposition;

            disposition.CreationDate = System.IO.File.GetCreationTime(path);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(path);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(path);

            return attach;
        }

        private SmtpClient CreateClient()
        {
            SmtpClient client = new SmtpClient();

            client.Host = "smtp.gmail.com";
            client.Port = int.Parse("587");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            //выша почта
            client.Credentials = new NetworkCredential("shovkanyanforlab@gmail.com", "yanshov2001");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            return client;
        }

    }
}