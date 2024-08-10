namespace Antares.VTravel.Shared.Core.Event;

public class ActionOnDispose(Action onDispose, IDisposable? disposable = null) : IDisposable
{
    public void Dispose()
    {
        onDispose?.Invoke();
        disposable?.Dispose();
    }
}
