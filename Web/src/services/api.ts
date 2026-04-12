import axios from "axios";
import ApiBadRequestError from "../errors/ApiBadRequestError";


const api = axios.create({
    baseURL: "/api"
});

// TODO: Add auth token to headers if available

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
        if (error.response?.status === 400 && error.response?.data?.errors) {
            return Promise.reject(new ApiBadRequestError(error.response.data.errors));
        }
        return Promise.reject(error);
    }
);

export default api;