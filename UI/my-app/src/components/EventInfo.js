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
        eventid:'',
        eventName:'',
        description:'',
        time:'',
        place:'',
        category:'',
        fileName:'',




        registeredUsers:[]
      }
    }
    componentDidMount() {
      this.getTargetEvent();
    }




    registerUser=()=> {
      console.log(this.state);
      fetch(`${variables.USER_API_URL}/AddUserInEvent?EventId=${this.state.eventid}&UserId=${localStorage.getItem('userid')}`,{
        method: 'POST',
        headers: {Authorization: `Bearer ${localStorage.getItem('token')}`}
      })
        .then(response => {
          response.text()})
          
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

          this.setState({
            eventid: data.EventId,
            eventName: data._EventName,
            description: data._Description,
            time: data._Time,
            place: data._Place,
            category: data._Category,
            fileName: data.FileName
          })
          console.log(this.state);
        }
        );
      }
      render() {
        
        // Получение параметра id из this.props.params
        const { id } = this.props.params;
        
        return (
          <div>
          <Card style={{ width: '100%' }}>
        <Card.Body>
          <Row className="align-items-center">
            {/* Левая колонка: Картинка */}
            <Col xs={4} className="text-center">
              <Card.Img 
                variant="top" 
                src={`${variables.EVENT_PHOTO_URL}/${this.state.fileName}`} 
                alt={this.state.eventName} 
                style={{ width: '100%', height: 'auto' }}
              />
              <button className="btn btn-primary mt-5" onClick={(this.registerUser)}>Enroll</button>
            </Col>

            {/* Правая колонка: Заголовок "Event Details" и описание, место, категория */}
            <Col xs={8}>
              <h4>Event Details</h4> {/* Заголовок на одном уровне с картинкой */}
              <p><strong>Event Name:</strong> {this.state.eventName}</p>
              <p><strong>Description:</strong> {this.state.description}</p>
              <p><strong>Place:</strong> {this.state.place}</p>
              <p><strong>Category:</strong> {this.state.category}</p>
              <p><strong>Time:</strong> {this.state.time}</p>
            </Col>
          </Row>
        </Card.Body>
      </Card>




      
      <table className="table table-striped table-hover">
                    <thead>
                        <tr>
                            <th>
                                Event Name
                            </th>
                            { localStorage.getItem('isadmin') == 'True' ? (
                            <th>
                                Options
                            </th> ) : null
                               }
                        </tr>
                    </thead>
                    {/*<tbody>
                        {(e =>
                            <tr key={e._EventName} onClick={() => this.handleEventRedirect(e.EventId)}>
                                <td>{e._EventName}</td>
                                <td>{e._Description}</td>
                                <td>{e._Time}</td>
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
                    */}
                </table>
      </div>
          );
      }
  }

  export const EventPageWithParamWithRouter = WithRouter(EventInfo);