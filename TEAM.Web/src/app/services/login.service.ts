import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { LoginDto } from '../models/loginDto';


@Injectable({
  providedIn: 'root'
})
export class LoginService {

  baseUri: string = "/api/";

  http: Http;
  constructor(http: Http) {
    this.http = http;
  }

  login(userId: string, password: string) {
    let options = new Headers();
    options.set('Content-Type', 'application/json');
    let loginDto = new LoginDto(userId, password);
    return this.http.post(this.baseUri + "login/Test", JSON.stringify(loginDto), { headers: options });
  }
}
