using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Application.Common.Interfaces;
public interface IOAuthAuthenticationService
{
    AuthenticationHeaderValue CreateAuthenticationHeaderValue(HttpMethod httpMethod);
}
