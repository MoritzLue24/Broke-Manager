import { Link } from "react-router-dom";
import { login } from "../services/auth";
import BadRequestError from "../errors/BadRequestError";
import ResponseMappingError from "../errors/ResponseMappingError";


export default function Login() {
    async function handleSubmit(event: React.SubmitEvent) {
        event.preventDefault();

        const formData = new FormData(event.target);
        const email = formData.get("email");
        const password = formData.get("password");

        try {
            await login(email as string, password as string);
        } catch (err) {
            alert((err as Error).message);
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
