import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { AppConfiguration } from '../common/AppConfiguration';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(private http: Http) { }

  getMyIncompleteTasks() {
    let options = new Headers();
    options.set('Content-Type', 'application/json');
    return this.http.get(AppConfiguration.baseUri + "tasks/getIncompleteTasks");
  }
}
