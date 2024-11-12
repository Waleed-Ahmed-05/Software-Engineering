const express = require('express');
const mongoose = require('mongoose');
const AuthRoutes = require('./routes/auth');
const ProductRoutes = require('./routes/products');
const cors = require('cors');

// Initialize the app before using it
const app = express(); 
const PORT = 5000;

// Connect to MongoDB
mongoose.connect('mongodb+srv://Admin:<Password>@ecommerce.5pene.mongodb.net/?retryWrites=true&w=majority&appName=ecommerce')
.then(() => console.log('Database connected'))
  .catch(err => console.error('Database connection error:', err));

// Middleware to parse JSON
app.use(express.json());

// Use CORS middleware
app.use(cors());

// Use authentication routes
app.use('/auth', AuthRoutes);
app.use('/products', ProductRoutes);

// Start the server
app.listen(PORT, () => {
    console.log(`Server running on http://localhost:${PORT}`);
});