import React, { Component } from "react";
import { jwtDecode } from "jwt-decode";
import { variables } from "../variables";
import { v4 as uuid } from 'uuid';
import { RefreshToken } from "./TokenRefresher";
export class Events extends Component {
    constructor(props) {
        super(props);
        this.state = {
            events: [],
            initialEventName: "",
            modalTitle: "",
            _EventName: "",
            _Description: "",
            _Time: "",
            _Place: "",
            _Category: "",
            FileName: "default.png",

            eventNameFilter:"",
            eventTimeFilter:"",
            eventPlaceFilter:"",
            eventCategoryFilter:"",
            eventsWithoutFilter:[]
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

    format(target) {
        let date = target.split('T')[0];
        let time = target.split('T')[1];

        let returnDate = date.split('-').reverse().join('.');
        if (time != null) returnDate += " " + time;
        return returnDate;
    }
    
    filter() {
        var eventNameFilter = this.state.eventNameFilter;
        var eventTimeFilter = this.state.eventTimeFilter;
        var eventPlaceFilter = this.state.eventPlaceFilter;
        var eventCategoryFilter = this.state.eventCategoryFilter;

        var filteredData = this.state.eventsWithoutFilter.filter(
            function(el){
                return el._EventName.toString().toLowerCase().includes(
                    eventNameFilter.toString().trim().toLowerCase()
                )&&
                el._Time.toString().toLowerCase().includes(
                    eventTimeFilter.toString().trim().toLowerCase()
                )&&
                el._Place.toString().toLowerCase().includes(
                    eventPlaceFilter.toString().trim().toLowerCase()
                )&&
                el._Category.toString().toLowerCase().includes(
                    eventCategoryFilter.toString().trim().toLowerCase()
                )
            }
        );
        this.setState({events:filteredData});
        
    }
    updateEvent(name, id) {
        const token = localStorage.getItem('token' || "");
        fetch(variables.EVENT_API_URL + '/GetEventByName?Name=' + name, {
            headers: {Authorization: `Bearer ${token}`}
        })
        .then(response => {
            if (!response.ok) {
                localStorage.setItem('error', response.status);
                localStorage.setItem('error-desc', response.statusText)
                window.location.href='/error'
            } else {
            return response.json()}
        }
    )
            .then(data => {
                const newEvent = JSON.stringify({
                    EventId: data.EventId,
                    _EventName: this.state._EventName,
                    _Description: this.state._Description,
                    _Time: this.state._Time,
                    _Place: this.state._Place,
                    _Category: this.state._Category,
                    FileName: this.state.FileName
                })

                return fetch(`${variables.EVENT_API_URL}/EditEvent`, {
                    method: 'POST',
                    headers: {
                        Authorization: `Bearer ${token}`,
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                        
                    },
                    body: newEvent
                });
            })
                .then(response => {
                    if (!response.ok) {
                        localStorage.setItem('error', response.status);
                        localStorage.setItem('error-desc', response.statusText)
                        window.location.href='/error'
                    } else {
                    response.json()}
                }
                )
            .then(() => {
                alert("Успешный успех")
                this.refreshEventList();
                window.location.reload();
            })
            .catch(err => {})
    }
    deleteEvent(name) {
        const token = localStorage.getItem('token' || "");
        if (window.confirm("Are you sure?")) {
            fetch(variables.EVENT_API_URL + '/GetEventByName?Name=' + name, {
                headers: {Authorization: `Bearer ${token}`}
            })
                .then(response => {
                        if (!response.ok) {
                            localStorage.setItem('error', response.status);
                            localStorage.setItem('error-desc', response.statusText)
                            window.location.href='/error'
                        } else {
                        return response.json()
                    }
                })

                .then(data => {
                    return fetch(`${variables.EVENT_API_URL}/DeleteEvent?eventId=${data.EventId}`, {
                        method: 'DELETE',
                        headers: {
                            Authorization: `Bearer ${token}`,
                            'Accept': 'application/json',
                            'Content-Type': 'application/json'
                        }
                    });
                })
                .then(response => {
                        if (!response.ok) {
                            localStorage.setItem('error', response.status);
                            localStorage.setItem('error-desc', response.statusText)
                            window.location.href='/error'
                        } else {
                        return response.json()}
                    }
                )
                .then(result => {
                    alert("Успешный успех")
                    this.refreshEventList();
                    
                })
                .catch(err => {})
        }
    }
    addEvent() {
        const token = localStorage.getItem('token' || "");
        const newEvent = JSON.stringify({
            EventId: uuid(),
            _EventName: this.state._EventName,
            _Description: this.state._Description,
            _Time: this.state._Time,
            _Place: this.state._Place,
            _Category: this.state._Category,
            FileName: this.state.FileName
        })
        fetch(`${variables.EVENT_API_URL}/AddEvent`, {
            method: 'POST',
            headers: {
                Authorization: `Bearer ${token}`,
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: newEvent
        })
        .then(response => {
            if (!response.ok) {
                localStorage.setItem('error', response.status);
                localStorage.setItem('error-desc', response.statusText)
                window.location.href='/error'
            } else {
            response.json()}}
        )
            .then(result => {
                alert("Успешный успех")
                this.refreshEventList();
            })
            .catch(err => {

            })
    }
    refreshEventList() {
        const checkToken = localStorage.getItem('token') || "";
        if (checkToken !== "" && jwtDecode(checkToken).exp < Math.floor(Date.now()/1000)) {
            RefreshToken()
        }
        const newToken = localStorage.getItem("token")
        fetch(variables.EVENT_API_URL + '/GetEventList', {
            headers: {Authorization: `Bearer ${newToken}`}
        })
            .then(response => {
                if (!response.ok) {
                    localStorage.setItem('error', response.status);
                    localStorage.setItem('error-desc', response.statusText)
                    //window.location.href='/error'
                } else {
                return response.json()}}
           )
            .then(data => {
                this.setState({ events: data , eventsWithoutFilter: data});
            })
            .catch(err => { 
            })
            
        }


    componentDidMount() {
        if (localStorage.getItem('userid') != '') {
            this.refreshEventList();
        } else {
            localStorage.setItem('error', '401');
            localStorage.setItem('error-desc', 'Unauthorized')

            
            //window.location.href='/error'
        }

    }
    ChangeEventName = (e) => {
        this.setState({ _EventName: e.target.value });
    }
    ChangeDescription = (e) => {
        this.setState({ _Description: e.target.value });
    }
    ChangeTime = (e) => {
        this.setState({ _Time: e.target.value });
    }
    ChangePlace = (e) => {
        this.setState({ _Place: e.target.value });
    }
    ChangeCategory = (e) => {
        this.setState({ _Category: e.target.value });
    }
    ChangeEventNameFilter = (e) => {
        this.state.eventNameFilter = e.target.value;
        this.filter();
    }
    ChangeEventTimeFilter = (e) => {
        this.state.eventTimeFilter = e.target.value;
        this.filter();
    }
    ChangeEventPlaceFilter = (e) => {
        this.state.eventPlaceFilter = e.target.value;
        this.filter();
    }
    ChangeEventCategoryFilter = (e) => {
        this.state.eventCategoryFilter = e.target.value;
        this.filter();
    }
    UploadImage = (e) => {
        const token = localStorage.getItem('token' || "");
        e.preventDefault();
        const formData = new FormData();
        formData.append("file", e.target.files[0], e.target.files[0].name);
        fetch(variables.EVENT_API_URL+"/UploadImage", {
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
            this.setState({FileName:e.target.files[0].name})
            this.refreshEventList();
        })
        .catch(err => {})
    }
    addClick() {
        this.setState({
            modalTitle: "Add Event",
            initialEventName: "",
            _EventName: "",
            _Description: "",
            _Time: "",
            _Place: "",
            _Category: "",
            FileName: "default.png"
        })
    }
    enroll(eventid) {
        fetch(variables.USER_API_URL+`/EnrollUserInEvent?EventId=${eventid}&UserId=${localStorage.getItem('userid')}`, {
            headers: {Authorization: `Bearer ${localStorage.getItem('token')}`}
        })
        .then(response => {
            if (!response.ok) {
                localStorage.setItem('error', response.status);
                localStorage.setItem('error-desc', response.statusText)
                window.location.href='/error'
            } else {
            response.json()
            }
        })

        .catch(err => {})

    }

    editClick(ev) {
        this.setState({
            modalTitle: "Edit Event",
            initialEventName: ev._EventName,
            _EventName: ev._EventName,
            _Description: ev._Description,
            _Time: ev._Time,
            _Place: ev._Place,
            _Category: ev._Category,
            FileName: ev.FileName
        })
    }
    handleEventRedirect =(id)=> {
        window.location.href=`/e/${id}`
        }

    render() {
        
        const {
            events,
            modalTitle,
            _EventName,
            _Description,
            _Time,
            _Place,
            _Category,
            FileName
        } = this.state;
        return (
            
            <div>
                <button type="button" className="btn btn-primary" data-bs-toggle="modal"
                    data-bs-target="#eventModal" onClick={() => this.addClick()}>Add Event</button>
                <table className="table table-striped table-hover">
                    <thead>
                        <tr>
                            <th>
                                <input className="form-control m-2"
                                onChange={this.ChangeEventNameFilter}
                                placeholder="Filter"/>
                                Event Name
                            </th>
                            <th>
                                Description
                            </th>
                            <th>
                            <input className="form-control m-2"
                                onChange={this.ChangeEventTimeFilter}
                                placeholder="Filter"/>
                                Time
                            </th>
                            <th>
                            <input className="form-control m-2"
                                onChange={this.ChangeEventPlaceFilter}
                                placeholder="Filter"/>
                                Place
                            </th>
                            <th>
                            <input className="form-control m-2"
                                onChange={this.ChangeEventCategoryFilter}
                                placeholder="Filter"/>
                                Category
                            </th>
                            { /*localStorage.getItem('isadmin') == 'True' ? (
                            <th>
                                Options
                            </th> ) : null
                               */}
                        </tr>
                    </thead>
                    <tbody>
                        {events.map(e =>
                            <tr key={e._EventName} onClick={() => this.handleEventRedirect(e.EventId)}>
                                <td>{e._EventName}</td>
                                <td>{e._Description}</td>
                                <td>{this.format(e._Time)}</td>
                                <td>{e._Place}</td>
                                <td>{e._Category}</td>
                                { localStorage.getItem('isadmin') == 'True' ? (
                                <><td><button type="button"
                                        className="btn btn-light mr-1" data-bs-toggle="modal"
                                        data-bs-target="#eventModal" onClick={(env) => {
                                            env.stopPropagation();
                                            this.editClick(e)}}>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-pencil-square" viewBox="0 0 16 16">
                                            <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
                                            <path fillRule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z" />
                                        </svg>
                                    </button></td><td><button type="button"
                                        className="btn btn-light mr-1" onClick={(env) => {
                                            env.stopPropagation();
                                            this.deleteEvent(e._EventName)
                                            }}>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-trash-fill" viewBox="0 0 16 16">
                                            <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5M8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5m3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0" />
                                        </svg>
                                    </button></td></>) : null                                }
                            </tr>
                        )}
                    </tbody>
                </table>


                <div className="modal fade" id="eventModal" tabIndex="-1" aria-hidden="true">
                    <div className="modal-dialog modal-1g modal-dialog-centered">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">{modalTitle}</h5>
                                <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>

                            <div className="modal-body">
                                <div className="d-flex flex-row bd-highlight mb-3">
                                    <div className="p-2 w-50 bd-highlight">

                                        <div className="input-group mb-3">
                                            <span className="input-group-text">
                                                Event Name
                                            </span>
                                            <input type="text" className="form-control"
                                                value={_EventName} onChange={this.ChangeEventName} />
                                        </div>


                                        <div className="input-group mb-3">
                                            <span className="input-group-text">
                                                Description
                                            </span>
                                            <input type="text" className="form-control"
                                                value={_Description} onChange={this.ChangeDescription} />
                                        </div>


                                        <div className="input-group mb-3">
                                            <span className="input-group-text">
                                                Time
                                            </span>
                                            <input type="date" className="form-control"
                                                value={_Time} onChange={this.ChangeTime} />
                                        </div>

                                        <div className="input-group mb-3">
                                            <span className="input-group-text">
                                                Place
                                            </span>
                                            <input type="text" className="form-control"
                                                value={_Place} onChange={this.ChangePlace} />
                                        </div>

                                        <div className="input-group mb-3">
                                            <span className="input-group-text">
                                                Category
                                            </span>
                                            <input type="text" className="form-control"
                                                value={_Category} onChange={this.ChangeCategory} />
                                        </div>

                                        <div className="p-2 w-50 bd-highlight">
                                            <img width="400px" height="100%"
                                                src={`${variables.EVENT_PHOTO_URL}\\${FileName}`} />
                                            <input className="m-2" type="file" accept="image/*" onChange={this.UploadImage}/>
                                        </div>
                                        {this.state.initialEventName == ''
                                            ? <button type="button" className="btn btn-primary float-start" onClick={() => this.addEvent()}>Create</button>
                                            : null
                                        }
                                        {this.state.initialEventName != ''
                                            ? <button type="button" className="btn btn-primary float-start" onClick={() => this.updateEvent(this.state.initialEventName, this.state.eventId)}>Update</button>
                                            : null
                                        }

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>)
    }
}