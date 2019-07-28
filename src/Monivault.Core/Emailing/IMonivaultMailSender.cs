using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.Emailing
{
    public interface IMonivaultMailSender
    {
        void SendUserAccountCreatedMail(string userEmail, string userName, string userPassword, string fullName);
        void SendResetPasswordLink(string userEmail, string resetLink);
        void SendTemporaryPassword(string userEmail, string temporaryPassword);
    }
}
