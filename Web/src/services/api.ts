import axios from "axios";
import BadRequestError from "../errors/BadRequestError";
import NotFoundError from "../errors/NotFoundError";
import ApiError from "../errors/ApiError";


const api = axios.create({
    baseURL: "/api"
});

// TODO: Add auth token to headers if available

api.interceptors.request.use(config => {
    if (config.data) {
        console.log(`[${config.method?.toUpperCase()}] ${config.url} ${JSON.stringify(config.data)}`);
    }
    else {
        console.log(`[${config.method?.toUpperCase()}] ${config.url}`);
    }
    return config;
});

api.interceptors.response.use(
    response => { return response },
    error => {
        switch (error.response?.status) {
            case 400:
                return Promise.reject(new BadRequestError(
                    error.response?.data?.errors || {}
                ));
            case 404:
                return Promise.reject(new NotFoundError(
                    error.response?.data?.message || "Resource not found"
                ));
        }
        return Promise.reject(new ApiError(
            error.response?.status || 500,
            error.response?.data?.message || "An unknown error occurred"
        ));
    }
);

export default api;