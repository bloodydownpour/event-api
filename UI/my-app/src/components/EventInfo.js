import { variables } from "../variables";
import React, {Component} from 'react';
import { useParams } from "react-router-dom";
export function WithRouter(Component) {
    function ComponentWithRouterProp(props) {
      let params = useParams();
      return <Component {...props} params={params} />;
    }
  
    return ComponentWithRouterProp;
  }
  

  
  // Классовый компонент для отображения параметра
  export class EventInfo extends Component {
    getTargetEvent() {
      const { id }= this.props.params;
      fetch(`${variables.EVENT_API_URL}/GetEventById?id=${id}`,{
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`,
          'Content-Type': 'application/json'
      },
      })
      .then(response => response.json())
      .then(data => console.log(data));
    }
    render() {
      // Получение параметра id из this.props.params
      const { id } = this.props.params;
      
      return (
      <div>
        <button className="btn btn-primary" onClick={this.getTargetEvent()}>Get Event</button>
        <h1>Параметр из URL: {id}</h1>
        </div>
        );
    }
  }

  export const EventPageWithParamWithRouter = WithRouter(EventInfo);