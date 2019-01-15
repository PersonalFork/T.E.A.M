import { Component, OnInit } from '@angular/core';
import { UserManagementService } from '../../../services/user-management.service';
import { UserServerDto } from '../../../models/userServer';

@Component({
  selector: 'app-server-list',
  templateUrl: './server-list.component.html',
  styleUrls: ['./server-list.component.css']
})
export class ServerListComponent implements OnInit {

  isServerConfigured: boolean;
  configuredServers: Array<UserServerDto>;

  constructor(private userManagementService: UserManagementService) { }

  ngOnInit() {
    this.isServerConfigured = true;
    this.getUserServers();
  }

  getUserServers() {
    debugger;
    this.userManagementService.getServersByUserId()
      .subscribe(
        response => {
          debugger;
          this.configuredServers = response.json();
        },
        error => {

        }
      )
  }
}
