using Abp.Dependency;
using Abp.Events.Bus.Exceptions;
using Abp.Events.Bus.Handlers;
using Castle.Core.Logging;

namespace Monivault.Web.ExceptionEventHandler
{
    public class MonivaultExceptionHandler : IEventHandler<AbpHandledExceptionData>, ITransientDependency
    {
        public ILogger Logger { get; set; }

        public MonivaultExceptionHandler()
        {
            Logger = NullLogger.Instance;
        }
        
        public void HandleEvent(AbpHandledExceptionData eventData)
        {
            Logger.Error("Inside monivault event handler for AbpHandledExceptionData");
            Logger.Error(eventData.Exception.StackTrace);   
        }
    }
}