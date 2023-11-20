namespace CoffeeCalculator;

public class RefillSchedule
{
	public RefillSchedule(Dictionary<DateTimeOffset, (int RefillCount, decimal CoffeeAmount)> schedule, decimal totalRefillAmount, decimal totalRateAmount, decimal coffeeAmount)
	{
		Schedule = schedule;
		TotalRefillAmount = totalRefillAmount;
		TotalRateAmount = totalRateAmount;
		CoffeeAmount = coffeeAmount;
	}

	public Dictionary<DateTimeOffset, (int RefillCount, decimal CoffeeAmount)> Schedule { get; }

	public decimal TotalRefillAmount { get; }

	public decimal TotalRateAmount { get; }

	public decimal CoffeeAmount { get; }
}