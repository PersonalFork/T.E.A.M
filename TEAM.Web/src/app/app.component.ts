import { Component, OnInit } from '@angular/core';
import { NavbarService } from './services/navbar.service';
import { fail } from 'assert';
import { SessionManager } from './common/SessionManager';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'Task Effort Activity Manager';
  isToolbarVisible: boolean = true;

  constructor(private nav: NavbarService) {

  }

  ngOnInit(): void {
    this.isToolbarVisible = SessionManager.userSession != null;
    this.nav.navbarVisibilityChanged.subscribe((isVisible) => {
      // timeout has been placed to remove the expressionchangedafterithasbeencheckederror.
      // source : https://stackoverflow.com/questions/50962765/expressionchangedafterithasbeencheckederror-invoking-toastr-in-nginit-in-angular
      setTimeout(() => {
        if (isVisible) {
          this.isToolbarVisible = true;
        }
        else {
          this.isToolbarVisible = false;
        }
      }, 0);
    });
  }
}
