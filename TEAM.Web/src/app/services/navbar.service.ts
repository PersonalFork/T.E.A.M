import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NavbarService {

  visible: boolean;
  userName: string;

  constructor() {
    debugger;
    this.visible = true;
    let userSessionInfoData = localStorage.getItem("userSessionInfo");
    if (userSessionInfoData != null) {
      let userSessionInfo = JSON.parse(userSessionInfoData);
      this.userName = userSessionInfo.User.FirstName + " " + userSessionInfo.User.LastName;
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
