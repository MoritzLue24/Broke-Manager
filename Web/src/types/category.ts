import { type Keyword, mapKeyword } from "./keyword";


export type Category = {
    id: number;
    name: string;
    keywords: Keyword[];
    interval: string;
    isDefault: boolean;
}

export function mapCategory(data: any): Category {
    return {
        id: data.id,
        name: data.name,
        keywords: data.keywords.map((k: any) => mapKeyword(k)),
        interval: data.interval,
        isDefault: data.isDefault
    };
}