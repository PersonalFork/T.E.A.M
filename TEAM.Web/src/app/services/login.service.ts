import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AppConfiguration } from '../common/AppConfiguration';
import { LoginDto } from '../models/loginDto';


@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) {
    this.http = http;
  }

  login(userId: string, password: string) {
    let options = new HttpHeaders({ 'Content-Type': 'application/json' });
    //options.set('Accept', 'application/json');
    //options.set('Content-Type', 'application/json');
    let loginDto = new LoginDto(userId, password);
    return this.http.post(AppConfiguration.baseUri + "login/doLogin", JSON.stringify(loginDto), { headers: options });
  }

  logOff() {
    return this.http.post(AppConfiguration.baseUri + "login/doLogout", {});
  }
}
