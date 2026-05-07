**Status:** Closed

## 1. Context
`StandingOrder`'s and `Category`'s have keywords for automatic assignment to transactions.
We could just store the keywords as string, but we could create a seperate value object.

## 2. Options
**1. As string**
If we store them as a string, we have to check for business rules inside `StandingOrder` and `Category` scope.
Because we just check `isNullOrWhitespace`, the scope is not the problem.

What is a problem, is that if we want to change `Keyword` specific business rules, we need to change them
in `StandingOrder` and `Category`.

**2. Seperate vo**
We can store all business rules inside a seperate vo. That ensures more flexability for future changes.
We dont need to apply future changes in `StandingOrder` and `Category`, just inside the `Keyword` vo.

## 3. Decision
We use **Option 2.**.
