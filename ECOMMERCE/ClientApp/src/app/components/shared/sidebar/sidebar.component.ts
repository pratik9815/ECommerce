import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HasRoleGuard } from 'src/app/guard/has-role.guard';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  hasRole:boolean = false;

  constructor(private _authService:AuthService,private _router:Router) { 
   this.hasRole =  this._authService.user.usertype.includes('SuperAdmin')
  }

  ngOnInit(): void {
  }

  orderListWithStatusNavigation(orderStatus:any)
  {
    this._router.navigate(['order-with-status/'+orderStatus])
  }

}
