import api from "./api";

// TODO: Catch errors
// TODO: Check for maximum length of new / edited keyword

// TODO: Get response?
export async function addKeyword(categoryId: number, keyword: string): Promise<void> {
    await api.post(`/categories/${categoryId}/keywords`, { keyword });
}

export async function deleteKeyword(categoryId: number, keywordId: number): Promise<void> {
    await api.delete(`/categories/${categoryId}/keywords/${keywordId}`);
}
