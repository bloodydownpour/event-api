import { variables } from "../variables";

export function RefreshToken() {
    fetch(variables.USER_API_URL+`/RefreshToken?token=${localStorage.getItem('token')}`)
    .then(response => 
        response.text()
    )
    .then(data => {
        console.log(data);
        localStorage.setItem('token', data)
        window.location.reload();
    })
    .catch(err => {})
}