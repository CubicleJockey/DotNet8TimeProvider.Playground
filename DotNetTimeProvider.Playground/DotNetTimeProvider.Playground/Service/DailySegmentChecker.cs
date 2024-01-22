namespace DotNetTimeProvider.Playground.Service;

public class DailySegmentChecker
{
    private readonly TimeProvider timeProvider;
    
    /// <summary>
    /// Ctor that takes in the time provider.
    ///
    /// Normally people dependency injected DateTime to do DateTime testing per method.
    /// This will use the TimeProvider provided only once for the entire object.
    ///
    ///
    /// NOTE: The method timeframes can shift depending on who is using it or the season. These timeframes
    ///       are picked purely for demonstration purposes.
    /// 
    /// </summary>
    /// <param name="timeProvider">TimeProvider object that can be faked for testing or use real-time.</param>
    /// <exception cref="ArgumentNullException">Time Provider is empty.</exception>
    public DailySegmentChecker(TimeProvider timeProvider)
    {
        this.timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    /// <summary>
    /// Morning is 12:00AM - 12:00PM
    /// </summary>
    /// <returns>True if in morning timeframe, else false.</returns>
    public bool IsMorning(bool useLocal = false)
    {
        var now = GetNow(useLocal);
        return now.Hour is >= 0 and < 12;
    }

    

    /// <summary>
    /// Afternoon is 12:00PM - 5:00PM
    /// </summary>
    /// <returns>True if in afternoon timeframe, else false.</returns>
    public bool IsAfternoon(bool useLocal = false)
    {
        var now = GetNow(useLocal);
        return now.Hour is >= 12 and < 17;
    }

    /// <summary>
    /// Evening is 5:00PM - 9:00PM
    /// </summary>
    /// <returns>True if it is evening timeframe, else false.</returns>
    public bool IsEvening(bool useLocal = false)
    {
        var now = GetNow(useLocal);
        return now.Hour is >= 17 and < 21;
    }

    /// <summary>
    /// Night is 9:00PM - 12:00AM
    /// </summary>
    /// <returns>True if it is night timeframe, else false.</returns>
    public bool IsNight(bool useLocal = false)
    {
        var now = GetNow(useLocal);
        return now.Hour is >= 21 or < 0;
    }

    #region Helper Methods

    private DateTimeOffset GetNow(bool useLocal)
    {
        return useLocal ? timeProvider.GetLocalNow() : timeProvider.GetUtcNow();
    }

    #endregion Helper Methods
}