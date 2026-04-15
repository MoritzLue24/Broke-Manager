import { useEffect, useState } from "react";
import { deleteCategory, getCategories } from "../services/categories";
import { type Keyword } from "../types/keyword";
import "./CategoriesPanel.css";


function CategoriesPanel() {
    const [categories, setCategories] = useState([]);

    useEffect(() => {
        getCategories().then(categories => {
            setCategories(categories);
        });
    }, []);

    function handleDeleteCategory(categoryId: number) {
        deleteCategory(categoryId);
        setCategories(categories.filter(category => category.id !== categoryId));
    }

    return (
        <>
        <button className="category-add">+</button>
        <div className="category-container">
        {
            categories.map(category => (
                <div className="category-card" key={category.id}>
                    <div className="category-header">
                        <h3 className="category-name">{category.name}</h3>
                        <span className="category-interval">{category.interval}</span>
                    </div>
                    <ul className="category-keywords">
                        {
                            category.keywords.map((keyword: Keyword) => (
                                <li key={keyword.id}>
                                    {keyword.value}
                                    <button>✖</button>
                                </li>
                            ))
                        }
                    </ul>
                    <button className="category-delete" onClick={() => handleDeleteCategory(category.id)}>
                        🗑️
                    </button>
                </div>
            ))
        }
        </div>
        </>
    );
}

export default CategoriesPanel;