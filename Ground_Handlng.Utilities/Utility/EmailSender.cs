using Ground_Handlng.Abstractions.Utility;
using Ground_Handlng.DataObjects.Models.Others;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Ground_Handlng.Utilities.Utility
{
    public class EmailSender : IEmailSender
    {
        public async Task<object> SendEmailAsync(string message, List<string> recievers, string subject)
        {
            try
            {
                if (recievers?.Count > 0)
                {
                    MailMessage emailMessage = new MailMessage();
                    EmailConfiguration emailConfiguration = new EmailConfiguration
                    {
                        FromEmail = "ethiopianairlinesservices@ethiopianairlines.com",
                        Host = "outlook.office365.com",
                        Password = "EtOffice365"
                    };

                    MailAddress from = new MailAddress(emailConfiguration.FromEmail, "SkyLight Reservation Notification");
                    emailMessage.From = from;

                    foreach (var reciever in recievers)
                        emailMessage.To.Add(reciever);

                    emailMessage.Subject = subject;
                    emailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.Never;
                    emailMessage.Body = message;
                    //emailMessage.CC.Add("reservation@ethiopianskylighthotel.com");
                    //emailMessage.CC.Add("seblewongeld@EthiopianSkyLighthotel.com");
                    //emailMessage.CC.Add("ChenL@EthiopianSkyLighthotel.com");

                    emailMessage.Bcc.Add("mulugetan@ethiopianairlines.com");
                    emailMessage.Bcc.Add("fishat@ethiopianairlines.com");

                    emailMessage.IsBodyHtml = true;
                    SmtpClient smtpClient = new SmtpClient
                    {
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("ethiopianairlinesservices@ethiopianairlines.com", emailConfiguration.Password),
                        Port = 25,
                        Host = emailConfiguration.Host,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };

                    try
                    {
                        smtpClient.SendAsync(emailMessage, null);

                        return new DataObjects.Models.Others.OperationResult
                        {
                            Status = OperationStatus.SUCCESS,
                            Message = Enum.GetName(typeof(OperationStatus), OperationStatus.Ok)
                        };
                    }
                    catch (Exception ex)
                    {
                        return new DataObjects.Models.Others.OperationResult
                        {
                            ex = ex,
                            Status = OperationStatus.ERROR,
                            Message = Enum.GetName(typeof(OperationStatus), OperationStatus.ERROR)
                        };
                    }
                }

                return new DataObjects.Models.Others.OperationResult
                {
                    Status = OperationStatus.NOT_OK,
                    Message = "Reciever email address is not given."
                };
            }
            catch (Exception ex)
            {
                return new DataObjects.Models.Others.OperationResult
                {
                    ex = ex,
                    Message = "Exception Occured on SendEmail",
                    Status = OperationStatus.ERROR
                };
            }
        }

        public async Task<object> SendEmailWithAttachmentAsync(string message, List<string> recievers, string subject, MemoryStream receiptStream)
        {
            try
            {
                if (recievers?.Count > 0)
                {
                    MailMessage emailMessage = new MailMessage();
                    EmailConfiguration emailConfiguration = new EmailConfiguration
                    {
                        FromEmail = "ethiopianairlinesservices@ethiopianairlines.com",
                        Host = "outlook.office365.com",
                        Password = "EtOffice365"
                    };

                    MailAddress from = new MailAddress(emailConfiguration.FromEmail, "SkyLight Reservation Notification");
                    emailMessage.From = from;

                    foreach (var reciever in recievers)
                        emailMessage.To.Add(reciever);

                    emailMessage.Subject = subject;
                    emailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.Never;
                    emailMessage.Body = message;

                    //emailMessage.CC.Add("reservation@ethiopianskylighthotel.com");
                    //emailMessage.Bcc.Add("mulugetan@ethiopianairlines.com");
                    //emailMessage.Bcc.Add("fishat@ethiopianairlines.com");

                    //Attachment file
                    if (receiptStream != null && receiptStream.Length > 0)
                    {
                        receiptStream.Position = 0;
                        Attachment receiptAttachment = new Attachment(receiptStream, new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf));
                        receiptAttachment.ContentDisposition.FileName = "Ethiopian SkyLight Reservation Confirmation.pdf";
                        emailMessage.Attachments.Add(receiptAttachment);
                    }

                    emailMessage.IsBodyHtml = true;
                    SmtpClient smtpClient = new SmtpClient
                    {
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("ethiopianairlinesservices@ethiopianairlines.com", emailConfiguration.Password),
                        Port = 25,
                        Host = emailConfiguration.Host,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };

                    try
                    {
                        smtpClient.SendAsync(emailMessage, null);

                        return new DataObjects.Models.Others.OperationResult
                        {
                            Status = OperationStatus.SUCCESS,
                            Message = Enum.GetName(typeof(OperationStatus), OperationStatus.Ok)
                        };
                    }
                    catch (Exception ex)
                    {
                        return new DataObjects.Models.Others.OperationResult
                        {
                            ex = ex,
                            Status = OperationStatus.ERROR,
                            Message = Enum.GetName(typeof(OperationStatus), OperationStatus.ERROR)
                        };
                    }
                }

                return new DataObjects.Models.Others.OperationResult
                {
                    Status = OperationStatus.NOT_OK,
                    Message = "Reciever email address is not given."
                };
            }
            catch (Exception ex)
            {
                return new DataObjects.Models.Others.OperationResult
                {
                    ex = ex,
                    Message = "Exception Occured on SendEmail",
                    Status = OperationStatus.ERROR
                };
            }
        }
    }
}
