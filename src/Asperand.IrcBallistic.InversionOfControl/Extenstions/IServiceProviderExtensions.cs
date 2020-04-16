using System;

namespace Asperand.IrcBallistic.InversionOfControl.Extenstions
{
    public static class IServiceProviderExtensions
    {
        public static bool IsRegistered(this IServiceProvider serviceProvider, Type type)
        {
            try
            {
                var service = serviceProvider.GetService(type);
                if (service == null)
                    return false;
                return true;
            }
            catch { /*do nothing... yeah I know...*/ }
            return false;
        }
    }
}