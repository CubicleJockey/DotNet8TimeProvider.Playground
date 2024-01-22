using DotNetTimeProvider.Playground.Service;
using System;

namespace DotNetTimeProvider.Playground;

[TestClass]
public class TimeProviderPlayground
{
    private readonly TimeProvider fakeTimeProvider = A.Fake<TimeProvider>();

    [TestCleanup]
    public void TestCleanup() => Fake.ClearRecordedCalls(fakeTimeProvider);
    
    
    [TestMethod]
    [Description("Because this is based on system the results are based on whoever is running the test and when.")]
    public void TimeProviderUsingStandardSystem()
    {
        var service = new DailySegmentChecker(TimeProvider.System);
        
        var prompt = $"""
                      Morning: {service.IsMorning()}
                      Afternoon: {service.IsAfternoon()}
                      Evening: {service.IsEvening()}
                      Night: {service.IsNight()}
                      """;
        
        WriteLine(prompt.Trim());
    }

    [DataRow("2024-01-22T00:00:00Z", true)]  // 0 AM UTC
    [DataRow("2024-01-22T01:00:00Z", true)]  // 1 AM UTC
    [DataRow("2024-01-22T02:00:00Z", true)]  // 2 AM UTC
    [DataRow("2024-01-22T03:00:00Z", true)]  // 3 AM UTC
    [DataRow("2024-01-22T04:00:00Z", true)]  // 4 AM UTC
    [DataRow("2024-01-22T05:00:00Z", true)]  // 5 AM UTC
    [DataRow("2024-01-22T06:00:00Z", true)]  // 6 AM UTC
    [DataRow("2024-01-22T07:00:00Z", true)]  // 7 AM UTC
    [DataRow("2024-01-22T08:00:00Z", true)]  // 8 AM UTC
    [DataRow("2024-01-22T09:00:00Z", true)]  // 9 AM UTC
    [DataRow("2024-01-22T10:00:00Z", true)]  // 10 AM UTC
    [DataRow("2024-01-22T11:00:00Z", true)]  // 11 AM UTC
    [DataRow("2024-01-22T12:00:00Z", false)] // 12 PM UTC
    [DataTestMethod]
    public void IsMorning_Utc(string utcTime, bool expected)
    {
        TestUtcMethod(utcTime, expected, fakeTimeProvider, (timeProvider) =>
        {
            var service = new DailySegmentChecker(timeProvider);

            //Act
            var result = service.IsMorning();
            return result;
        });
    }

    [DataRow("2024-01-22T00:00:00Z", false)]  // 0 AM UTC,  4 PM (previous day) PST
    [DataRow("2024-01-22T01:00:00Z", false)]  // 1 AM UTC,  5 PM (previous day) PST
    [DataRow("2024-01-22T02:00:00Z", false)]  // 2 AM UTC,  6 PM (previous day) PST
    [DataRow("2024-01-22T03:00:00Z", false)]  // 3 AM UTC,  7 PM (previous day) PST
    [DataRow("2024-01-22T04:00:00Z", false)]  // 4 AM UTC,  8 PM (previous day) PST
    [DataRow("2024-01-22T05:00:00Z", false)]  // 5 AM UTC,  9 PM (previous day) PST
    [DataRow("2024-01-22T06:00:00Z", false)]  // 6 AM UTC,  10 PM (previous day) PST
    [DataRow("2024-01-22T07:00:00Z", false)]  // 7 AM UTC,  11 PM (previous day) PST
    [DataRow("2024-01-22T08:00:00Z", true)]   // 8 AM UTC,  12 AM PST
    [DataRow("2024-01-22T09:00:00Z", true)]   // 9 AM UTC,  1 AM PST
    [DataRow("2024-01-22T10:00:00Z", true)]   // 10 AM UTC, 2 AM PST
    [DataRow("2024-01-22T11:00:00Z", true)]   // 11 AM UTC, 3 AM PST
    [DataRow("2024-01-22T12:00:00Z", true)]   // 12 PM UTC, 4 AM PST
    [DataTestMethod]
    public void IsMorning_LocalPst(string utcTime, bool expected)
    {
       TestLocalPstMethod(utcTime, expected, fakeTimeProvider,(timeProvider) =>
       {
           var service = new DailySegmentChecker(timeProvider);

           //Act
           var result = service.IsMorning(true);
           return result;
       });
    }

    [DataRow("2024-01-22T12:00:00Z", true)]
    [DataRow("2024-01-22T16:59:00Z", true)]
    [DataRow("2024-01-22T17:00:00Z", false)]
    [DataTestMethod]
    public void IsAfternoon_Utc(string utcTime, bool expected)
    {
        TestUtcMethod(utcTime, expected, fakeTimeProvider, timeProvider =>
        {
            var service = new DailySegmentChecker(timeProvider);
            var result = service.IsAfternoon();
            return result;
        });
    }

