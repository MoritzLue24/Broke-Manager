**Status:** Closed

## 1. Context
We use enumerations for certain properties, like `Role`, `Interval`. To store them inside the postgreSQL database, we could use `TEXT`, or a custom `TYPE`.

## 2. Pros and Cons 
**PostgreSQL native enum type**
- ✅ Database enforces valid values
- ✅ Slightly more storage-efficient 
- ❌ Adding a new value requires an `ALTER TYPE` migration 
- ❌ Renaming or removing values is painful and error-prone 
- ❌ Tightly couples the database schema to the application's enum definition 

**TEXT** 
- ✅ No migration needed when enum values change 
- ✅ Decouples database schema from application code 
- ✅ Simpler EF Core configuration 
- ❌ Validity is enforced only at the application level (domain layer)

## 3. Decision
We use `TEXT`.