using System.Threading.Tasks;

namespace Ground_Handlng.Web.Interfaces.Utility
{
    public interface ISmsService
    {
        Task SendAsync(string number, string message);
    }
}
