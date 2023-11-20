using CoffeeCalculator;

var holidays = new List<DateTimeOffset>()
{
	DateTimeOffset.Parse("2024-01-01 +0"),
	DateTimeOffset.Parse("2024-01-02 +0"),
	DateTimeOffset.Parse("2024-01-03 +0"),
	DateTimeOffset.Parse("2024-01-04 +0"),
	DateTimeOffset.Parse("2024-01-05 +0"),
	DateTimeOffset.Parse("2024-01-08 +0"),
	DateTimeOffset.Parse("2024-02-23 +0"),
	DateTimeOffset.Parse("2024-03-08 +0"),
	DateTimeOffset.Parse("2024-04-29 +0"),
	DateTimeOffset.Parse("2024-04-30 +0"),
	DateTimeOffset.Parse("2024-05-01 +0"),
	DateTimeOffset.Parse("2024-05-09 +0"),
	DateTimeOffset.Parse("2024-05-10 +0"),
	DateTimeOffset.Parse("2024-06-12 +0"),
	DateTimeOffset.Parse("2024-11-04 +0"),
	DateTimeOffset.Parse("2024-12-30 +0"),
	DateTimeOffset.Parse("2024-12-31 +0"),
};
var calendar = new Calendar(holidays);

var startDate = DateTimeOffset.Parse("2024-01-04 +0");

var calculatorSettings = new CalculatorSettings
{
	CoffeeMachineCapacity = 1m,
	RefillThreshold = .5m,
	CurrentCoffeeAmount = .5m,
	BaseRate = 1m,
	CoffeeDayRate = 1.5m
};

var calculator = new Calculator(calendar, calculatorSettings);
var sprintsCalendar = calendar.GenerateSprintsCalendar(startDate, 14, 2);
var calculationResult = calculator.CalculateRefillSchedule(sprintsCalendar, 8);

foreach (var sprint in sprintsCalendar)
{
	foreach (var day in sprint)
	{
		Console.Write($"{day:dd.MM.yyyy}");
		if (calculationResult.Schedule.ContainsKey(day))
		{
			Console.WriteLine($" - Кол-во пополнений за день -> {calculationResult.Schedule[day].RefillCount}");
		}
		else
		{
			Console.WriteLine(" - Пополнения не потребуются");
		}
	}

	Console.WriteLine();
}

Console.WriteLine($"Итого пополнено за выбранный период: {calculationResult.TotalRefillAmount}");
Console.WriteLine($"Итого выпито за выбранный период: {calculationResult.TotalRateAmount}");
Console.WriteLine($"Остаток за выбранный период: {calculationResult.CoffeeAmount}");