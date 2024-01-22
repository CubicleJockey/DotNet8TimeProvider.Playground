using DotNetTimeProvider.Playground.Objects;

namespace DotNetTimeProvider.Playground;

[TestClass]
public class ITimerPlayground
{
    [DataRow(default)]
    [DataRow("Some Kind Of State Object")]
    [DataTestMethod]
    public void SystemTimer(string state)
    {
        var timeProvider = TimeProvider.System;
        
        TimerCallback callback = TimerCallbackMethod;
        using var timer = timeProvider.CreateTimer(callback, state, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(5));
        
        //Start the timer
        var changed = timer.Change(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(5));
        changed.Should().BeTrue();

        // Wait for the timer to elapse, so it can report to the Test Runner Console.
        using var autoResetEvent = new AutoResetEvent(false);
        autoResetEvent.WaitOne(TimeSpan.FromSeconds(15));
    }

    [TestMethod]
    public void FakedTimer()
    {
        var fakeTimeProvider = A.Fake<TimeProvider>();
        var createTimerFunc =
            A.CallTo(() => fakeTimeProvider.CreateTimer(
                  A<TimerCallback>.Ignored
                , A<object?>.Ignored
                , A<TimeSpan>.Ignored
                , A<TimeSpan>.Ignored)
            );

        createTimerFunc.Returns(new WeirdTimer());

        TimerCallback callback = TimerCallbackMethod;
        var timer = fakeTimeProvider.CreateTimer(callback, A.Dummy<object?>(), A.Dummy<TimeSpan>(), A.Dummy<TimeSpan>());

        //Check Console for message.
        timer.Change(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(5));
        
        createTimerFunc.MustHaveHappenedOnceExactly();
    }
    
    #region Helper Methods

    private static void TimerCallbackMethod(object? state)
    {
        if (state == default)
        {
            WriteLine("No Timer State was provided.");
            return;
        }
        
        WriteLine($"Timer elapsed: State: {state}");
    }
    
    #endregion Helper Methods
}