const express = require('express');
const Product = require('../models/Product');
const router = express.Router();

// Create a new product
router.post('/', async (req, res) => {const newProduct = new Product(req.body); await newProduct.save(); res.status(201).json(newProduct);});

// Get all products
router.get('/', async (req, res) => {const products = await Product.find(); res.json(products);});

// Update a product
router.put('/:id', async (req, res) => {const updatedProduct = await Product.findByIdAndUpdate(req.params.id, req.body, { new: true }); res.json(updatedProduct);});

// Delete a product
router.delete('/:id', async (req, res) => {await Product.findByIdAndDelete(req.params.id); res.json({ message: 'Product deleted' });});

module.exports = router;