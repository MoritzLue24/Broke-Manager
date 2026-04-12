
class BadRequestError extends Error {
    public fieldErrors: Record<string, string[]>;

    constructor(fieldErrors: Record<string, string[]>) {
        super("Bad Request: \n" + JSON.stringify(fieldErrors, null, 2));
        Object.setPrototypeOf(this, BadRequestError.prototype);

        this.name = "BadRequestError";
        this.fieldErrors = fieldErrors;
    }
}

export default BadRequestError;