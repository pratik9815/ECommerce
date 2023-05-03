import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm: any;
  visible:boolean =true;
  changeType:boolean = true;
  isButtonClicked:boolean = false;
  errorMsg:string ; 
  
  constructor(private _formBuilder:FormBuilder,
    private _authService:AuthService,
    private _router:Router,
    private _toastrService:ToastrService) { }

  ngOnInit(): void {
    this.loginForm = this._formBuilder.group({
      username: ['', Validators.required],
      password: ['', [Validators.minLength(8), Validators.required]],
    });
  }
  onLogin() {
    this.isButtonClicked = true;
    if (this.loginForm.invalid) 
    {
      this.errorMsg = "UserName and Password are required."
      return;
    }
    this._authService.onLogin(this.loginForm.value).subscribe(
      {
        next: res => {  
          this._toastrService.success("Login successful", "success");
          // this._router.navigate(['']);
        },
        error: error => {
          this._toastrService.error("Enter valid credentials", "Unauthorized");
        },
        complete: () =>{console.info('complete')}
      })
  }

  viewpass()
  {
    this.visible = !this.visible;
    this.changeType = !this.changeType;
  }

}
