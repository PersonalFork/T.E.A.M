import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppConfiguration } from '../common/AppConfiguration';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  private options = new HttpHeaders();
  constructor(private http: HttpClient) {
    this.options.set("Content-Type", 'application/json');
  }

  getMyIncompleteTasks() {
    return this.http.get(AppConfiguration.baseUri + "tasks/getIncompleteTasks", { headers: this.options });
  }

  getCurrentWeekTasks() {
    return this.http.get(AppConfiguration.baseUri + "tasks/getCurrentWeekTasks", { headers: this.options });
  }
}
