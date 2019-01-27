import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/internal/operators';
import { SessionManager } from '../common/SessionManager';
import { NotificationService } from '../services/notification.service';

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {

  constructor(
    private notificationService: NotificationService,
    private router: Router) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler):
    Observable<HttpEvent<any>> {
    // handle the response.
    return next.handle(req)
      .pipe(
        catchError((err, caught: Observable<HttpEvent<any>>) => {
          if (err instanceof HttpErrorResponse && err.status == 401) {
            if (err.headers.has("IsInvalidSession") && err.headers.get("IsInvalidSession") == 'true') {
              SessionManager.clearUserSession();
              this.notificationService.showError("Session not found or expired. Please login to continue");
              this.router.navigate(['/login'], {});
              return;
            }
            return of(err as any);
          }
          throw err;
        })
      )
  };
}
