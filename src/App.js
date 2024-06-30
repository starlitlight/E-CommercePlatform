import React, { useState, useContext } from 'react';
import {
  BrowserRouter as Router,
  Route,
  Switch,
  Redirect,
  Link
} from 'react-router-dom';
import HomePage from './HomePage';
import AboutPage from './AboutPage';
import NotFoundPage from './NotFoundPage';

const fakeAuth = {
  isAuthenticated: false,
  login(cb) {
    fakeAuth.isAuthenticated = true;
    setTimeout(cb, 100); 
  },
  logout(cb) {
    fakeAuth.isAuthenticated = false;
    setTimeout(cb, 100);
  },
};

const AuthContext = React.createContext();

function PrivateRoute({ children, ...rest }) {
  let auth = useContext(AuthContext);
  return (
    <Route
      {...rest}
      render={({ location }) =>
        auth.user ? (
          children
        ) : (
          <Redirect
            to={{
              pathname: "/login",
              state: { from: location }
            }}
          />
        )
      }
    />
  );
}

function LoginPage() {
  let auth = useContext(AuthContext);
  let login = () => {
    fakeAuth.login(() => {
      auth.signIn(() => {});
    });
  };

  return (
    <div>
      <p>You must log in to view the page at /about</p>
      <button onClick={login}>Log in</button>
    </div>
  );
}

class App extends React.Component {
  render() {
    return (
      <AuthProvider>
        <Router>
          <div className="main-app">
            <nav>
              <Link to="/">Home</Link> | <Link to="/about">About</Link>
            </nav>
            <Switch>
              <Route exact path="/" component={HomePage} />
              <PrivateRoute path="/about">
                <AboutPage />
              </PrivateRoute>
              <Route path="/login" component={LoginPage} />
              <Route component={NotFoundége} />
            </Switch>
          </div>
        </Router>
      </AuthProvider>
    );
  }
}

export default App;

function AuthProvider({ children }) {
  let [user, setUser] = useState(null);

  let signIn = cb => {
    fakeAuth.login(() => {
      setUser("user");
      cb();
    });
  };

  let signOut = cb => {
    fakeAuth.logout(() => {
      setUser(null);
      cb();
    });
  };

  let value = { user, signIn, signOut };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}