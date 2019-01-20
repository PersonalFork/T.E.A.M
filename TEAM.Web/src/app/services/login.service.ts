import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { AppConfiguration } from '../common/AppConfiguration';
import { LoginDto } from '../models/loginDto';


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
    return this.http.post(AppConfiguration.baseUri + "login/doLogin", JSON.stringify(loginDto), { headers: options });
  }

  logOff() {
    return this.http.post(AppConfiguration.baseUri + "login/doLogout", {});
  }
}
