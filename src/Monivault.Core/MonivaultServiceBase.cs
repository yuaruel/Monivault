using Abp;

namespace Monivault
{
    /// <summary>
    /// This class can be used as a base class for services in this application.
    /// It has some useful objects property-injected and has some basic methods most of services may need to.
    /// It's suitable for non domain nor application service classes.
    /// </summary>
    public abstract class MonivaultServiceBase : AbpServiceBase
    {
        protected MonivaultServiceBase()
        {
            LocalizationSourceName = MonivaultConsts.LocalizationSourceName;
        }
        
    }
}