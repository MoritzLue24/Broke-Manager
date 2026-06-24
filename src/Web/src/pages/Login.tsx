// src/pages/Login.tsx
import { useState } from "react";
import { login } from "../lib/api";
import { useNavigate } from "react-router-dom";

export default function Login() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    const handleSubmit = async (e: React.SubmitEvent) => {
        e.preventDefault();

        await login({ email, password });
        navigate("/dashboard");
    };

    return (
        <form onSubmit={handleSubmit}>
        <input
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
        />
        <input
            placeholder="Password"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
        />
        <button type="submit">Login</button>
        </form>
    );
}