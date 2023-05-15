import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-password-change-success',
  templateUrl: './password-change-success.component.html',
  styleUrls: ['./password-change-success.component.css']
})
export class PasswordChangeSuccessComponent implements OnInit {

  constructor(private _router:Router,private _authService:AuthService) { }

  ngOnInit(): void {
  }


  onLogout()
    {
      this._authService.onLogout();
      // this._router.navigate(['/password-change-success']);
    }
}
