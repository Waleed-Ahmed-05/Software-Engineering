import React, { useEffect, useState } from 'react';

function ProductList() {
    const [products, setProducts] = useState([]);
    const [newProduct, setNewProduct] = useState({ name: '', description: '', price: '' });
    const [editProduct, setEditProduct] = useState(null);

    useEffect(() => {
        fetchProducts();
    }, []);

    const fetchProducts = async () => {
        try {
            const response = await fetch('http://localhost:5000/products');
            if (!response.ok) throw new Error('Network response was not ok');
            const data = await response.json();
            setProducts(data);
        } catch (error) {
            console.error('There was an error fetching the products!', error);
        }
    };

    const handleAddProduct = async () => {
        try {
            const response = await fetch('http://localhost:5000/products', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newProduct),
            });
            if (response.ok) {
                fetchProducts();
                setNewProduct({ name: '', description: '', price: '' });
            }
        } catch (error) {
            console.error('Error adding product', error);
        }
    };

    const handleEditProduct = async (id) => {
        try {
            const response = await fetch(`http://localhost:5000/products/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(editProduct),
            });
            if (response.ok) {
                fetchProducts();
                setEditProduct(null);
            }
        } catch (error) {
            console.error('Error updating product', error);
        }
    };

    const handleDeleteProduct = async (id) => {
        try {
            const response = await fetch(`http://localhost:5000/products/${id}`, {
                method: 'DELETE',
            });
            if (response.ok) fetchProducts();
        } catch (error) {
            console.error('Error deleting product', error);
        }
    };

    return (
        <div>
            <h2>Product List</h2>

            <h3>Add New Product</h3>
            <input
                type="text"
                placeholder="Name"
                value={newProduct.name}
                onChange={(e) => setNewProduct({ ...newProduct, name: e.target.value })}
            />
            <input
                type="text"
                placeholder="Description"
                value={newProduct.description}
                onChange={(e) => setNewProduct({ ...newProduct, description: e.target.value })}
            />
            <input
                type="number"
                placeholder="Price"
                value={newProduct.price}
                onChange={(e) => setNewProduct({ ...newProduct, price: e.target.value })}
            />
            <button onClick={handleAddProduct}>Add Product</button>

            {products.map((product) => (
                <div key={product._id}>
                    {editProduct && editProduct._id === product._id ? (
                        <div>
                            <input
                                type="text"
                                value={editProduct.name}
                                onChange={(e) =>
                                    setEditProduct({ ...editProduct, name: e.target.value })
                                }
                            />
                            <input
                                type="text"
                                value={editProduct.description}
                                onChange={(e) =>
                                    setEditProduct({ ...editProduct, description: e.target.value })
                                }
                            />
                            <input
                                type="number"
                                value={editProduct.price}
                                onChange={(e) =>
                                    setEditProduct({ ...editProduct, price: e.target.value })
                                }
                            />
                            <button onClick={() => handleEditProduct(product._id)}>Save</button>
                            <button onClick={() => setEditProduct(null)}>Cancel</button>
                        </div>
                    ) : (
                        <div>
                            <h3>{product.name}</h3>
                            <p>{product.description}</p>
                            <p>Price: ${product.price}</p>
                            <button onClick={() => setEditProduct(product)}>Edit</button>
                            <button onClick={() => handleDeleteProduct(product._id)}>Delete</button>
                        </div>
                    )}
                </div>
            ))}
        </div>
    );
}

export default ProductList;
