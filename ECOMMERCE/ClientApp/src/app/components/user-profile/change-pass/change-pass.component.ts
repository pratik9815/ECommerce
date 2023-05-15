import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-change-pass',
  templateUrl: './change-pass.component.html',
  styleUrls: ['./change-pass.component.css']
})
export class ChangePassComponent implements OnInit {


 @Output("change-pass") callback = new EventEmitter<object>();
  passwordForm: any;
  isChange: boolean =false;
  submitted: boolean = false;
  apiResponse: any;


  visibleOld:boolean =true;
  visibleNew:boolean = true;
  changeTypeOld:boolean = true;
  changeTypeNew:boolean = true;

  // isPasswordChanged:boolean = false;
  constructor(private _formBuilder: FormBuilder, private _authService: AuthService, private _toastrService: ToastrService,
              private _router:Router) { }

  ngOnInit(): void {
  
    this.passwordForm = this._formBuilder.group({
      oldPassword: ['', [Validators.required, Validators.pattern("^(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*#?&])[a-zA-Z0-9@$!%*#?&]{8,}$")]],
      newPassword: ['', [Validators.required, Validators.pattern("^(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*#?&])[a-zA-Z0-9@$!%*#?&]{8,}$")]],
      // confirmPassword:['',[Validators.required,Validators.pattern("^(?=.*[a-z])(?=.*[A-Z])(?=.*[@$!%*#?&])[a-zA-Z0-9@$!%*#?&]{8,}$")]],
    });
    

  }

  get getFormControl() {
    return this.passwordForm.controls;
  }
  close() {
    this.passwordForm.reset();
    this.submitted = false;

    if(this.isChange)
    {
      this._authService.onLogout();
    }
  }
  passwordChange() {
    console.log(this.passwordForm.value)
    this.submitted = true;
    if (this.passwordForm.invalid) {
      return;
    }
    // if(this.passwordForm.get('newPassword').value == this.passwordForm.get('confirmPassword').value)
    // {
    //   console.log(this.passwordForm.value)
    //   console.log(this.passwordForm.get('newPassword').value)
    // }

    this._authService.changePassword(this.passwordForm.value).subscribe({
      next: (res:any) => {
        console.log(res)
        this.apiResponse = res.responseCode;

        console.log(this.apiResponse);

        if (this.apiResponse == 200) {
          this.callback.emit();
          this.isChange = true;
          // 
          // 
          this._toastrService.success(res.message, 'Success',{ timeOut: 5000 });
          this._toastrService.info("Please login again to continue.","Logged out")
        }                     
        else{
          this._toastrService.error(res.message,"Error")
        }
      }
    
    });
  }
  viewpassOld()
  {
    this.visibleOld = !this.visibleOld;
    this.changeTypeOld = !this.changeTypeOld;
  }
  viewpassNew()
  {
    this.visibleNew = !this.visibleNew;
    this.changeTypeNew = !this.changeTypeNew;
  }


}
