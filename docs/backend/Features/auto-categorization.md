# Auto-Categorization
This feature includes automatically assigning standing orders & categories to transactions based on keywords.

## 1. Flow
The user calls an *auto-assign* action (by creating a transaction / importing / updating /  manually calling). Then, every transaction gets checked: first for any standing order machtes, then category matches. But only check for category matches, if the standing order has no assigned category.

As a graph:
```mermaid
flowchart TD
	subgraph ENTRY["`*Entry*`"]
		CREATE["`create`"]
		IMPORT["`import`"]
		MANUAL["`manual<br>0 or more affected`"]
	end
	
	subgraph ORDER["`*Standing order*`"]
		CHECK1["`Check 1<br>*Standing orders*`"]
		ASSIGN1["`Assign<br>*Standing order*`"]
		
		ASSIGN3["`Assign<br>*Standing order category*`"]
	end
	
	subgraph CATEGORY["`*Category*`"]
		CHECK2["`Check 2<br>*Categories*`"]
		ASSIGN2["`Assign<br>*Category*`"]
		
		ASSIGN_DEFAULT["`Assign<br>*default-category*`"]
	end
	
	CREATE --> CHECK1
	IMPORT -->|for each| CHECK1
	MANUAL -->|for each| CHECK1
	
	CHECK1 -->|hit| ASSIGN1
	CHECK1 -->|no hit| CHECK2
	
	ASSIGN1 -->|has category| ASSIGN3
	ASSIGN1 -->|has no category| CHECK2
	
	CHECK2 -->|hit| ASSIGN2
	CHECK2 -->|no hit| ASSIGN_DEFAULT
```


## 2. Rating of hits
For each **check**, a hit list is created, sorted by descending relevance. Relevance is composed of:

- Number of keywords of a category that appear in the transaction
- Proportion of the keywords relative to the transaction title. E.g. for the keyword "Essen" and transaction title "Essen gehen", the proportion is 50%, since "Essen" makes up 5 of the 10 non-whitespace characters of the title.
- Only categories are returned that have at least one keyword appearing in the transaction.
- The default category is ALWAYS returned with the lowest relevance, even if it has no keywords or its keywords do not appear in the transaction.