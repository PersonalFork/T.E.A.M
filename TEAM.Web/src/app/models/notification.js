"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NotificationType;
(function (NotificationType) {
    NotificationType[NotificationType["Info"] = 0] = "Info";
    NotificationType[NotificationType["Warning"] = 1] = "Warning";
    NotificationType[NotificationType["Error"] = 2] = "Error";
    NotificationType[NotificationType["Success"] = 3] = "Success";
})(NotificationType = exports.NotificationType || (exports.NotificationType = {}));
var Notification = /** @class */ (function () {
    function Notification(notificationType, message, details) {
        this.type = NotificationType.Info;
        this.type = notificationType;
        this.message = message;
        this.details = details;
    }
    Notification.prototype.getTypeString = function () {
        return NotificationType[this.type];
    };
    return Notification;
}());
exports.Notification = Notification;
//# sourceMappingURL=notification.js.map