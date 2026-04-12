import type { ZodError } from "zod";


class ResponseMappingError extends Error {
    public zodError: ZodError;
    public data?: unknown;

    constructor(message: string, zodError: ZodError, data?: unknown) {
        super(
            message + ": \n" + 
            JSON.stringify(zodError.issues, null, 2) +
            "\nResponse data: \n" +
            JSON.stringify(data, null, 2)
        );
        Object.setPrototypeOf(this, ResponseMappingError.prototype);

        this.name = "ResponseMappingError";
        this.zodError = zodError;
        this.data = data;
    }
}

export default ResponseMappingError;