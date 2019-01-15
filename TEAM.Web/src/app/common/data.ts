import { UserSession } from "../models/userSession";

export class SessionData {
  static userSession: UserSession;
  static getUserSession(): UserSession {
    if (this.userSession != null) {
      return this.userSession;
    }
    let userSessionInfoData = localStorage.getItem("userSessionInfo");
    if (userSessionInfoData != null) {
      let userSessionInfo: UserSession = JSON.parse(userSessionInfoData);
      return userSessionInfo;
    }
    return null;
  }
}

export class Configuration {
  static baseUri: string = "/api/";
  static secureUrl: string = "/api/secured/";
}
