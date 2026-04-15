import CategoriesPanel from "../components/CategoriesPanel";
import { getCategory } from "../services/categories";


export default function Categories() {

    async function fetchCategory() {
        try {
            await getCategory(5);
        } catch (error) {
            alert((error as Error).message);
        }
    }

    return (
        <div className="categories">
            <h1>Categories</h1>
            <button onClick={fetchCategory}>Fetch Category</button>

            <CategoriesPanel />
        </div>
    );
}