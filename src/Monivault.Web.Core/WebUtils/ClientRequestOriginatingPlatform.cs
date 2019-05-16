using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.WebUtils
{
    /// <summary>
    ///     Used to specify the platform, the client request originates from. Will be used in web and api controllers.
    /// </summary>
    public class ClientRequestOriginatingPlatform
    {
        public const string Web = "Web";
        public const string Mobile = "Mobile";
    }
}
