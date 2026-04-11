import { useEffect, useState } from "react";
import type { Category } from "../types/api";
import { getCategories } from "../lib/api";

export default function Categories() {
    const [categories, setCategories] = useState<Category[]>([]);

    useEffect(() => {
        const load = async () => {
            const [categories] = await Promise.all([
                getCategories()
            ]);
            setCategories(categories);
        };

        load();
    }, [])

    return (
        <div className="categories">
            <h1>Categories</h1>
            {categories.map((c) => (
                <div key={c.id}>
                    <h2>{c.name}</h2>
                    {c.matchingRules.map((r) => (
                        <span>{r.keyword}</span>
                    ))}
                    {c.isDefault && (<span>default</span>)}
                </div>
            ))}
        </div>
    );
}