import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class LanguageInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const currentLang = 'pt-PT';
    const modifiedReq = req.clone({ 
      headers: req.headers.set('client-lang', currentLang),
    });
    return next.handle(modifiedReq);
  }
}