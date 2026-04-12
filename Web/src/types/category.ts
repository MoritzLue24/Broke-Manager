import z from "zod";
import { KeywordSchema } from "./keyword";


export const IntervalSchema = z.enum(["Once", "Weekly", "Monthly", "Quarterly", "Yearly"]);
export type Interval = z.infer<typeof IntervalSchema>;

export const CategorySchema = z.object({
    id: z.number(),
    name: z.string(),
    keywords: z.array(KeywordSchema),
    interval: IntervalSchema,
    isDefault: z.boolean(),
});
export type Category = z.infer<typeof CategorySchema>;