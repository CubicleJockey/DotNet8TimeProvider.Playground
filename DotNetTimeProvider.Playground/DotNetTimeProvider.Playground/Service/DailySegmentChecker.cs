using System.Data;

namespace DotNetTimeProvider.Playground.Service;

public class DailySegmentChecker
{
    private readonly TimeProvider timeProvider;
    
    /// <summary>
    /// Ctor that takes in the time provider.
    ///
    /// Normally people dependency injected DateTime to do DateTime testing per method.
    /// This will use the TimeProvider provided only once for the entire object.
    /// </summary>
    /// <param name="timeProvider">TimeProvider object that can be faked for testing or use real-time.</param>
    /// <exception cref="ArgumentNullException">Time Provider is empty.</exception>
    public DailySegmentChecker(TimeProvider timeProvider)
    {
        this.timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    public bool IsMorning()
    {
        throw new NotImplementedException("Oh noes n' stuff!");
    }

    public bool IsAfternoon()
    {
        throw new NotImplementedException("Oh noes n' stuff!");
    }

    public bool IsEvening()
    {
        throw new NotImplementedException("Oh noes n' stuff!");
    }

    public bool IsNight()
    {
        throw new NotImplementedException("Oh noes n' stuff!");
    }
}