import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { AppConfiguration } from '../common/AppConfiguration';

@Injectable({
  providedIn: 'root'
})
export class UserManagementService {

  constructor(private http: Http) {
  }

  getServersByUserId() {
    let options = new Headers();
    options.set('Content-Type', 'application/json');
    let uri: string = AppConfiguration.baseUri + "userMgmt/getServersByUserId";
    return this.http.get(uri);
  }
}
