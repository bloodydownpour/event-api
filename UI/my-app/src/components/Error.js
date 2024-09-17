import React, { Component } from "react";
import { variables } from "../variables";
export class Error extends Component {
    constructor(props) {
        super(props);
    }

render() {
    const code=  localStorage.getItem('error') ;
    const description = localStorage.getItem("error-desc");
    return (
      <div className="container mt-5">
        <div className="alert alert-danger" role="alert">
          <h4 className="alert-heading">Error Code: {code}</h4>
          <p>{description}</p>
        </div>
      </div>
    )
}
}