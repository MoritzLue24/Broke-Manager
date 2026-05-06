**Status:** Closed

## 1. Context
For our `Category.Keywords` and `StandingOrder.Keywords` properties, we need to store them in our PostgreSQL-DB either as a `jsonb` (json-binary), or inside a separate `TABLE` .

## 2. Pros and Cons 
**jsonb**
- ✅ No `JOIN` required for loading
- ✅ Order is kept
- ✅ GIN-Index still supported
- ❌ No keyword uniqueness within a category ensured by the DB

**Separate TABLE** 
- ✅ Uniqueness by the DB
- ✅ Simple queries
- ❌ No as readable inside DB
- ❌ Needs `JOIN`

## 3. Decision
We use `jsonb`.