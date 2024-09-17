import { jwtDecode } from "jwt-decode";
import React,{Component} from "react";
import { variables } from "../variables";
import userEvent from "@testing-library/user-event";
export class UserInfo extends Component {
    constructor(props) {
        super(props);
        this.state = {
            userId:'',
            _Name:"",
            _Surname:"",
            _DateOfBirth:"",
            _RegisterDate:"",
            _Email:"",
            PfpName:"default.png",
            IsAdmin:""
        }
    }
    componentDidMount() {
        if (localStorage.getItem('userid') != ''){
            this.state.userId = localStorage.getItem('userid')
        }
        else 
            this.setState({userId: 'default.png'})

        this.parseUser()
        console.log(this.state)
    }
    parseUser() {
        fetch (variables.USER_API_URL+"/GetUserByGuid?id="+this.state.userId, {
            headers: {
                Authorization: `Bearer ${JSON.stringify(localStorage.getItem('token'))}`,
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            
        })
        .then (response => {
            if (!response.ok) {
                localStorage.setItem('error', 401);
                localStorage.setItem('error-desc', "Unauthorized")
                window.location.href='/error'
            }
            return response.json();
        })
        .then (data => {
            this.setState({
                _Name: data._Name,
                _Surname:data._Surname,
                _DateOfBirth:data._DateOfBirth,
                _RegisterDate:data._RegisterDate,
                _Email:data._Email,
                PfpName: data.PfpName,
                IsAdmin:data.IsAdmin
            });
            return;
        })
        .catch(err => {}) 
    }

    render() {
        const {
            _Name,
            _Surname,
            _DateOfBirth,
            _RegisterDate,
            PfpName,
            IsAdmin
        } = this.state;
        return(
        <div>
            <img width="400px" height="100%"
            src={`${variables.USER_PHOTO_URL}\\${PfpName}`} /><br/>
        </div>
        )
    }
}