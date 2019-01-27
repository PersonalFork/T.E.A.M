import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AppConfiguration } from '../common/AppConfiguration';

@Injectable({
  providedIn: 'root'
})
export class UserManagementService {

  constructor(private http: HttpClient) {
  }

  getServersByUserId() {
    let options = new HttpHeaders();
    options.set('Content-Type', 'application/json');
    let uri: string = AppConfiguration.baseUri + "userMgmt/getServersByUserId";
    return this.http.get(uri);
  }
}
