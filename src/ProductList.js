import React, { useState, useEffect } from 'react';
import axios from 'axios';

const ProductList = () => {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    let isMounted = true;

    const fetchData = async () => {
      try {
        const result = await axios(`${process.env.REACT_APP_BACKEND_URL}/products`);
        if (isMounted) {
          setProducts(result.data);
        }
      } catch (error) {
        console.error('Failed to fetch products:', error);
      }
    };

    fetchData();

    return () => {
      isMounted = false;
    };
  }, []);

  const renderProduct = (product) => (
    <li key={product.id}>{product.name} - ${product.price}</li>
  );

  return (
    <div>
      <h2>Product List</h2>
      <ul>
        {products.map(renderProduct)}
      </ul>
    </div>
  );
};

export default React.memo(ProductList);