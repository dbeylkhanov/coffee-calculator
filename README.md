# coffee-calculator
Расчет потребления кофе человеками (с привязкой к спринтам). 

Для расчета потребления кофе требуется инициализировать календарь спринтов (есть возможность учесть необходимые праздники) и передать его в в калькулятор подсчета. Календарь формируется на основе даты отсчета, продолжительности спринта в днях и количестве спринтов. Опционально для калькулятора можно также передать номер дня, который будет влиять на увеличенное потребление кофе (к примеру, с 8го дня в спринте кофе начинают пить больше, чем обычно).

```cs
var calendar = new Calendar(holidays);
var calculator = new Calculator(calendar, calculatorSettings);
var sprintsCalendar = calendar.GenerateSprintsCalendar(startDate, 14, 2); // 2 спринта по 14 дней
var calculationResult = calculator.CalculateRefillSchedule(sprintsCalendar);
```

# Демо
Достаточно запустить проект в консоли, где будет рассчет для демо-примера
