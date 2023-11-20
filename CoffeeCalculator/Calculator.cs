namespace CoffeeCalculator;

public class Calculator
{
	private readonly CalculatorSettings _calculatorSettings;
	public Calendar Calendar { get; }

	public Calculator(Calendar calendar, CalculatorSettings calculatorSettings)
	{
		_calculatorSettings = calculatorSettings;
		Calendar = calendar;
	}

	private RefillSchedule CalculateRefillScheduleBySprint(List<DateTimeOffset> sprint, int increasedCoffeeRateStartDay = 0)
	{
		decimal totalRefillAmount = 0;
		decimal totalRateAmount = 0;
		decimal currentAmount = _calculatorSettings.CurrentCoffeeAmount;
		var refillSchedule = new Dictionary<DateTimeOffset, (int RefillCount, decimal CoffeeAmount)>();
		var nonWorkingDays = Calendar.GetNonWorkingDays(sprint.First(), sprint.Last());

		foreach (var day in sprint)
		{
			if (nonWorkingDays.Contains(day))
			{
				continue;
			}

			var resultCoffeeDayRate = Calendar.CoffeeDayRateForDate(sprint, increasedCoffeeRateStartDay, _calculatorSettings.CoffeeDayRate, day);

			var effectiveRate = _calculatorSettings.BaseRate * resultCoffeeDayRate;

			var remainingRate = effectiveRate;
			totalRateAmount += remainingRate;
			var refillCount = 0;
			while (true)
			{
				decimal refillAmount = 0;
				if (currentAmount <= _calculatorSettings.RefillThreshold)
				{
					refillCount++;
					refillAmount = _calculatorSettings.CoffeeMachineCapacity - currentAmount;
					totalRefillAmount += refillAmount;
					currentAmount += refillAmount;
				}
				var availableToSpend = currentAmount - _calculatorSettings.RefillThreshold;
				if (remainingRate <= availableToSpend)
				{
					currentAmount -= remainingRate;
					remainingRate = 0;
				}
				else
				{
					currentAmount = _calculatorSettings.RefillThreshold;
					remainingRate -= availableToSpend;
				}
				if (refillAmount == 0 && remainingRate == 0)
				{
					break;
				}
			}

			refillSchedule[day] = (refillCount, currentAmount);
		}

		return new RefillSchedule(refillSchedule, totalRefillAmount, totalRateAmount, currentAmount);
	}

	public RefillSchedule CalculateRefillSchedule(List<List<DateTimeOffset>> sprintsCalendar, int increasedCoffeeRateStartDay = 0)
	{
		var totalRefillAmount = 0m;
		var totalRateAmount = 0m;
		var coffeeAmount = _calculatorSettings.CurrentCoffeeAmount;
		var schedule = sprintsCalendar.Aggregate(new Dictionary<DateTimeOffset, (int, decimal)>(), (result, sprint) =>
		{
			var calculationResult = CalculateRefillScheduleBySprint(sprint, increasedCoffeeRateStartDay);
			totalRefillAmount += calculationResult.TotalRefillAmount;
			totalRateAmount += calculationResult.TotalRateAmount;
			coffeeAmount = calculationResult.CoffeeAmount;
			foreach (var item in calculationResult.Schedule)
			{
				result[item.Key] = item.Value;
			}
			return result;
		});

		return new RefillSchedule(schedule, totalRefillAmount, totalRateAmount, coffeeAmount);
	}
}