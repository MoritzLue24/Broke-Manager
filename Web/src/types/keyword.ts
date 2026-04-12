import z from "zod";


export const KeywordSchema = z.object({
    id: z.number(),
    value: z.string(),
    categoryId: z.number(),
});
export type Keyword = z.infer<typeof KeywordSchema>;