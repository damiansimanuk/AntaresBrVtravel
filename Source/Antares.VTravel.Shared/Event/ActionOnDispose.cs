namespace Antares.VTravel.Shared.Event;

public class ActionOnDispose(Action onDispose, IDisposable? disposable = null) : IDisposable
{
    public void Dispose()
    {
        onDispose?.Invoke();
        disposable?.Dispose();
    }
}
