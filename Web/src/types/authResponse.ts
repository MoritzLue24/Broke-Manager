import z from "zod";

export const AuthResponseSchema = z.object({
    token: z.string(),
});

export type AuthResponse = z.infer<typeof AuthResponseSchema>;