import { Injectable } from '@angular/core';
import { UserSession } from '../models/userSession';

@Injectable({
  providedIn: 'root'
})
export class NavbarService {

  visible: boolean;
  userName: string;

  constructor() {
    this.visible = true;
    let userSessionInfoData = localStorage.getItem("userSessionInfo");
    if (userSessionInfoData != null) {
      let userSessionInfo: UserSession = JSON.parse(userSessionInfoData);
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
