import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SessionManager } from '../../../common/SessionManager';
import { UserSession } from '../../../models/userSession';
import { LoaderService } from '../../../services/loader.service';
import { LoginService } from '../../../services/login.service';
import { NavbarService } from '../../../services/navbar.service';
import { NotificationService } from '../../../services/notification.service';
import { ErrorResponseManager } from '../../../common/ErrorResponseManager';

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
    private notificationService: NotificationService,
    private loginService: LoginService) {
    nav.hide();
  }

  userId: string;
  password: string;
  errorMessage: string = "";

  ngOnInit() {
    this.loader.showLoader("Loading...");
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
          SessionManager.createLocalSession(userSession);

          this.nav.userName = userSession.firstName + " " + userSession.lastName;
          this.loader.hideLoader();
          this.router.navigate(['/server-list'], {});
        },
        error => {
          this.loader.hideLoader();
          this.notificationService.showError("An error occurred while login.", ErrorResponseManager.GetErrorMessageString(error));
          this.errorMessage = error.statusText;
        }
      );
  }

  validateLogin() {
    this.loader.hideLoader();
    if (SessionManager.getUserSession() != null) {
      this.router.navigate(['/server-list'], {});
    };
  }
}
