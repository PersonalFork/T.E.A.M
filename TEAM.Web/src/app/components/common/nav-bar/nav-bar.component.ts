import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../../../services/navbar.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  show: boolean;

  constructor(private navService: NavbarService) { }

  ngOnInit() {
  }
}
