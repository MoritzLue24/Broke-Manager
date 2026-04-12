import api from "./api";
import ApiResponseMappingError from "../errors/ApiResponseMappingError";
import { AuthResponseSchema } from "../types/authResponse";

const TOKEN_KEY = "token";


export async function register(email: string, password: string, passwordConfirm: string): Promise<void> {
    const res = await api.post("/auth/register", { email, password, passwordConfirm });
    const validationRes = await AuthResponseSchema.safeParseAsync(res.data);

    if (!validationRes.success) {
        throw new ApiResponseMappingError(
            "Registration failed: Invalid response format",
            validationRes.error,
            res.data
        );
    }
    localStorage.setItem(TOKEN_KEY, validationRes.data.token);
}

export async function login(email: string, password: string): Promise<void> {
    const res = await api.post("/auth/login", { email, password });
    const validationRes = await AuthResponseSchema.safeParseAsync(res.data);

    if (!validationRes.success) {
        throw new ApiResponseMappingError(
            "Login failed: Invalid response format",
            validationRes.error,
            res.data
        );
    }
    localStorage.setItem(TOKEN_KEY, validationRes.data.token);
}

export function logout(): void {
    localStorage.removeItem(TOKEN_KEY);
}

export function isAuthenticated(): boolean {
    return !!localStorage.getItem(TOKEN_KEY);
}

export function getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
}