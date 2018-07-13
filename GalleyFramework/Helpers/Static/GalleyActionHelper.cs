using System;
namespace GalleyFramework.Helpers.Static
{
    public static class GalleyActionHelper
    {
        public static Exception ExecuteSuppressExceptions(Action action)
        {
            try
            {
                action.Invoke();
                return null;
            }
            catch(Exception ex)
            {
                return ex;
            }
        }
    }
}
