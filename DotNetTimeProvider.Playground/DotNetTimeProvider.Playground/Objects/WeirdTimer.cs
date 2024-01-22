namespace DotNetTimeProvider.Playground.Objects;

public class WeirdTimer : ITimer
{
    public void Dispose() => WriteLine($"{nameof(Dispose)} method was triggered.");

    public async ValueTask DisposeAsync()
    {
        WriteLine($"{nameof(DisposeAsync)} method was triggered.");
        await Task.CompletedTask;
    }

    public bool Change(TimeSpan dueTime, TimeSpan period)
    {
        WriteLine($"{nameof(Change)} method was triggered.");
        return true;
    }
}