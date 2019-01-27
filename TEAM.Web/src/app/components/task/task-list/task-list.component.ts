import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ErrorResponseManager } from '../../../common/ErrorResponseManager';
import { LoaderService } from '../../../services/loader.service';
import { NavbarService } from '../../../services/navbar.service';
import { NotificationService } from '../../../services/notification.service';
import { TaskService } from './../../../services/task.service';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {

  tasks: Array<any>;
  selectedTask: any;

  constructor(
    private router: Router,
    private navbarService: NavbarService,
    private taskService: TaskService,
    private loaderService: LoaderService,
    private notificationService: NotificationService) {
  }

  ngOnInit() {
    this.navbarService.show();
    //this.loaderService.hideAll();
    this.getCurrentWeekTasks();
  }

  selectTask(task) {
    this.selectedTask = task;
  }

  // gets the current week tasks.
  getCurrentWeekTasks() {
    this.loaderService.showLoader("Loading Tasks from Server... This will take some time.");
    this.taskService.getCurrentWeekTasks()
      .subscribe(
        (result: Array<any>) => {
          this.tasks = result;
          this.loaderService.hideLoader();
        },
        error => {
          this.loaderService.hideLoader();
          this.notificationService.showError("Failed to get tasks.", ErrorResponseManager.GetErrorMessageString(error));
        }
      );
  }

  // loads the list of incomplete items.
  getMyIncompleteTasks() {
    this.taskService.getMyIncompleteTasks()
      .subscribe(
        (result:any[]) => {
          this.tasks = result;
        },
        error => {
        }
      );
  }
}
