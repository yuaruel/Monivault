using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.Emailing
{
    public interface IMonivaultEmailTemplateProvider
    {
        string GetUserAccountCreatedTemplate();
    }
}
