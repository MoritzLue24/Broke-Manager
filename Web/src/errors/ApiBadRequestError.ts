
class ApiBadRequestError extends Error {
    public fieldErrors: Record<string, string[]>;

    constructor(fieldErrors: Record<string, string[]>) {
        super("Bad Request: Validation errors occurred");
        this.name = "ApiBadRequestError";
        this.fieldErrors = fieldErrors;
    }
}

export default ApiBadRequestError;