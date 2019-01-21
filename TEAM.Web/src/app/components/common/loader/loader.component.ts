import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { LoaderService } from '../../../services/loader.service';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.css']
})
export class LoaderComponent implements OnInit {

  message: string;
  loaderCount: number;
  isVisible: boolean;

  constructor(private loaderService: LoaderService) {
    this.loaderCount = 0;
    this.isVisible = false;
  }

  ngOnInit() {
    this.loaderService.loaderVisibilityChanged.subscribe((payload) => {
      if (payload == null) {
        setTimeout(() => {
          this.message = "";
          this.loaderCount = 0;
        }, 0);
        return;
      }

      this.message = payload.message;
      if (payload.isLoaderVisible) {
        setTimeout(() => {
          this.isVisible = true;
          this.loaderCount++;
        }, 0);
      }
      else {
        setTimeout(() => {
          this.loaderCount--;
          if (this.loaderCount == 0) {
            this.isVisible = false;
          }
        }, 0);
      }
    });
  }

}
