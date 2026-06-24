# MatchingRule as Entity or Value Object  
**Status:** Closed  

## 1. Context  
We model keyword-based matching rules that are used to automatically assign categories to transactions.  
A `MatchingRule` currently consists of a single `Keyword` and is stored in its own database table (`matching_rules`).  

We need to decide whether `MatchingRule` should be modeled as:  
- a **Value Object embedded inside `Category`**, or  
- a **separate Entity with its own identity (Id)**  

This decision impacts domain modeling, persistence design, and future extensibility.

---

## 2. Pros and Cons  

### **MatchingRule as Entity (current approach)**  
- ✅ Enables direct identification via `Id` for update/delete operations  
- ✅ Supports independent lifecycle management (create, update, delete rules individually)  
- ✅ Easier to extend in the future (e.g. priority, regex rules, weights, disabled flag)  
- ✅ Maps naturally to a separate database table (`matching_rules`)  
- ✅ Works well with EF Core navigation and relationships  
- ❌ Slightly more complex domain model (additional abstraction layer)  
- ❌ Potentially over-engineered if rules remain simple keywords only  
- ❌ Requires handling of identity even though current payload is minimal  

---

### **MatchingRule as Value Object (alternative approach)**  
- ✅ Simpler domain model (just a keyword list inside `Category`)  
- ✅ No need for identity or separate table  
- ✅ Easier reasoning: rule is just data, not an object lifecycle  
- ❌ Cannot easily target a specific rule for update/delete  
- ❌ Harder to extend with additional metadata per rule  
- ❌ Limited flexibility for future features (e.g. rule analytics, ordering, activation state)  

---

## 3. Decision  
We use **MatchingRule as a separate Entity with its own identity**.

The main reasons are:
- Individual rules must be addressable for update and deletion  
- The domain anticipates future extension beyond simple keywords  
- A separate table better supports lifecycle management and persistence requirements  
- Identity prevents ambiguity when rules evolve beyond simple string matching  

Even though the current structure is simple, modeling `MatchingRule` as an Entity provides better scalability and aligns with expected future complexity of the domain.
