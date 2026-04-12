import api from "./api";
import { CategorySchema, type Category } from "../types/category";
import ApiResponseMappingError from "../errors/ApiResponseMappingError";


export async function getCategories(): Promise<Category[]> {
    const res = await api.get("/categories");
    const validationRes = await CategorySchema.array().safeParseAsync(res.data);

    if (!validationRes.success) {
        throw new ApiResponseMappingError(
            "GET categories failed: Invalid response format",
            validationRes.error,
            res.data
        );
    }
    return validationRes.data;
}

export async function getCategory(id: number): Promise<Category> {
    const res = await api.get(`/categories/${id}`);
    const validationRes = await CategorySchema.safeParseAsync(res.data);

    if (!validationRes.success) {
        throw new ApiResponseMappingError(
            "GET category failed: Invalid response format",
            validationRes.error,
            res.data
        );
    }
    return validationRes.data;
}

export async function deleteCategory(id: number): Promise<void> {
    await api.delete(`/categories/${id}`);
}