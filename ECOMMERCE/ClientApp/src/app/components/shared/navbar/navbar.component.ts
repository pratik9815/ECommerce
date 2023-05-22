import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(private _authService: AuthService,
    private _toastrService: ToastrService) { }

  ngOnInit(): void {
  }
  get fullName() {
    return this._authService.userInfo?.fullName ?? "";
  }
  onLogout() {
    this._authService.onLogout().subscribe({
      next: res => {
        this._toastrService.info("You are logged out", "Logout")
        localStorage.removeItem('token');
      }
    });
  }
}
