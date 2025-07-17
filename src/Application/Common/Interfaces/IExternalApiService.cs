using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Application.Common.Interfaces;

    public interface IExternalApiService
    {
        Task<string> AuthenticateAsync();
        Task<string> GetTokenAsync();
}

