import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserSession } from '../../../models/userSession';
import { LoginService } from '../../../services/login.service';
import { NavbarService } from '../../../services/navbar.service';
import { SessionData } from '../../../common/data';
import { LoaderService } from '../../../services/loader.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  constructor(
    private router: Router,
    private nav: NavbarService,
    private loader: LoaderService,
    private loginService: LoginService) {
    nav.hide();
  }

  userId: string;
  password: string;
  errorMessage: string = "";

  ngOnInit() {
    this.nav.hide();
    this.validateLogin();
  }

  doLogin() {
    this.loader.showLoader("Please wait while we log you in..");
    this.errorMessage = "";
    this.loginService.login(this.userId, this.password)
      .subscribe(
        response => {
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
          this.loader.hideLoader();
          this.router.navigate(['/server-list'], {});
        },
        error => {
          console.log(error);
          this.loader.hideLoader();
          this.errorMessage = error.statusText;
        }
      );
  }

  validateLogin() {
    if (SessionData.getUserSession() != null) {
      this.router.navigate(['/server-list'], {});
    };
  }
}
