import React, { useState } from 'react';
import ProductList from './ProductList'; 

function Login({ switchToSignup }) {
    const [formData, setFormData] = useState({ email: '', password: '' });
    const [message, setMessage] = useState('');
    const [isLoggedIn, setIsLoggedIn] = useState(false);

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await fetch('http://localhost:5000/auth/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(formData)
            });
            const data = await response.json();
            if (response.ok) {
                setIsLoggedIn(true); // Set logged-in state
            } else {
                setMessage(data.message || 'Login failed');
                setTimeout(() => setMessage(''), 3000);
            }
        } catch (error) {
            setMessage('Error logging in');
            setTimeout(() => setMessage(''), 3000);
        }
    };

    if (isLoggedIn) {
        return <ProductList />; // Render ProductList after successful login
    }

    return (
        <div>
            <form onSubmit={handleSubmit}>
                <input type="email" name="email" placeholder="Email" onChange={handleChange} />
                <input type="password" name="password" placeholder="Password" onChange={handleChange} />
                <button type="submit">Login</button>
            </form>
            <button onClick={switchToSignup}>Signup</button>
            {message && <p>{message}</p>}
        </div>
    );
}

export default Login;
