namespace CoffeeCalculator;

public class CalculatorSettings
{
	/// <summary>
	/// вместимость кофе-машины
	/// </summary>
	public decimal CoffeeMachineCapacity { get; set; }
	/// <summary>
	///  порог остатка, при котором необходимо наполнять кофе-машину
	/// </summary>
	public decimal RefillThreshold { get; set; }
	/// <summary>
	/// текущий остаток в кофемашине 
	/// </summary>
	public decimal CurrentCoffeeAmount { get; set; }
	/// <summary>
	/// базовый расход кофе
	/// </summary>
	public decimal BaseRate { get; set; }
	/// <summary>
	/// коэффициент расхода кофе в день (может меняться, а по дефолту - 1, т.е. без отклонений)
	/// </summary>
	public decimal CoffeeDayRate { get; set; } = 1;
}