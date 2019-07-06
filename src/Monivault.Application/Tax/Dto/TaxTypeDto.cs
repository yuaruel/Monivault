using Abp.AutoMapper;
using Monivault.AppModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.Tax.Dto
{
    [AutoMapFrom(typeof(TaxType))]
    public class TaxTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
