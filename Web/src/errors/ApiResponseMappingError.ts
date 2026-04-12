import type { ZodError } from "zod";


class ApiResponseMappingError extends Error {
    public zodError: ZodError;
    public data?: unknown;

    constructor(message: string, zodError: ZodError, data?: unknown) {
        super(message);
        Object.setPrototypeOf(this, ApiResponseMappingError.prototype);

        this.name = "ApiResponseMappingError";
        this.zodError = zodError;
        this.data = data;
    }
}

export default ApiResponseMappingError;