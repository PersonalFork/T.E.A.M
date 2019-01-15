import { Component, ChangeDetectorRef  } from '@angular/core';
import { NavbarService } from './services/navbar.service';
import { log } from 'util';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Task Effort Activity Manager';

  constructor(private nav: NavbarService) {
  }
}
