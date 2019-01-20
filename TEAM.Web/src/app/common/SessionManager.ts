import { UserSession } from "../models/userSession";

export class SessionManager {

  private static sessionKey: string = "userSessionInfo";
  static userSession: UserSession;

  static createLocalSession(session: UserSession) {
    this.userSession = session;
    localStorage.setItem(this.sessionKey, JSON.stringify(session));
  }

  static getUserSession(): UserSession {
    if (this.userSession != null) {
      return this.userSession;
    }
    let userSessionInfoData = localStorage.getItem(this.sessionKey);
    if (userSessionInfoData != null) {
      let userSessionInfo: UserSession = JSON.parse(userSessionInfoData);
      return userSessionInfo;
    }
    return null;
  }

  static clearUserSession() {
    this.userSession = null;
    localStorage.removeItem(this.sessionKey);
  }
}
