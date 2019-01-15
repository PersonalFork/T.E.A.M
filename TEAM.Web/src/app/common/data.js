"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionData = /** @class */ (function () {
    function SessionData() {
    }
    SessionData.getUserSession = function () {
        if (this.userSession != null) {
            return this.userSession;
        }
        var userSessionInfoData = localStorage.getItem("userSessionInfo");
        if (userSessionInfoData != null) {
            var userSessionInfo = JSON.parse(userSessionInfoData);
            return userSessionInfo;
        }
        return null;
    };
    return SessionData;
}());
exports.SessionData = SessionData;
var Configuration = /** @class */ (function () {
    function Configuration() {
    }
    Configuration.baseUri = "/api/";
    Configuration.secureUrl = "/api/secured/";
    return Configuration;
}());
exports.Configuration = Configuration;
//# sourceMappingURL=data.js.map