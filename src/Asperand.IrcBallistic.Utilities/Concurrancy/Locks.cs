using System;

namespace Asperand.IrcBallistic.Utilities.Concurrancy
{
    public static class Locks
    {
        public static void SimpleInverseSpinLock(ref bool condition, int timeoutSeconds, Action timeoutAction)
        {
            while (!condition)
            {
                System.Threading.Thread.Sleep(1000);
                timeoutSeconds --;
                if (timeoutSeconds <= 0)
                {
                    timeoutAction();
                    break;
                }
            }
        }
    }
}