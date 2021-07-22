using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace Ground_Handlng.Abstractions.SkyLight
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model, IHostingEnvironment _environment);
    }
}
