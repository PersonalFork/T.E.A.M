import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Configuration } from '../common/data';

@Injectable({
  providedIn: 'root'
})
export class UserManagementService {

  constructor(private http: Http) {
  }

  getServersByUserId() {
    let options = new Headers();
    options.set('Content-Type', 'application/json');
    let uri: string = Configuration.baseUri + "userMgmt/getServersByUserId";
    return this.http.get(uri);
  }
}
