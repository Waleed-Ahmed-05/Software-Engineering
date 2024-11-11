const express = require('express');
const mongoose = require('mongoose');
const User = require('./models/User');
const Product = require('./models/Product');
const AuthRoutes = require('./routes/auth');
const ProductRoutes = require('./routes/products');

// Initialize the app before using it
const app = express(); 
const PORT = 5000;

// Connect to MongoDB
mongoose.connect('mongodb://localhost:27017/ecommerce', {useNewUrlParser: true, useUnifiedTopology: true,}).then(() => console.log('Database connected')).catch(err => console.error('Database connection error.', err));

// Middleware to parse JSON
app.use(express.json());

// Use authentication routes
app.use('/auth', AuthRoutes);
app.use('/products', ProductRoutes);

// Start the server
app.listen(PORT, () => {console.log(`Server running on http://localhost:${PORT}`);});