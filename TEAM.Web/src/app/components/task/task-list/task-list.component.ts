import { TaskService } from './../../../services/task.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {

  tasks;
  title = 'List of Tasks';

  constructor(service: TaskService) {
    this.tasks = service.getTasks();
  }

  ngOnInit() {
  }

}
