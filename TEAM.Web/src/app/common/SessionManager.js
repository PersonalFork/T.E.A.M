"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionManager = /** @class */ (function () {
    function SessionManager() {
    }
    SessionManager.createLocalSession = function (session) {
        this.userSession = session;
        localStorage.setItem(this.sessionKey, JSON.stringify(session));
    };
    SessionManager.getUserSession = function () {
        if (this.userSession != null) {
            return this.userSession;
        }
        var userSessionInfoData = localStorage.getItem(this.sessionKey);
        if (userSessionInfoData != null) {
            var userSessionInfo = JSON.parse(userSessionInfoData);
            return userSessionInfo;
        }
        return null;
    };
    SessionManager.clearUserSession = function () {
        this.userSession = null;
        localStorage.removeItem(this.sessionKey);
    };
    SessionManager.sessionKey = "userSessionInfo";
    return SessionManager;
}());
exports.SessionManager = SessionManager;
//# sourceMappingURL=SessionManager.js.map