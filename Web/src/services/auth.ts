import api from "./api";

const TOKEN_KEY = "token";

// TODO: Throwen


export async function login(email: string, password: string): Promise<void> {
    console.log(email, password);
    const res = await api.post("/auth/login", { email, password });
    localStorage.setItem(TOKEN_KEY, res.data.token);
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