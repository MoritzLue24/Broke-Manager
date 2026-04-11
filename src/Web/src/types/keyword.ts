

export type Keyword = {
    id: number;
    value: string;
}

export function mapKeyword(data: any): Keyword {
    return {
        id: data.id,
        value: data.value
    };
}