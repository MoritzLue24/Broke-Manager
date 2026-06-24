// src/pages/Dashboard.tsx
import { useEffect, useState } from "react";
import { getCategories, getCategoryBreakdown, getSummary, getTransactions } from "../lib/api";
import type { Transaction } from "../types/api";

type TransactionWithCategory = Transaction & {
  categoryName: string;
};

export default function Dashboard() {
    const [transactions, setTransactions] = useState<TransactionWithCategory[]>([]);
    const [period] = useState({
        range: "alltime"
    });
    const [summary, setSummary] = useState<any>(null);
    const [breakdown, setBreakdown] = useState<any[]>([]);


    useEffect(() => {
        const load = async () => {
            const [transactions, categories, summary, breakdown] = await Promise.all([
                getTransactions(),
                getCategories(),
                getSummary(period),
                getCategoryBreakdown(period)
            ]);
            const categoryMap = new Map<string, string>(
                categories.map((c) => [c.id, c.name])
            );
            const enriched = transactions.map((t) => ({
                ...t,
                categoryName: categoryMap.get(t.categoryId) ?? "Unknown",
            }));
            setTransactions(enriched);
            setSummary(summary);
            setBreakdown(breakdown);
        };

        load();
    }, [period]);

    return (
        <div>
        <h1>Dashboard</h1>

        {summary && (
            <div>
            <h2>Summary</h2>
            <p>Balance: {summary.balance}</p>
            <p>Income: {summary.income}</p>
            <p>Expenses: {summary.expenses}</p>
            </div>
        )}

        <div>
            <h2>Category Breakdown</h2>
            {breakdown.map((b) => (
            <div key={b.category.id}>
                {b.category.name} - {b.expenses} ({b.percentage}%)
            </div>
            ))}
        </div>

        <div>
            <h2>Transactions</h2>
            {transactions.map((t) => (
                <div key={t.id}>
                    {t.title} - {t.categoryName} - {t.amount} - {t.type}
                </div>
            ))}
        </div>
        </div>
    );
}