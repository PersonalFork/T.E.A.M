import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { LoginDto } from '../models/loginDto';
import { Configuration } from '../common/data';


@Injectable({
  providedIn: 'root'
})
export class LoginService {

  http: Http;
  constructor(http: Http) {
    this.http = http;
  }

  login(userId: string, password: string) {
    let options = new Headers();
    options.set('Content-Type', 'application/json');
    let loginDto = new LoginDto(userId, password);
    return this.http.post(Configuration.baseUri + "login/doLogin", JSON.stringify(loginDto), { headers: options });
  }

  logOff() {
    return this.http.post(Configuration.baseUri + "login/doLogout", {});
  }
}
