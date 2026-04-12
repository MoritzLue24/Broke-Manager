import { Link } from "react-router-dom";
import { login } from "../services/auth";
import ApiBadRequestError from "../errors/ApiBadRequestError";
import ApiResponseMappingError from "../errors/ApiResponseMappingError";


export default function Login() {
    async function handleSubmit(event: React.SubmitEvent) {
        event.preventDefault();

        const formData = new FormData(event.target);
        const email = formData.get("email");
        const password = formData.get("password");

        try {
            await login(email as string, password as string);
        } catch (err) {
            if (err instanceof ApiBadRequestError) {
                alert(
                    err.message + "\n" +
                    Object.entries(err.fieldErrors).map(
                        ([field, messages]) => `${field}: ${messages.join(", ")}`
                    ).join("\n")
                );
            }
            else if (err instanceof ApiResponseMappingError) {
                alert(
                    err.message + "\n" + 
                    "Data: " + JSON.stringify(err.data)
                );
            }
        }
    }

    return (
        <div className="Login">
            <h1>Login</h1>
            <form onSubmit={handleSubmit}>
                <input type="email" name="email" placeholder="Email" />
                <input type="password" name="password" placeholder="Password" />
                <button type="submit">Login</button>
            </form>
            <Link to="/register">Register</Link>
        </div>
    );
}
