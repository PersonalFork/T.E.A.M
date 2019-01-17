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

  @ViewChild('showLoader') showLoaderButton: ElementRef;
  @ViewChild('hideLoader') hideLoaderButton: ElementRef;

  constructor(private loaderService: LoaderService) {
    this.loaderCount = 0;
    this.isVisible = false;
  }

  ngOnInit() {
    this.loaderService.loaderVisibilityChanged.subscribe((payload) => {
      this.message = payload.message;
      if (payload.isLoaderVisible) {
        if (this.loaderCount == 0) {
          this.showLoaderButton.nativeElement.click();
        }
        this.isVisible = true;
        this.loaderCount++;
      }
      else {
        this.loaderCount--;
        if (this.loaderCount == 0) {
          this.hideLoaderButton.nativeElement.click();
          this.isVisible = false;
        }
      }
    });
  }

}
