import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
const apiUrl = process.env.REACT_APP_API_URL;
ReactDOM.render(<App apiUrl={apiUrl} />, document.getElementById('root'));