import React, { useState } from 'react';
import './Signup.css';

function Signup({ switchToLogin }) {
    const [formData, setFormData] = useState({ username: '', email: '', password: '' });
    const [message, setMessage] = useState('');

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await fetch('http://localhost:5000/auth/signup', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(formData)
            });
            const data = await response.json();
            setMessage(data.message || 'Signup successful');
            setTimeout(() => setMessage(''), 3000);
        } catch (error) {
            setMessage('Error signing up');
            setTimeout(() => setMessage(''), 3000);
        }
    };

    return (
        <div>
            <form onSubmit={handleSubmit}>
                <input type="text" name="username" placeholder="Username" onChange={handleChange} />
                <input type="email" name="email" placeholder="Email" onChange={handleChange} />
                <input type="password" name="password" placeholder="Password" onChange={handleChange} />
                <button type="submit">Sign Up</button>
            </form>
            <button onClick={switchToLogin}>Login</button>
            {message && <p>{message}</p>}
        </div>
    );
}

export default Signup;
