import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import { BrowserRouter as Router } from 'react-router-dom';
import { createContext, useContext, useState } from 'react';

const apiUrl = process.env.REACT_APP_API_URL;

const GlobalContext = createContext();

export const GlobalProvider = ({ children }) => {
  const [globalState, setGlobalState] = useState({
    isAuthenticated: false,
    cartProps: [],
  });

  const updateGlobalState = (newState) => {
    setGlobalState((prevState) => ({ ...prevState, ...newState }));
  };

  return (
    <GlobalContext.Provider value={{ globalState, updateGlobalState }}>
      {children}
    </GlobalContext.Provider>
  );
};

export const useGlobalContext = () => useContext(GlobalContext);

ReactDOM.render(
  <Router>
    <GlobalProvider>
      <App apiUrl={apiUrl} />
    </GlobalProvider>
  </Router>,
  document.getElementById('root')
);