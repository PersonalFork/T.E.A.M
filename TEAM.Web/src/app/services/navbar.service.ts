import { Injectable } from '@angular/core';
import { SessionData } from '../common/data';
import { UserSession } from '../models/userSession';

@Injectable({
  providedIn: 'root'
})
export class NavbarService {

  visible: boolean;
  userName: string;

  constructor() {

    let userSessionInfo: UserSession = null;
    this.visible = true;

    if (SessionData.userSession == null) {
      let userSessionInfoData = localStorage.getItem("userSessionInfo");
      if (userSessionInfoData != null) {
        userSessionInfo = JSON.parse(userSessionInfoData);
      }
    }
    if (userSessionInfo != null) {
      this.userName = userSessionInfo.firstName + " " + userSessionInfo.lastName;
      this.visible = true;
    }
    else {
      this.userName = "";
      this.visible = false;
    }
  }

  show() {
    this.visible = true;
  }

  hide() {
    this.visible = false;
  }
}
