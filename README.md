# coffee-calculator
Расчет потребления кофе человеками (с привязкой к спринтам). 

Для расчета потребления кофе требуется инициализировать
- календарь спринтов (есть возможность учесть необходимые праздники). Календарь формируется на основе даты отсчета, продолжительности спринта в днях и количестве спринтов. Опционально для калькулятора можно также передать номер дня, который будет влиять на увеличенное потребление кофе (к примеру, с 8го дня в спринте кофе начинают пить больше, чем обычно).
- настройки калькулятора
```cs
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
public decimal CoffeeDayRate { get; set; }
```

Проинициализированные объекты календаря и настроек калькулятора требуется передать непосредственно в калькулятор для подсчета потребления кофе.

Пример:
```cs
var calendar = new Calendar(holidays);
var calculator = new Calculator(calendar, calculatorSettings);
var sprintsCalendar = calendar.GenerateSprintsCalendar(startDate, 14, 2); // 2 спринта по 14 дней
var calculationResult = calculator.CalculateRefillSchedule(sprintsCalendar);
```

# Демо
Достаточно запустить проект в консоли, где будет рассчет для демо-примера
