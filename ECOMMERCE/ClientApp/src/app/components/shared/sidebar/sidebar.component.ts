import { Component, OnInit } from '@angular/core';
import { HasRoleGuard } from 'src/app/guard/has-role.guard';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  hasRole:boolean = false;

  constructor(private _authService:AuthService) { 
   this.hasRole =  this._authService.user.usertype.includes('SuperAdmin')
  }

  ngOnInit(): void {
  }

}
