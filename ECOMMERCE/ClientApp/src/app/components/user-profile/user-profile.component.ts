import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule } from '@angular/forms';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {

  updateprofile:any;
  constructor(private _authService:AuthService) { }

  ngOnInit(): void {
    this.updateprofile = this._authService.userInfo;
  }
 
  getFullName():string
  {
    return this._authService.userInfo?.fullName?? '';
  }
  getAdress()
  {
    return this._authService.userInfo?.address?? '';
  }
  getPhone()
  {
    return this._authService.userInfo?.phone?? '';
  }
  getEmail()
  {
    return this._authService.userInfo?.email?? '';
  }
  getAddress()
  {
    return this._authService.userInfo?.address?? '';
  }
  getUserType()
  {
    return this._authService.userInfo?.usertype??'';
  }
}
