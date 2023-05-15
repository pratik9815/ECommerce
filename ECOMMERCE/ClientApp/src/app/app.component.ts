import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth/auth.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'ClientApp';
  isLoggedIn$ : Observable<boolean>;

  constructor(private _authService:AuthService){
    
    this._authService.showLoginPageIfTokenExpries();

  }

  ngOnInit():void{
    this.isLoggedIn$ = this._authService.isLoggedIn$;  
  }
  
}
