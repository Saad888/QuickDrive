using System;
using System.Collections.Generic;
using System.Text;

namespace QuickDrive.Exceptions
{
    public class OAuthRefusedException : Exception { }
    public class RefreshTokenFailedException : Exception { public RefreshTokenFailedException(string m, Exception inner) : base(m, inner) { } }
    public class HttpResponseFailedException : Exception { public HttpResponseFailedException(string m) : base(m) { } }
    public class FileUploadFailedException : Exception { public FileUploadFailedException(string m, Exception inner) : base(m, inner) { } }
}
