﻿using Abp.AutoMapper;
using Monivault.Authentication.External;

namespace Monivault.AppModels.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
