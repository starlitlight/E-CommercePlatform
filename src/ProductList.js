import React, { useState, useEffect } from 'react';
import axios from 'axios';

const ProductList = () => {
  const [products, setProducts] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    let isMounted = true;
    setIsLoading(true); 

    const fetchData = async () => {
      try {
        const result = await axios(`${process.env.REACT_APP_BACKEND_URL}/products`);
        if (isMounted) {
          setProducts(result.data);
          setError(null); 
        }
      } catch (error) {
        console.error('Failed to fetch products:', error);
        if (isMounted) {
          setError('Failed to load products. Please try again later.'); 
        }
      } finally {
        if (isMounted) {
          setIsLoading(false); 
        }
      }
    };

    fetchData();

    return () => {
      isMounted = false;
    };
  }, []);

  if (error) {
    return (
      <div>
        <h2>Product List</h2>
        <p>Error: {error}</p>
      </div>
    );
  }

  if (isLoading) {
    return (
      <div>
        <h2>Product List</h2>
        <p>Loading...</p>
      </div>
    );
  }

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