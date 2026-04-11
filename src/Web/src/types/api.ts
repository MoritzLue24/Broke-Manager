export type ProblemDetails = {
  type: string
  title: string
  status: number
}


export type LoginRequest = {
    email: string
    password: string
}

export type User = {
    id: string
    email: string
    role: "User" | "Admin",
    createdAt: string
};

export type Transaction = {
    id: string
    userId: string
    categoryId: string
    categorySource: 'Unmatched' | 'Manual' | 'Auto' | 'FromStandingOrder'
    amount: number
    type: 'Income' | 'Expense'
    date: string
    title: string
    description: string
    counterParty: string
    createdAt: string
}

export type Category = {
    id: string,
    userId: string,
    name: string,
    isDefault: boolean,
    matchingRules: MatchingRule[],
    createdAt: string
}

export type MatchingRule = {
    id: string,
    keyword: string
}

export type AnalyticsPeriodRequest = {
    range: string;
    from?: string; // DateOnly → string (yyyy-mm-dd)
    to?: string;
};

export type SummaryResponse = {
    balance: number;
    income: number;
    expenses: number;
};

export type CategoryBreakdownResponse = {
    category: Category;
    expenses: number;
    percentage: number;
};