using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Ground_Handlng.DataObjects.Models.Others
{
    public class IndexModel : PageModel
    {
        public string AssemblyVersion { get; set; }
        public IndexModel()
        {
            AssemblyVersion = typeof(RuntimeEnvironment).GetTypeInfo()
                .Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            //TODO
            //AssemblyVersion = @Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationVersion;
        }
    }
}
