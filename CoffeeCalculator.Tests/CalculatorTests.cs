using FluentAssertions;

namespace CoffeeCalculator.Tests;

[Parallelizable]
public class CalculatorTests
{
	private Calculator _calculator;
	private CalculatorSettings _calculatorSettings;

	[SetUp]
	public void SetUp()
	{
		var calendar = new Calendar(Data.AnnualHolidays);
		_calculatorSettings = new CalculatorSettings();
		_calculator = new Calculator(calendar, _calculatorSettings);
	}

	[Test]
	public void CalculatorDoesNotIncludeHolidaysAndWeekends()
	{
		// Arrange
		var sprintSchedule = _calculator.Calendar.GenerateSprintsCalendar(DateTimeOffset.Parse("2024-01-01 +0"), 14, 1);
		var expectedDates = new List<DateTimeOffset>
		{
			DateTimeOffset.Parse("2024-01-09 +0"),
			DateTimeOffset.Parse("2024-01-10 +0"),
			DateTimeOffset.Parse("2024-01-11 +0"),
			DateTimeOffset.Parse("2024-01-12 +0"),
		};

		_calculatorSettings.CoffeeMachineCapacity = 1;
		_calculatorSettings.BaseRate = 1;

		// Act
		var refillSchedule = _calculator.CalculateRefillSchedule(sprintSchedule, 1);

		// Assert
		refillSchedule.Schedule.Keys.Should().Equal(expectedDates);
	}

	[Test]
	public void CalculatorRefillsUpCoffeeMachineToFullCapacityIfThresholdReached()
	{
		// Arrange
		var sprint = _calculator.Calendar.GenerateSprintsCalendar(DateTimeOffset.Parse("2024-01-09 +0"), 1, 1);
		_calculatorSettings.CoffeeMachineCapacity = 1m;
		_calculatorSettings.RefillThreshold = .5m;
		_calculatorSettings.CurrentCoffeeAmount = .5m;

		var expectedCoffeeAmount = _calculatorSettings.CoffeeMachineCapacity;
		var expectedScheduleLength = 1;
		var expectedRefillCount = 1;
		var expectedRefillAmount = .5m;
		var expectedRateAmount = 0m;

		// Act
		var calculationResult = _calculator.CalculateRefillSchedule(sprint);

		// Assert
		calculationResult.TotalRefillAmount.Should().Be(expectedRefillAmount);
		calculationResult.TotalRateAmount.Should().Be(expectedRateAmount);
		calculationResult.CoffeeAmount.Should().Be(expectedCoffeeAmount);
		calculationResult.Schedule.Count.Should().Be(expectedScheduleLength);
		calculationResult.Schedule.Single().Value.CoffeeAmount.Should().Be(expectedCoffeeAmount);
		calculationResult.Schedule.Single().Value.RefillCount.Should().Be(expectedRefillCount);
	}

	[Test]
	public void CalculatorCalculatesRefillScheduleBasedOnSpecifiedPeriod()
	{
		// Arrange
		var sprint = _calculator.Calendar.GenerateSprintsCalendar(DateTimeOffset.Parse("2024-01-04 +0"), 14, 1);
		_calculatorSettings.CoffeeMachineCapacity = 1m;
		_calculatorSettings.RefillThreshold = .5m;
		_calculatorSettings.CurrentCoffeeAmount = .5m;
		_calculatorSettings.BaseRate = 1m;
		_calculatorSettings.CoffeeDayRate = 1.5m;

		var expectedRefillAmount = 10m;
		var expectedRateAmount = 9.5m;
		var expectedCoffeeAmount = 1m;
		var expectedSchedule = new Dictionary<DateTimeOffset, (int, decimal)>
		{
			[DateTimeOffset.Parse("2024-01-09 +0")] = (3, 1m),
			[DateTimeOffset.Parse("2024-01-10 +0")] = (2, 1m),
			[DateTimeOffset.Parse("2024-01-11 +0")] = (3, 1m),
			[DateTimeOffset.Parse("2024-01-12 +0")] = (3, 1m),
			[DateTimeOffset.Parse("2024-01-15 +0")] = (3, 1m),
			[DateTimeOffset.Parse("2024-01-16 +0")] = (3, 1m),
			[DateTimeOffset.Parse("2024-01-17 +0")] = (3, 1m),
		};

		// Act
		var calculationResult = _calculator.CalculateRefillSchedule(sprint, 8);

		// Assert
		calculationResult.TotalRefillAmount.Should().Be(expectedRefillAmount);
		calculationResult.TotalRateAmount.Should().Be(expectedRateAmount);
		calculationResult.CoffeeAmount.Should().Be(expectedCoffeeAmount);
		calculationResult.Schedule.Should().Equal(expectedSchedule);
	}
}