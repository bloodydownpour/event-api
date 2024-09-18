import React, { Component } from "react";
import { variables } from "../variables";
import { jwtDecode } from "jwt-decode";
import { v4 as uuid } from 'uuid';
export class Login extends Component {
    constructor(props) {
        super(props);
        this.state={
            registeringName: "",
            registeringSurname: "",
            registeringDateOfBirth: "",
            registeringEmail: "",
            registeringPassword: ""
        }
    }
    formatDate(date) {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();
    
        if (month.length < 2) 
            month = '0' + month;
        if (day.length < 2) 
            day = '0' + day;
    
        return [year, month, day].join('-');
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

    registerUser() {
        const user = JSON.stringify({
            UserId: uuid(),
            _Name: this.state.registeringName,
            _Surname: this.state.registeringSurname,
            _DateOfBirth: this.state.registeringDateOfBirth,
            _RegisterDate: this.formatDate(new Date()),
            _Email: this.state.registeringEmail,
            _Password: this.state.registeringPassword,
            IsAdmin: false,
            PfpName: "default.png"

        })
        fetch(`${variables.REGISTER_URL}`, {
                method: 'POST',
                
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: user
            })
            .then(response => {
                if (!response.ok) {
                    localStorage.setItem('error', response.status);
                    localStorage.setItem('error-desc', response.statusText+": Validation error");
                    window.location.href='/error'
                }
                response.json();
            })
            .then(data => {
                
            })
            .catch(err => {
                console.log(err._Password)
            })
    }
    ChangeRegisteringName=(e)=> {
        this.setState({registeringName: e.target.value});
    }
    ChangeRegisteringSurname=(e)=> {
        this.setState({registeringSurname: e.target.value});
    }
    ChangeRegisteringDateOfBirth=(e)=> {
        this.setState({registeringDateOfBirth: e.target.value});
    }
    ChangeRegisteringEmail=(e)=> {
        this.setState({registeringEmail: e.target.value});
    }
    ChangeRegisteringPassword=(e)=> {
        this.setState({registeringPassword: e.target.value});
    }
    render() {
        localStorage.setItem('token', '');
        localStorage.setItem('userid', '');
        localStorage.setItem('isadmin', '');
       return (<div>
        <input type="text" id="email" placeholder="E-mail"/>
        <input type="password" id="password" placeholder="Password"/>
        <input type="submit" value="Submit" onClick={() => this.login()}/>

        <button type="button" className="btn btn-primary" data-bs-toggle="modal"
                    data-bs-target="#registerModal">Register</button>
        

        <div className="modal fade" id="registerModal" tabIndex="-1" aria-hidden="true">
                    <div className="modal-dialog modal-1g modal-dialog-centered">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">Register</h5>
                                <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>

                            <div className="modal-body">
                                <div className="d-flex flex-row bd-highlight mb-3">
                                    <div className="p-2 w-50 bd-highlight">

                                        <div className="input-group mb-3">
                                            <input type="text" className="form-control"
                                                placeholder="Enter your name here..." onChange={this.ChangeRegisteringName} />
                                        </div>


                                        <div className="input-group mb-3">
                                            <input type="text" className="form-control"
                                                placeholder="Enter your surname here..." onChange={this.ChangeRegisteringSurname} />
                                        </div>


                                        <div className="input-group mb-3">
                                            <input type="date" className="form-control"
                                            onChange={this.ChangeRegisteringDateOfBirth} />
                                        </div>

                                        <div className="input-group mb-3">
                                            <input type="email" className="form-control"
                                                placeholder="name@example.com" onChange={this.ChangeRegisteringEmail} />
                                        </div>

                                        <div className="input-group mb-3">
                                            <input type="password" className="form-control"
                                                 onChange={this.ChangeRegisteringPassword} />
                                        </div>
                                        <button type="button" className="btn btn-primary float-start"
                                        onClick={() => this.registerUser()}>Update</button>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
       </div>

       )
    }
}