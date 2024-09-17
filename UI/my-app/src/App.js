import logo from './logo.svg';
import './App.css';
import {Home} from './components/Home'
import {Events} from './components/Event'
import {UserInfo} from './components/UserInfo'
import {Login} from './components/Login'
import {Error} from './components/Error'
import {PageWithParamWithRouter} from './components/EventInfo'
import { BrowserRouter, Route, Routes, NavLink} from 'react-router-dom';

function App() {
  return (
    <BrowserRouter>
    <div className="App container">
      <h3 className ="d-flex justify-content-center m-3">
        React JS Front </h3>
      <nav className="navbar navbar-expand-sm bg-light navbar-dark">
        <ul className="navbar-nav">
          <li className="nav-item m-1">
            <NavLink className="btn btn-light mx-1 btn-outline-primary" to="/login">
            Login
            </NavLink>
            <NavLink className="btn btn-light mx-1 btn-outline-primary" to="/event">
            Event
            </NavLink>
            <NavLink className="btn btn-light mx-1 btn-outline-primary" to="/userinfo">
            UserInfo
            </NavLink>
          </li>
        </ul>
      </nav>

    <Routes>
      <Route path="/home" element={<Home />}/>
      <Route path="/login" element={<Login />}/>
      <Route path="/event" element={<Events />}/>
      <Route path="/userinfo" element={<UserInfo />}/>
      <Route path="/error" element={<Error />}/>
      <Route path= '/e'>
        <Route path=":id"  element={<PageWithParamWithRouter />}/>
      </Route>

    </Routes>
    





    
    </div>
    </BrowserRouter>
  );
}

export default App;
