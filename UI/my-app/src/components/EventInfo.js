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
    render() {
      // Получение параметра id из this.props.params
      const { id } = this.props.params;
      return <h1>Параметр из URL: {id}</h1>;
    }
  }

  export const PageWithParamWithRouter = WithRouter(EventInfo);