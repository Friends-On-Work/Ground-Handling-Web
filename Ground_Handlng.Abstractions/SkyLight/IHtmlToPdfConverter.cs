using Microsoft.AspNetCore.Hosting;

namespace Ground_Handlng.Abstractions.SkyLight
{
    public interface IHtmlToPdfConverter
    {
        byte[] Convert(string htmlCode, string pdfName, IHostingEnvironment _environment);
    }
}
