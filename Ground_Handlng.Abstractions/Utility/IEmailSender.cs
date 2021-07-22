using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Ground_Handlng.Abstractions.Utility
{
    public interface IEmailSender
    {
        Task<object> SendEmailAsync(string message, List<string> reciever, string subject);
        Task<object> SendEmailWithAttachmentAsync(string message, List<string> recievers, string subject, MemoryStream receiptStream);
    }
}
