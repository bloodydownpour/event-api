import { jwtDecode } from "jwt-decode";
import { useParams } from "react-router-dom";
import React,{Component} from "react";
import { variables } from "../variables";
import userEvent from "@testing-library/user-event";
import { scryRenderedComponentsWithType } from "react-dom/test-utils";
export function WithRouter(Component) {
    function ComponentWithRouterProp(props) {
      let params = useParams();
      return <Component {...props} params={params} />;
    }
  
    return ComponentWithRouterProp;
  }
export class UserInfo extends Component {
    constructor(props) {
        super(props);
        this.state = {
            user:{},
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
        this.state.userId=this.props.params.id;
        this.parseUser()
    }
    setPfp(id, filename) {
        fetch(variables.USER_API_URL+"/UpdateUserPfp?id="+id+"&fileName="+filename,
            {method: 'POST',
                headers: {
                Authorization: `Bearer ${JSON.stringify(localStorage.getItem('token'))}`,
            }}
        ).then(response => {
            if (!response.ok) {
                localStorage.setItem('error', response.status);
                localStorage.setItem('error-desc', response.statusText)
                window.location.href='/error'
            } })
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
        })
        .catch(err => {}) 
    }
    handleImageChange = (e) => {
        const token = localStorage.getItem('token' || "");
        e.preventDefault();
        const formData = new FormData();
        formData.append("file", e.target.files[0], e.target.files[0].name);
        fetch(variables.USER_API_URL+"/UploadImage", {
            method:"POST",
            headers: {Authorization: `Bearer ${token}`},
            body:formData
        })
        .then(response => {
            console.log(response)
            if (!response.ok) {
                localStorage.setItem('error', response.status);
                localStorage.setItem('error-desc', response.statusText)
                window.location.href='/error'
            } else {
                
            response.text()}
        }
    )
        .then(data => {
            this.setPfp(this.state.userId, e.target.files[0].name);
            this.parseUser();
            window.location.reload();
        })
        .catch(err => {})
    }
    render() {
        const {
            user,
            _Name,
            _Surname,
            _DateOfBirth,
            _RegisterDate,
            PfpName,
        } = this.state;
        return(
            <div className="container mt-5">
            <div className="row justify-content-center">
                <div className="col-md-6">
                    <div className="card">
                        <div className="card-body text-center">
                        <img width="400px" height="100%" src={`${variables.USER_PHOTO_URL}\\${PfpName}`} 
                        alt="Фотография" className=" mb-3"/>
                            <h2>{_Name + " " + _Surname}</h2>
                            <p><strong>Дата рождения:</strong> {_DateOfBirth}</p>
                            <p><strong>Дата регистрации:</strong> {_RegisterDate}</p>
                            {this.state.userId == localStorage.getItem('userid') ?
                            <input className="m-2" type="file" accept="image/*" onChange={this.handleImageChange}/> : null}
                        </div>
                    </div>
                </div>
            </div>
        </div>
            
        )
    }
}

export const UserPageWithParamWithRouter = WithRouter(UserInfo);