using System.Threading.Tasks;

namespace Alternet.UI.Integration
{
    internal static class TaskExtensions
    {
        public static void FireAndForget(this Task task)
        {
            _ = task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Log.Error($"Exception caught by FireAndForget: {t.Exception}");
                }
            }, TaskScheduler.Default);
        }
    }
}
