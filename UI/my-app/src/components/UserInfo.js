import { useParams } from "react-router-dom";
import React,{Component} from "react";
import { variables } from "../variables";
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
            paramsUserId:'',
            userId:'',
            _Name:"",
            _Surname:"",
            _DateOfBirth:"",
            _RegisterDate:"",
            _Email:"",
            PfpName:"default.png",
            IsAdmin:"",


            events:[]
        }
    }
    componentDidMount() {
        this.state.paramsUserId=this.props.params.id;
        this.state.userId=localStorage.getItem('userid');
        this.parseUser()
    }
    componentDidUpdate() {
        if (this.state.paramsUserId !== this.props.params.id)
            window.location.reload();
    }

    format(target) {
        let date = target.split('T')[0];
        let time = target.split('T')[1];

        let returnDate = date.split('-').reverse().join('.');
        if (time != null) returnDate += " " + time;
        return returnDate;
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
        fetch (variables.USER_API_URL+"/GetUserByGuid?id="+this.props.params.id, {
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
                
            }
            return response.json();
        })
        .then (data => {
            this.setState({
                _Name: data._Name,
                _Surname:data._Surname,
                _DateOfBirth:data._DateOfBirth,
                PfpName: data.PfpName,
            });
            fetch(`${variables.EVENT_API_URL}/GetEventsForThisUser?UserId=${this.state.paramsUserId}`, {
                method: 'GET',
                headers: {
                    Authorization: `Bearer ${localStorage.getItem('token')}`,
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })
            .then(response => {
                return response.json();
            })
            .then(data => {
                this.setState({
                    events: data
                });

            })
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

    handleEventRedirect(id) {
        window.location.href=`/e/${id}`
    }

    render() {
        const {
            _Name,
            _Surname,
            _DateOfBirth,
            PfpName,
            events
        } = this.state;
        if (localStorage.getItem('userid') == '') {
            localStorage.setItem('error', "403");
            localStorage.setItem('error-desc', "Unauthorized")
            window.location.href='/error'
        } else 
        return(
            <div className="container mt-5">
            <div className="row justify-content-center">
                <div className="col-md">
                    <div className="card">
                        <div className="card-body text-center">
                        <img width="400px" height="auto" src={`${variables.USER_PHOTO_URL}\\${PfpName}`} 
                        alt="Фотография" className=" mb-3"
                        onClick={() =>  {
                            if (this.state.paramsUserId == localStorage.getItem('userid')) this.fileInput.click()}} />
                            <h2>{_Name + " " + _Surname}</h2>
                            <p><strong>Дата рождения:</strong> {this.format(_DateOfBirth)}</p>
                            {this.state.userId == localStorage.getItem('userid') ?
                            <input
                            type="file"
                            accept="image/*"
                            style={{ display: 'none' }}
                            ref={input => this.fileInput = input}
                            onChange={this.handleImageChange}
                        /> : null}
                        </div>
                    </div>
                </div>
            </div>
            {events.length != 0 ? 
            <table className="table table-striped table-hover">
                    <thead>
                        <tr>
                            <th>
                                Events this person enrolled to
                            </th>
                            <th>
                                Description
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        {events.map(event =>
                            <tr key={event.EventId} onClick={() => this.handleEventRedirect(event.EventId)}>
                                <td>{`${event._EventName}`}</td>
                                <td>{`${event._Description}`}</td>
                            </tr>
                        )}
                    </tbody>
                    
                </table> : null}
        </div>
            
        )
    }
}

export const UserPageWithParamWithRouter = WithRouter(UserInfo);