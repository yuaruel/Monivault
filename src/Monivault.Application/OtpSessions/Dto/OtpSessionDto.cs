using System.Collections.Generic;
using Abp.AutoMapper;
using Monivault.AppModels;

namespace Monivault.OtpSessions.Dto
{
    [AutoMap(typeof(OtpSession))]
    public class OtpSessionDto
    {
        public int Id { get; set; }

        public string PhoneNumberSentTo { get; set; }

        public long UserId { get; set; }

        public Dictionary<string, string> ActionProperty { get; set; }
    }
}