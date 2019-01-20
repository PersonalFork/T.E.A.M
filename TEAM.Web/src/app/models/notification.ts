export enum NotificationType {
  Info,
  Warning,
  Error,
  Success

}

export class Notification {
  type: NotificationType = NotificationType.Info;
  message: string;
  details: string;

  constructor(notificationType: NotificationType, message: string, details: string) {
    this.type = notificationType;
    this.message = message;
    this.details = details;
  }

  getTypeString() {
    return NotificationType[this.type];
  }
}
