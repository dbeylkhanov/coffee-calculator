namespace CoffeeCalculator;

public class Calendar
{
	public Calendar(List<DateTimeOffset> holidays)
	{
		Holidays = holidays;
	}

	public List<DateTimeOffset> Holidays { get; }

	public List<DateTimeOffset> GetWeekends(DateTimeOffset from, DateTimeOffset to)
	{
		return Enumerable.Range(0, (int) (to - from).TotalDays + 1)
			.Select(x => from.AddDays(x))
			.Where(date => date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday).ToList();
	}

	public HashSet<DateTimeOffset> GetNonWorkingDays(DateTimeOffset from, DateTimeOffset to)
	{
		return GetWeekends(from, to).Union(Holidays).ToHashSet();
	}

	public List<List<DateTimeOffset>> GenerateSprintsCalendar(DateTimeOffset startDate, int sprintLength, int sprintsCount)
	{
		var sprint = Enumerable.Range(0, sprintLength).Select(dayNumber => startDate.AddDays(dayNumber));

		return Enumerable.Range(0, sprintsCount).Select(sprintIndex => sprint.Select(day => day.AddDays(sprintIndex * sprintLength)).ToList()).ToList();
	}

	public decimal CoffeeDayRateForDate(List<DateTimeOffset> sprint, int startDay, decimal coffeeDayRate, DateTimeOffset date)
	{
		var dayIndex = sprint.IndexOf(date);
		if (dayIndex < 0)
		{
			return 0;
		}

		return dayIndex + 1 >= startDay ? coffeeDayRate : 1;
	}
}