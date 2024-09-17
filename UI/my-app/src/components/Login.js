import React, { Component } from "react";
import { variables } from "../variables";
import { jwtDecode } from "jwt-decode";
export class Login extends Component {
    constructor(props) {
        super(props);
    }

    login() {
        let email = document.getElementById("email").value;
        let password = document.getElementById("password").value;
        fetch(variables.USER_API_URL+"/JWT_Login?Email="+email+"&Password="+password, {
            method:"POST",
            headers: {
                'Content-Type': 'application/json'
            }
        })
        .then(response => {
          if (!response.ok) {
                localStorage.setItem('error', response.status);
                localStorage.setItem('error-desc',  response.statusText)
                window.location.href='/error'


            } else 
            return response.json();
          })
        .then(data => {
            localStorage.setItem('token', data.token);
            localStorage.setItem('userid', jwtDecode(data.token).sub)
            localStorage.setItem('isadmin', jwtDecode(data.token).isAdmin)
            window.location.href='/event'
        })
        .catch(error => {
        })
    }
    render() {
        localStorage.setItem('token', '');
        localStorage.setItem('userid', '');
        localStorage.setItem('isadmin', '');
       return (<div>
        <input type="text" id="email" placeholder="E-mail"/>
        <input type="password" id="password" placeholder="Password"/>
        <input type="submit" value="Submit" onClick={() => this.login()}/>
        
       </div>
       )
    }
}