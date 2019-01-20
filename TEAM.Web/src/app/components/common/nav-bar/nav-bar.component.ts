import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SessionManager } from '../../../common/SessionManager';
import { LoginService } from '../../../services/login.service';
import { NavbarService } from '../../../services/navbar.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  show: boolean;

  constructor(private loginService: LoginService,
    private router: Router,
    private navService: NavbarService, ) { }

  ngOnInit() {
  }

  logOut() {
    this.loginService.logOff().subscribe(
      response => {
        SessionManager.clearUserSession();
        localStorage.removeItem("userSessionInfo");
        this.navService.hide();
        this.router.navigate(['/login'], {});
      },
      error => {
        alert("Could not log out");
      }
    )
  }
}
