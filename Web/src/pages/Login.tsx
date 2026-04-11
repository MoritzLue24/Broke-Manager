import { Link } from "react-router-dom";
import { login } from "../services/auth";


export default function Login() {
    function handleSubmit(event: React.SubmitEvent) {
        event.preventDefault();

        const formData = new FormData(event.target);
        const email = formData.get("email");
        const password = formData.get("password");

        console.log(email, password);

        login(email as string, password as string);
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
