import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  let authReq = req;
  if (token) {
    authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMsg = 'An unknown error occurred!';

      if (error.error && error.error.message) {
        errorMsg = error.error.message;
      }
      else if (typeof error.error === 'string') {
        errorMsg = error.error;
      }
      else if (error.status === 401) {
        errorMsg = 'Unauthorized. Please log in again.';
        authService.logout();
      }

      console.error('HTTP Error:', errorMsg);
      return throwError(() => new Error(errorMsg));
    })
  );
};
