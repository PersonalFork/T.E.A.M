import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { SessionManager } from '../common/SessionManager';
import { UserSession } from '../models/userSession';


@Injectable({
  providedIn: 'root'
})
export class NavbarService {

  visible: boolean;
  userName: string;
  navbarVisibilityChanged: Subject<boolean> = new Subject<boolean>();

  constructor() {
    let userSessionInfo: UserSession = null;
    this.visible = true;

    if (SessionManager.userSession == null) {
      let userSessionInfoData = localStorage.getItem("userSessionInfo");
      if (userSessionInfoData != null) {
        userSessionInfo = JSON.parse(userSessionInfoData);
      }
    }
    if (userSessionInfo != null) {
      this.userName = userSessionInfo.firstName + " " + userSessionInfo.lastName;
      this.navbarVisibilityChanged.next(true);
      this.visible = true;
    }
    else {
      this.userName = "";
      this.navbarVisibilityChanged.next(false);
      this.visible = false;
    }
  }

  show() {
    this.navbarVisibilityChanged.next(true);
    this.visible = true;
  }

  hide() {
    this.navbarVisibilityChanged.next(false);
    this.visible = false;
  }
}
