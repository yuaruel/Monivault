using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.Emailing
{
    public interface IMonivaultMailSender
    {
        void SendUserAccountCreatedMail(string userEmail, string userName, string userPassword, string fullName);
    }
}