    [DataRow("2024-01-22T12:00:00Z", false)] // 12 PM UTC, 4 AM PST
    [DataRow("2024-01-22T16:59:00Z", false)] // 4:59 PM UTC, 8:59 AM PST
    [DataRow("2024-01-22T17:00:00Z", false)] // 5 PM UTC, 9 AM PST
    [DataRow("2024-01-22T20:00:00Z", true)]  // 8 PM UTC, 12 PM PST
    [DataTestMethod]
    public void IsAfternoon_LocalPst(string utcTime, bool expected)
    {
        TestLocalPstMethod(utcTime, expected, fakeTimeProvider, timeProvider =>
        {
            var service = new DailySegmentChecker(timeProvider);
            var result = service.IsAfternoon(true);
            return result;
        });
    }

    [DataRow("2024-01-22T17:00:00Z", true)]
    [DataRow("2024-01-22T20:59:00Z", true)]
    [DataRow("2024-01-22T21:00:00Z", false)]
    [DataTestMethod]
    public void IsEvening_Utc(string utcTime, bool expected)
    {
        TestUtcMethod(utcTime, expected, fakeTimeProvider, timeProvider =>
        {
            var service = new DailySegmentChecker(timeProvider);
            var result = service.IsEvening();
            return result;
        });
    }

    [DataRow("2024-01-22T17:00:00Z", false)]  // 5 PM UTC, 9 AM PST
    [DataRow("2024-01-22T20:59:00Z", false)]  // 8:59 PM UTC, 12:59 PM PST
    [DataRow("2024-01-22T21:00:00Z", false)]  // 9 PM UTC, 1 PM PST
    [DataRow("2024-01-23T01:00:00Z", true)]   // 1 AM UTC, 5 PM PST
    [DataRow("2024-01-23T02:00:00Z", true)]   // 2 AM UTC, 6 PM PST
    [DataRow("2024-01-23T03:00:00Z", true)]   // 3 AM UTC, 7 PM PST
    [DataRow("2024-01-23T04:00:00Z", true)]   // 4 AM UTC, 8 PM PST
    [DataTestMethod]
    public void IsEvening_LocalPst(string utcTime, bool expected)
    {
        TestLocalPstMethod(utcTime, expected, fakeTimeProvider, timeProvider =>
        {
            var service = new DailySegmentChecker(timeProvider);
            var result = service.IsEvening(true);
            return result;
        });
    }

    [DataRow("2024-01-22T21:00:00Z", true)]
    [DataRow("2024-01-23T00:00:00Z", false)]
    [DataTestMethod]
    public void IsNight_Utc(string utcTime, bool expected)
    {
        TestUtcMethod(utcTime, expected, fakeTimeProvider, timeProvider =>
        {
            var service = new DailySegmentChecker(timeProvider);
            var result = service.IsNight();
            return result;
        });
    }

    [DataRow("2024-01-22T21:00:00Z", false)] // 9 PM UTC, 1 PM PST
    [DataRow("2024-01-23T00:00:00Z", false)] // 12 AM (midnight) UTC, 4 PM PST
    [DataRow("2024-01-23T05:00:00Z", true)]  // 5 AM UTC, 9 PM PST (January 22, 2024)
    [DataRow("2024-01-23T06:00:00Z", true)]  // 6 AM UTC, 10 PM PST (January 22, 2024)
    [DataRow("2024-01-23T07:00:00Z", true)]  // 7 AM UTC, 11 PM PST (January 22, 2024)
    [DataTestMethod]
    public void IsNight_LocalPst(string utcTime, bool expected)
    {
        TestLocalPstMethod(utcTime, expected, fakeTimeProvider, timeProvider =>
        {
            var service = new DailySegmentChecker(timeProvider);
            var result = service.IsNight(true);
            return result;
        });
    }

    #region Helper Methods

    private static void TestUtcMethod(string utcTime, bool expected, TimeProvider timeProvider, Func<TimeProvider, bool> segmentCheckFunc)
    {
        //Arrange
        var mockDateTime = DateTime.Parse(utcTime).ToUniversalTime();

        var getUtcNow = A.CallTo(() => timeProvider.GetUtcNow());
        getUtcNow.Returns(mockDateTime);

        //Act
        var result = segmentCheckFunc(timeProvider);

        //Assert
        result.Should().Be(expected);
        getUtcNow.MustHaveHappenedOnceExactly();
    }

    private static void TestLocalPstMethod(string utcTime, bool expected, TimeProvider timeProvider, Func<TimeProvider, bool> segmentCheckFunc)
    {
        //Arrange
        var mockDateTime = DateTime.Parse(utcTime).ToUniversalTime();

        var getUtcNow = A.CallTo(() => timeProvider.GetUtcNow());
        getUtcNow.Returns(mockDateTime);

        var pstTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
        var getLocalTimeZoneFunc = A.CallTo(() => timeProvider.LocalTimeZone);
        getLocalTimeZoneFunc.Returns(pstTimeZone);

        //Act
        var result = segmentCheckFunc(timeProvider);

        //Assert
        result.Should().Be(expected);
        getUtcNow.MustHaveHappenedOnceExactly();
        getLocalTimeZoneFunc.MustHaveHappenedOnceExactly();
    }
    
    #endregion Helper Methods
}