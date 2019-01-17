import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

export class LoaderPayload {
  message: string;
  isLoaderVisible: boolean;
  constructor(message, isLoaderVisible) {
    this.message = message;
    this.isLoaderVisible = isLoaderVisible;
  }
}

@Injectable({
  providedIn: 'root'
})
export class LoaderService {
  loader: LoaderPayload;
  loaderVisibilityChanged: Subject<LoaderPayload> = new Subject<LoaderPayload>();

  constructor() { }

  showLoader(message: string) {
    this.loader = new LoaderPayload(message, true);
    this.loaderVisibilityChanged.next(this.loader);
  }

  hideLoader() {
    this.loader = new LoaderPayload("", false);
    this.loaderVisibilityChanged.next(this.loader);
  }
}
