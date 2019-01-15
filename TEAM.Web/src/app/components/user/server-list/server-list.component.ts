import { Component, OnInit } from '@angular/core';
import { UserManagementService } from '../../../services/user-management.service';
import { UserServerDto } from '../../../models/userServer';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-list',
  templateUrl: './server-list.component.html',
  styleUrls: ['./server-list.component.css']
})
export class ServerListComponent implements OnInit {

  isServerConfigured: boolean;
  configuredServers: Array<UserServerDto>;

  constructor(
    private userManagementService: UserManagementService,
    private router: Router) {
  }

  ngOnInit() {
    this.isServerConfigured = true;
    this.getUserServers();
  }

  getUserServers() {
    this.userManagementService.getServersByUserId()
      .subscribe(
        response => {
          this.configuredServers = response.json();
        },
        error => {
          var errorMessage = error.statusText;
          console.log(errorMessage);
          this.router.navigate(['/login'], {});
        }
      )
  }
}
