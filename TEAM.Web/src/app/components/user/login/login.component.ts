import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserSession } from '../../../models/userSession';
import { LoginService } from '../../../services/login.service';
import { NavbarService } from '../../../services/navbar.service';
import { SessionData } from '../../../common/data';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  constructor(
    private router: Router,
    private nav: NavbarService,
    private loginService: LoginService) {
    nav.hide();
  }

  userId: string;
  password: string;
  errorMessage: string = "";

  ngOnInit() {
    this.validateLogin();
  }

  doLogin() {
    this.errorMessage = "";
    this.loginService.login(this.userId, this.password)
      .subscribe(
        response => {
          this.nav.show();
          let userSessionResponse = response.json();
          let userSession = new UserSession(
            userSessionResponse.sessionId,
            userSessionResponse.userId,
            userSessionResponse.user.firstName,
            userSessionResponse.user.lastName
          );
          SessionData.userSession = userSession;
          localStorage.setItem("userSessionInfo", JSON.stringify(userSession));
          this.nav.userName = userSession.firstName + " " + userSession.lastName;
          this.router.navigate(['/server-list'], {});
        },
        error => {
          console.log(error);
          this.errorMessage = error.json().Message;
        }
      );
  }

  validateLogin() {
    if (SessionData.getUserSession() != null) {
      this.nav.show();
      this.router.navigate(['/server-list'], {});
    };
  }
}
