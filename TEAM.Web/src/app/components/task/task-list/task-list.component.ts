import { TaskService } from './../../../services/task.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {

  tasks: Array<any>;

  constructor(private taskService: TaskService) {
  }

  ngOnInit() {
    this.getMyIncompleteTasks();
  }

  // loads the list of incomplete items.
  getMyIncompleteTasks() {
    this.taskService.getMyIncompleteTasks()
      .subscribe(
        result => {
          this.tasks = result.json();
        },
        error => {
        }
      );
  }
}
