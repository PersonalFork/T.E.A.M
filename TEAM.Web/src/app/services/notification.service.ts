import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Notification, NotificationType } from '../models/notification';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor() { }

  onNotification: Subject<Notification> = new Subject<Notification>();

  showError(errorMessage: string, details: string = "") {
    let error = new Notification(NotificationType.Error, errorMessage, details);
    this.onNotification.next(error);
  }

  showInfo(info: string, details: string = "") {
    let infoNotification = new Notification(NotificationType.Info, info, details);
    this.onNotification.next(infoNotification);
  }

  showWarning(info: string, details: string = "") {
    let warn = new Notification(NotificationType.Warning, info, details);
    this.onNotification.next(warn);
  }

  showSuccess(successMessage: string, details: string = "") {
    let success = new Notification(NotificationType.Success, successMessage, details);
    this.onNotification.next(success);
  }
}
