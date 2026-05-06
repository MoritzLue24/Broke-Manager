

**On CSV import**
1. User imports CSV with transactions
2. Backend checks for format errors
3. Generates list of transactions
4. Automatically categorize all transactions
	1. If transactions has no hits: Assign default category, and `categorySource = Default`
	2. Otherwise: Assign according category, and `categorySource=Auto`
	3. On conflicting categories: returns a list of transactions and its conflicting categories to the user

**After CSV import, On startup**
1. Go trough every transaction with recurring category
2. Check if transactions date matches with the date-range and `executionDate` of `recurringDetail`
3. If not: Return error to user

**On startup**
1. Go through recurring categories
2. Are there transactions missing?
3. Ask the user if they want to add that transaction (they forgot to add / import it)
4. Otherwise, show this error every time

**On transaction create**
1. Transactions category explicitly given: `categorySource = Manual`, done.
2. Transactions category not given
	1. Complete auto-categorization on that transaction
	2. On multiple hits: return list of conflicting categories `categorySource = Auto`
	3. On no hits: assign default-category, `categorySource = Default`

---

**Restrictions**
- All transactions dates of a recurring categories need to match with `executionDate` and `interval` of `recurringDetail`. 
  For example: You want to send some additional rent to your landlord, and assign it to the category 'Rent', does not work.
- If you have two subscription plans, but transactions both occur on different dates, you cant assign the same category 'Subscriptions' for example. Could get ugly in financial insight.
- 