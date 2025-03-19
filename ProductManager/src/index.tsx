import React, { useState, useEffect } from "react";
import { createRoot } from "react-dom/client";
import "./styles.css";
import ImportProduct from "./ImportProduct";
import { Fragment } from "react/jsx-runtime";

interface Product {
    id: number;
    name: string;
    price: number;
    quantity: number;
}

const ProductList = (): JSX.Element => {
    const [products, setProducts] = useState<Product[]>([]);
    const [search, setSearch] = useState("");
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        fetchProducts();
    }, []);


    const fetchProducts = (query: string = "", quantity: number | null = null) => {
        let url = `/api/products`;

        if (query || quantity !== null) {
            url += `?name=${query}&quantity=${quantity ?? ""}`;
        }

        fetch("/api/auth/check") // Replace with actual authentication endpoint
            .then((res) => res.json())
            .then((data) => {
                if (!data.isAuthenticated) {
                    setError("Access Denied: You are not authorized to view this page.");
                } else {
                    // Fetch products only if authenticated
                    fetch("/api/products")
                        .then((res) => {
                            if (!res.ok) {
                                throw new Error(`HTTP error! Status: ${res.status}`);
                            }
                            return res.json();
                        })
                        .then((data) => setProducts(data))
                        .catch((error) => setError("Error fetching products: " + error.message));
                }
            })
            .catch(() => setError("Error checking authentication status."));
    };


    const filteredProducts = products.filter((product) =>
        product.name.toLowerCase().includes(search.toLowerCase())
    );

    const handleNewProduct = () => {
        window.location.href = "/Products/Create";
    }

    const handleUpdateProduct = (id: number) => {
        window.location.href = `/Products/Edit?id=${id}`;
    };

    const handleDeleteProduct = (id: number) => {
        debugger
        window.location.href = `/Products/Delete?id=${id}`;
    };

    const handleViewDetails = (id: number) => {
        window.location.href = `/Products/Details?id=${id}`;
    };

    const [showImportModal, setShowImportModal] = useState(false); // Track modal visibility

    const handleImportProduct = () => {
        setShowImportModal(true); // Show modal when button is clicked
    };

    const handleSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setSearch(value);
        fetchProducts(value); // Fetch filtered products
    };

    return (
        <div className="container mt-4">
            {error ? (
                <div className="alert alert-danger" role="alert">
                    {error}
                </div>
            ) : (
                <Fragment>
                    <div className="d-flex justify-content-between align-items-center mb-3">
                        <h2>Product List</h2>
                        <div className="d-flex gap-2 ms-auto">
                            <button className="btn btn-primary" onClick={handleNewProduct}>
                                New Product
                            </button>
                            <button className="btn btn-primary" onClick={handleImportProduct}>
                                Import Product
                            </button>
                        </div>
                    </div>

                    <input
                        type="text"
                        className="form-control mb-3"
                        placeholder="Search products..."
                        value={search}
                        onChange={handleSearch}
                    />
                    <table className="table table-bordered">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Name</th>
                                <th>Price</th>
                                <th>Quantity</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {filteredProducts.map((product) => (
                                <tr key={product.id}>
                                    <td>{product.id}</td>
                                    <td>{product.name}</td>
                                    <td>${product.price.toFixed(2)}</td>
                                    <td>{product.quantity}</td>
                                    <td>
                                        <button
                                            className="btn btn-info btn-sm me-2"
                                            onClick={() => handleViewDetails(product.id)}
                                            title="View Details"
                                        >
                                            <i className="bi bi-eye"></i> View
                                        </button>
                                        <button
                                            className="btn btn-warning btn-sm me-2"
                                            onClick={() => handleUpdateProduct(product.id)}
                                            title="Edit Product"
                                        >
                                            <i className="bi bi-pencil"></i> Edit
                                        </button>
                                        <button
                                            className="btn btn-danger btn-sm"
                                            onClick={() => handleDeleteProduct(product.id)}
                                            title="Delete Product"
                                        >
                                            <i className="bi bi-trash"></i> Delete
                                        </button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>

                    {/* Add Modal Here */}
                    {showImportModal && (
                        <div className="modal fade show d-block" tabIndex={-1} role="dialog">
                            <div className="modal-dialog" role="document">
                                <div className="modal-content">
                                    <div className="modal-header">
                                        <h5 className="modal-title">Import Products</h5>
                                        <button type="button" className="btn-close" onClick={() => setShowImportModal(false)}></button>
                                    </div>
                                    <div className="modal-body">
                                        <ImportProduct onUploadSuccess={() => {
                                            setShowImportModal(false);
                                            window.location.reload(); // Refresh products after upload
                                        }} />
                                    </div>
                                </div>
                            </div>
                        </div>
                    )}
                </Fragment>
            )}
        </div>

    );
};

const root = createRoot(document.getElementById("root")!);
root.render(<ProductList />);
