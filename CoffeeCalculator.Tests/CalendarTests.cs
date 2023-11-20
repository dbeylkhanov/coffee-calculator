using FluentAssertions;

namespace CoffeeCalculator.Tests;

public class CalendarTests
{
	private Calendar _calendar;

	[SetUp]
	public void SetUp()
	{
		_calendar = new Calendar(Data.AnnualHolidays);
	}
	
	[Test]
	public void GetsCorrectWeekendDates()
	{
		// Arrange
		var expectedDates = new List<DateTimeOffset>
		{
			DateTimeOffset.Parse("2024-01-06 +0"),
			DateTimeOffset.Parse("2024-01-07 +0"),
			DateTimeOffset.Parse("2024-01-13 +0"),
			DateTimeOffset.Parse("2024-01-14 +0"),
		};

		// Act
		var weekends = _calendar.GetWeekends(DateTimeOffset.Parse("2024-01-01 +0"), DateTimeOffset.Parse("2024-01-14 +0"));

		// Assert
		weekends.Should().Equal(expectedDates);
	}

	[Test]
	public void ReturnsZeroRateIfDateNotFoundInSprint()
	{
		// Arrange
		var sprint = _calendar.GenerateSprintsCalendar(DateTimeOffset.Parse("2024-01-01 +0"), 2, 1);

		// Act
		var coffeeDayRate = _calendar.CoffeeDayRateForDate(sprint[0], 1, 2, DateTimeOffset.Parse("2024-01-03 +0"));

		// Assert
		coffeeDayRate.Should().Be(0);
	}

	[Test]
	public void ReturnsDefaultRateIfDateIsBeforeStartingDay()
	{
		// Arrange
		var sprint = _calendar.GenerateSprintsCalendar(DateTimeOffset.Parse("2024-01-01 +0"), 2, 1);

		// Act
		var coffeeDayRate = _calendar.CoffeeDayRateForDate(sprint[0], 2, 2, DateTimeOffset.Parse("2024-01-01 +0"));

		// Assert
		coffeeDayRate.Should().Be(1);
	}

	[Test]
	public void ReturnsCalculatedRateIfDateIsOrAfterStartingDay()
	{
		// Arrange
		var sprint = _calendar.GenerateSprintsCalendar(DateTimeOffset.Parse("2024-01-01 +0"), 2, 1);

		// Act
		var coffeeDayRate = _calendar.CoffeeDayRateForDate(sprint[0], 2, 1.5m, DateTimeOffset.Parse("2024-01-02 +0"));

		// Assert
		coffeeDayRate.Should().Be(1.5m);
	}
}