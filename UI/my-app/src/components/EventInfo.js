import { variables } from "../variables";
import React, {Component} from 'react';
import { useParams } from "react-router-dom";
import { Card, Row, Col } from 'react-bootstrap';
export function WithRouter(Component) {
    function ComponentWithRouterProp(props) {
      let params = useParams();
      return <Component {...props} params={params} />;
    }
  
    return ComponentWithRouterProp;
  }
  

  
  // Классовый компонент для отображения параметра
  export class EventInfo extends Component {
    constructor(props) {
      super(props);

      this.state={
        eventName:'',
        description:'',
        time:'',
        place:'',
        category:'',
        fileName:'',

        registeredUsers:[],
      }
    }
    componentDidMount() {
      this.getTargetEvent();
    }
  
    format(target) {
      let date = target.split('T')[0];
      let time = target.split('T')[1];

      let returnDate = date.split('-').reverse().join('.');
      if (time != null) returnDate += " " + time;
      return returnDate;
  }

    isEnrolled=()=> {
      var isEnrolled = false;
      this.state.registeredUsers.forEach(user => 
        {
        if (user.UserId == localStorage.getItem('userid')) {
          isEnrolled = true;
        }
      });
       return isEnrolled;
    }
    handleUserRedirect=(id)=> {
      window.location.href=`/u/${id}`
    }
    getRegisteredUsers() {
      const {id} = this.props.params;
      fetch(`${variables.USER_API_URL}/GetUsersForThisEvent?EventId=${id}`,{
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`,
          'Accept': 'application/json',
          'Content-Type': 'application/json'}
      })
      .then(response => {
        return response.json()
      })
      .then(data => { 
        this.setState({registeredUsers: data})
      });
    }

    registerUserInEvent=()=> {
      fetch(`${variables.USER_API_URL}/AddUserInEvent?EventId=${this.state.eventid}&UserId=${localStorage.getItem('userid')}`,{
        method: 'POST',
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`
          
        }
      })
      .then(response => {
        alert("Done")
      window.location.reload()});
    }

    retractUserVote=()=> {
      fetch(`${variables.USER_API_URL}/RetractUserFromEvent?EventId=${this.state.eventid}&UserId=${localStorage.getItem('userid')}`, {
        method: 'POST',
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`
        }
      })
      .then(response => {
        alert("Retracted")
      window.location.reload()});
    }


    getTargetEvent=()=> {
      const {id} = this.props.params;
      fetch(`${variables.EVENT_API_URL}/GetEventById?id=${id}`,{
          headers: {
            Authorization: `Bearer ${localStorage.getItem('token')}`,
            'Content-Type': 'application/json'
        },
        })
        .then(response => {  
          return response.json()
        })
        .then(data => {
          this.getRegisteredUsers();
          this.setState({
            eventid: data.EventId,
            eventName: data._EventName,
            description: data._Description,
            time: data._Time,
            place: data._Place,
            category: data._Category,
            fileName: data.FileName
          })
        }
        );
      }
      render() {
        const {
        eventName,
        description,
        time,
        place,
        category,
        fileName,

        registeredUsers
        } = this.state;
        const isEnrolled = this.isEnrolled();
        return (
          
          <div>
          <Card style={{ width: '100%' }}>
        <Card.Body>
          <Row className="align-items-center">
            <Col xs={4} className="text-center">
              <Card.Img 
                variant="top" 
                src={`${variables.EVENT_PHOTO_URL}/${fileName}`} 
                alt={this.state.eventName} 
                style={{ width: '100%', height: 'auto' }}
              />
              {
                
              !isEnrolled ? 
              <button className="btn btn-primary mt-5" onClick={this.registerUserInEvent}>Enroll</button> :
              <button className="btn btn-primary mt-5" onClick={this.retractUserVote}>Retract</button> }

            </Col>

            <Col xs={8}>
              <h4>Event Details</h4>
              <p><strong>Event Name:</strong> {eventName}</p>
              <p><strong>Description:</strong> {description}</p>
              <p><strong>Place:</strong> {place}</p>
              <p><strong>Category:</strong> {category}</p>
              <p><strong>Time:</strong> {this.format(time)}</p>
            </Col>
          </Row>
        </Card.Body>
      </Card>

      <table className="table table-striped table-hover">
                    <thead>
                        <tr>
                            <th>
                                Enrolled Users
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                      {registeredUsers.length != 0 ?
                        <>{registeredUsers.map(user =>
                            <tr key={user.UserId} onClick={() => this.handleUserRedirect(user.UserId)}>
                                <td>{`${user._Name} ${user._Surname}`}</td>
                            </tr>
                        )}</> :  <p>None</p>
                      }
                    </tbody>
                    
                </table>
      </div>
          );
      }
  }

  export const EventPageWithParamWithRouter = React.memo(WithRouter(EventInfo));