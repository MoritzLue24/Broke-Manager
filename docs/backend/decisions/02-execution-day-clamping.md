**Status:** Closed

## 1. Context
In order to track when a transaction of a standing order is expected, we store recurring details
as `interval` and `executionDay` properties. But there are problems with the following scenarios:
1. A user specifies an `executionDay` outside of a valid range (7 for Week, 31 for Month)
2. e.g. `executionDay = 30` is not valid for all months (february has 28 days)

## 2. Options
**1. Clamping `executionDay`**
We could clamp the `executionDay` to the specified month. If we want to access the actual day of execution,
we need to calculate it based on a referenceDate (referenceDate is in the period of `interval`).
Example:
```cs
interval = Interval.Monthly
executionDay = 31 // end of month

DateOnly ActualExecutionDay(referenceDate = 20.2.2026)  // 20. feburary
  => 28.2.2026
```

> All values < 1 are still invalid and will not be clamped

## 3. Decision
We use **Option 1.**
