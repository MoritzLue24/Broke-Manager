**Status:** Closed

## 1. Context
`Category`'s have keywords / matching rules for automatic assignment to transactions.
We could just store the keywords as string, or we could create a seperate value object or store them as entities.

## 2. Pros and Cons
**1. As string**
- ✅ Simple to implement, no additional complexity 
- ✅ Simpler EF Core configuration 
- ❌ Business logic is scattered, harder to maintain and extend
- ❌ if we want to extend the business logic for matching rules, we have to change the `Category` entity, which is not ideal

**2. Seperate vo**
- ✅ Encapsulates business logic, easier to maintain and extend
- ✅ Makes sense, a matching rule should be identified by its properties.
- ❌ Hard to access because it has no `Id`. If we want to access it we need each property.
- ❌ Additional complexity in managing and maintaining the value object.

**3. As Entity**
- ✅ Easy accessable via its `Id`.
- ❌ Makes not really sense, a matching rule should be a value object.

## 3. Decision
We use **Option 2.**.
