// App.js
import React, { useState } from 'react';
import Signup from './Signup';
import Login from './Login';
import './App.css';

function App() {
    const [isSignup, setIsSignup] = useState(true);

    const switchToLogin = () => setIsSignup(false);
    const switchToSignup = () => setIsSignup(true);

    return (
        <div className="container">
            {isSignup ? (
                <Signup switchToLogin={switchToLogin} />
            ) : (
                <Login switchToSignup={switchToSignup} />
            )}
        </div>
    );
}

export default App;
