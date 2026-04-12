
class ApiError extends Error {
    constructor(status: number, message: string) {
        super(`Status ${status}: ${message}`);
        Object.setPrototypeOf(this, ApiError.prototype);
        this.name = "ApiError";
    }
}

export default ApiError;