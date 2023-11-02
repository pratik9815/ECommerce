import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Console } from 'console';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-update-profile',
  templateUrl: './update-profile.component.html',
  styleUrls: ['./update-profile.component.css']
})
export class UpdateProfileComponent implements OnInit {

  @Input("update-profile") updateprofile: any;

  @Output("User-Update") callback = new EventEmitter<object>();

  submitted: boolean = false;
  editForm: any;

  constructor(private _formBuilder: FormBuilder,
    private _authService: AuthService,
    private _toastrService:ToastrService) { }

  ngOnInit(): void {
    this.editForm = this._formBuilder.group({
      id: [null],
      fullName: ['',[Validators.required]],
      email: ['',[Validators.required]],
      phoneNumber: ['',[Validators.required]],
      address: ['',[Validators.required]],
      userName: [null],
      dob:[null],
      
    });
    this.editForm.patchValue(this.updateprofile)
  }

  get getFormControl()
  {
    return this.editForm.controls;
  }

  saveChanges() {
    this.submitted = true;
    if(this.editForm.invalid) return;

    this._authService.UpdateUser(this.editForm.value).subscribe({
      next: data =>{
        this._toastrService.info("The user has been updated")
        this.callback.emit();
      },
      error: err =>{
        this._toastrService.error("Updating user failed")
        console.log(err)
      }
    });
  }

  onCancel()
  {
    this.callback.emit();
  }


}
