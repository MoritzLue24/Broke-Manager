
class NotFoundError extends Error {
    constructor(message: string) {
        super(`404 Not Found: ${message}`);
        Object.setPrototypeOf(this, NotFoundError.prototype);
        this.name = "NotFoundError";
    }
}

export default NotFoundError;