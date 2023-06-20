import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UserService } from 'src/app/service/user/user.service';
import { AuthService } from 'src/app/services/auth/auth.service';
@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  hoverColor = 'white';
  updateprofile: any;
  isEdit: boolean = false;
  isChangePassword: boolean = false;
  res: any;
  constructor(private _authService: AuthService, private _userService: UserService) { }

  ngOnInit(): void {
    this.getUser();
  }

  getUser(): void {
    this._userService.getUser().subscribe({
      next: data => {
        this.res = data;
        console.log(data)
        this.res.forEach((item: any) => {
          if (item.id == this.getId()) {
            this.updateprofile = item;
          }
        });
      },
      error: err => {
        console.log(err)
      }
    });
  }


  callback(): void {
    this.isEdit = false;
    this.isChangePassword =false;
    this.close();
  }

  edit(): void {
  this.isEdit = true;
  this.isChangePassword = false;
  }
  changePassword()
  {
    this.isChangePassword = true;
    this.isEdit = false;
  }

  close() {
    this.getUser();
    this.isEdit = false;
    this.getAddress();
    this.getEmail();
    this.getFullName();
    this.getPhone();
    this.getUserType();
    console.log(this.isEdit)
  }

  getFullName(): string {
    return this.updateprofile?.fullName;
  }

  getPhone(): string {
    return this.updateprofile?.phoneNumber;
  }
  getEmail(): string {
    return this.updateprofile?.email;
  }
  getAddress(): string {
    return this.updateprofile?.address;
  }
  getUserType(): string {
    return this.updateprofile?.userType;
  }
  getId() {
    return this._authService.userInfo?.id ?? '';
  }



}
