import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { NavbarService } from '../../../services/navbar.service';
import { LoginService } from '../../../services/login.service';
import { log, debug } from 'util';

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

  doLogin() {
    this.errorMessage = "";
    this.loginService.login(this.userId, this.password)
      .subscribe(
        response => {
          this.nav.show();
          debugger;
          let userSessionInfo = response.json();
          localStorage.setItem("userSessionInfo", JSON.stringify(userSessionInfo));
          this.nav.userName = userSessionInfo.User.FirstName + " " + userSessionInfo.User.LastName;
          this.router.navigate(['/dashboard'], {});
        },
        error => {
          console.log(error);
          this.errorMessage = error.json().Message;
        }
      );
  }
  ngOnInit() { }
}
