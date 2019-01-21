import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { AppConfiguration } from '../common/AppConfiguration';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  private options = new Headers();
  constructor(private http: Http) {
    this.options.set("Content-Type", 'application/json');
  }

  getMyIncompleteTasks() {
    return this.http.get(AppConfiguration.baseUri + "tasks/getIncompleteTasks", { headers: this.options });
  }

  getCurrentWeekTasks() {
    let options = new Headers();
    options.set('Content-Type', 'application/json');
    return this.http.get(AppConfiguration.baseUri + "tasks/getCurrentWeekTasks", { headers: options });
  }
}
