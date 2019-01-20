import { Component, OnInit } from '@angular/core';
import { Notification } from '../../../models/notification';
import { NotificationService } from '../../../services/notification.service';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {

  private pendingNotifications: Array<Notification>;
  private interval;

  notifications: Array<Notification>;

  private maxNotificationCount: number = 4;

  constructor(private notificationService: NotificationService) {
    this.notifications = new Array<Notification>();
    this.pendingNotifications = new Array<Notification>();
  }

  ngOnInit() {
    this.notificationService.onNotification.subscribe(
      notification => {
        this.pendingNotifications.push(notification);
        if (this.interval == null) {
          this.interval = setInterval(() => { this.processNotifications(); }, 100);
        }
      }
    );
  }

  private processNotifications() {
    if (this.notifications.length >= this.maxNotificationCount) {
      return;
    }

    if (this.pendingNotifications.length == 0) {
      return;
    }

    let notification = this.pendingNotifications[0];
    this.pendingNotifications.splice(0, 1);
    this.notifications.push(notification);

    // delete the notification after 5 seconds.
    setTimeout(() => {
      if (this.notifications.length > 0) {
        this.notifications.splice(0, 1);
      }
    }, 5000);
  }
}
