import { type Category, mapCategory } from "./category";


export type Transaction = {
    id: number;
    date: string;
    amount: number;
    title: string;
    counterParty: string;
    category: Category;
}

export function mapTransaction(data: any): Transaction {
    return {
        id: data.id,
        date: data.date,
        amount: data.amount,
        title: data.title,
        counterParty: data.counterParty,
        category: mapCategory(data.category)
    };
}