import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SessionManager } from '../../../common/SessionManager';
import { UserServerDto } from '../../../models/userServer';
import { LoaderService } from '../../../services/loader.service';
import { NavbarService } from '../../../services/navbar.service';
import { NotificationService } from '../../../services/notification.service';
import { UserManagementService } from '../../../services/user-management.service';

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
    private notificationService: NotificationService,
    private router: Router) {
  }

  ngOnInit() {
    this.navService.show();
    this.isServerConfigured = true;
    this.getUserServers();
  }

  showNotification() {
    this.notificationService.showError("Some Error Occurred while displaying messsage");
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
            if (SessionManager.getUserSession() != null) {
              this.notificationService.showError("User Session has expired.Plese login to continue.");
            }
            SessionManager.clearUserSession();
            this.router.navigate(['/login'], {});
          }
          else if (error.status == 504) {
            SessionManager.clearUserSession();
            this.loaderService.hideLoader();
            this.router.navigate(['/login'], {});
            this.notificationService.showError("Some error occurred while connecting to server.");
          }
          else {
            console.log(error);
            this.notificationService.showError("Some error occurred.");
            this.loaderService.hideLoader();
          }
        }
      )
  }
}
