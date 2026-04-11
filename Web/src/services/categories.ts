import api from "./api";
import { type Category, mapCategory } from "../types/category";

// TODO: Catch errors


export async function getCategories(): Promise<Category[]> {
    const res = await api.get("/categories");
    return res.data.map(mapCategory);
}

export async function getCategory(id: number): Promise<Category> {
    const res = await api.get(`/categories/${id}`);
    return mapCategory(res.data);
}

export async function deleteCategory(id: number): Promise<void> {
    await api.delete(`/categories/${id}`);
}