import axios from "axios";
import type { AnalyticsPeriodRequest, Category, CategoryBreakdownResponse, LoginRequest, SummaryResponse, Transaction, User } from "../types/api";


const api = axios.create({
    baseURL: "/api",
    withCredentials: true
});

/*
api.interceptors.request.use(config => {
    console.log(`[API Request]: ${config.method?.toUpperCase()} - ${config.url}`);
    return config;
});

api.interceptors.response.use(
    response => {
        console.log(`[API-Response, ${response.status}]: ${response.config.method?.toUpperCase()} - ${response.config.url}`);
        return response;
    },
    error => {
        if (error.response) {
            console.error(`[API-Response, ${error.response.status}] ERROR: ${error.response.data.message}`);
        }
        return Promise.reject(error);
    }
);
*/

export const login = async (data: LoginRequest) => {
    const response = await api.post<User>("/auth/login", data);
    return response.data;
}

export const getTransactions = async () => {
    const response = await api.get<Transaction[]>("/transactions");
    return response.data;
}

export const getCategories = async () => {
    const response = await api.get<Category[]>(`/categories`)
    return response.data
}

export const getSummary = async (period: AnalyticsPeriodRequest) => {
    const query = new URLSearchParams(period).toString();
    const res = await api.get<SummaryResponse>(
        `/analytics/summary?${query}`
    );
    return res.data;
};

export const getCategoryBreakdown = async (period: AnalyticsPeriodRequest) => {
    const query = new URLSearchParams(period).toString();
    const res = await api.get<CategoryBreakdownResponse[]>(
        `/analytics/category-breakdown?${query}`
    );
    return res.data;
};