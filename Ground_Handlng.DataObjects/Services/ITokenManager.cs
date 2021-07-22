using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ground_Handlng.DataObjects.Services
{
    public interface ITokenManager
    {
        Task<bool> IsCurrentActiveToken();
        Task DeactivateCurrentAsync();
        Task<bool> IsActiveAsync(string token);
        Task DeactivateAsync(string token);
    }
}
