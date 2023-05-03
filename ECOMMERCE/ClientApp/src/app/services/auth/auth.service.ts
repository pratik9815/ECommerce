import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { BehaviorSubject, tap } from 'rxjs';
import jwt_decode from "jwt-decode";
import { ToastrService } from 'ngx-toastr';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _isLoggedIn$ = new BehaviorSubject<boolean>(false);
  isLoggedIn$ = this._isLoggedIn$.asObservable();

  apiUrl = "https://localhost:7069/api/ApplicationUser/";
  
  constructor(private _httpClient:HttpClient,
    private _toastrService:ToastrService) {
    const token = localStorage.getItem('token');
    this._isLoggedIn$.next(!!token);
   }

  GetToken() {
    return localStorage.getItem('token');
  }
  onLogin(authenticateRequest: any) {
    return this._httpClient.post(this.apiUrl + "autheticate-user", authenticateRequest).pipe(
      tap((res: any) => {
        this._isLoggedIn$.next(true);
        localStorage.setItem('token', res.token as string);
      })
    )
  }
  onLogout()
  {
    localStorage.removeItem('token');
    this._isLoggedIn$.next(false);
  }
  public decodeToken(): any {
    let rawToken = localStorage.getItem('token');
    if (rawToken != null)
      return jwt_decode(rawToken);
    else
      return null;
  }

  public showLoginPageIfTokenExpries(): void {
    if (this.isTokenExpired()){
       localStorage.removeItem('token');
       this._isLoggedIn$.next(false); // push to subscribers of observable
      this._toastrService.info('You session has expired. Please login again.', 'Info');
       
    }
    else
       this._isLoggedIn$.next(true);  // push to subscribers of observable
  }

  public isTokenExpired(): boolean {
    let rawToken = localStorage.getItem('token');
    if (rawToken == null) {
      this._toastrService.info('You session has expired. Please login again.', 'Info');
      return true;
    }

    const date = this.getTokenExpirationDate();
    if (date === undefined) return false;
    return !(date.valueOf() > new Date().valueOf());
  }

  public getTokenExpirationDate(): Date {
    let rawToken = localStorage.getItem('token');
    const decoded: any = jwt_decode(rawToken as string);

    if (decoded.exp === undefined)
      return null as any;

    const date = new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }


}
