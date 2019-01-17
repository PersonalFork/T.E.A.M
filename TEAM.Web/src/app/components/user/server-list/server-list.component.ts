import { Component, OnInit } from '@angular/core';
import { UserManagementService } from '../../../services/user-management.service';
import { UserServerDto } from '../../../models/userServer';
import { Router } from '@angular/router';
import { Response } from 'selenium-webdriver/http';
import { NavbarService } from '../../../services/navbar.service';
import { LoaderService } from '../../../services/loader.service';

@Component({
  selector: 'app-server-list',
  templateUrl: './server-list.component.html',
  styleUrls: ['./server-list.component.css']
})
export class ServerListComponent implements OnInit {

  isServerConfigured: boolean;
  configuredServers: Array<UserServerDto>;

  constructor(
    private navService: NavbarService,
    private loaderService: LoaderService,
    private userManagementService: UserManagementService,
    private router: Router) {
  }

  ngOnInit() {
    this.navService.show();
    this.isServerConfigured = true;
    this.getUserServers();
  }

  getUserServers() {
    this.loaderService.showLoader("Loading Server List..");
    this.userManagementService.getServersByUserId()
      .subscribe(
        response => {
          this.configuredServers = response.json();
          this.loaderService.hideLoader();
        },
        error => {
          // unauthorized.
          if (error.status == 401) {
            this.loaderService.hideLoader();
            this.router.navigate(['/login'], {});
          }
          else {
            this.loaderService.hideLoader();
          }
        }
      )
  }
}
