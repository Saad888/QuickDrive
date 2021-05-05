using QuickDrive.ExternalServices.Authentication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickDrive.ExternalServices.Authentication
{
    public interface IAccessTokenReader
    {
        Task<Token> GetAccessToken(AuthCredentialsModel credentials, DriveServices service);
    }
}