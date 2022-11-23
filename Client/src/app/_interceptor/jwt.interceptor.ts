import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { take } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService:AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser:User;

    //- o 'take(1)' dentro do '.pipe' é para pegar 1 elemento que retornar do 'observable', após este retorno automáticamente é feito o 'unsubscribe'.
    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe({
        next: user => currentUser = user
      });

    if(currentUser){
      request= request.clone({
        setHeaders:{
          Authorization: `Bearer ${currentUser.token}`
        }
      })
    }
    
    return next.handle(request);
  }
}
