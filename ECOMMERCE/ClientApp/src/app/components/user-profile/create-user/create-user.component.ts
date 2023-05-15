import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/service/user/user.service';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.css']
})
export class CreateUserComponent implements OnInit {

  model: NgbDateStruct;
  submitted : boolean = false;
  createUserForm:any;
  constructor(private _formBuilder:FormBuilder,
    private _userService:UserService,
    private _toastrService:ToastrService) { }

  ngOnInit(): void {
    this.createUserForm = this._formBuilder.group({
      fullName: ['',[Validators.required]],
      address: ['',[Validators.required]],
      phoneNumber: ['',[Validators.required]],
      email: ['',[Validators.required]],
      dob: ['',[Validators.required]],
      gender:[0,[Validators.required]],
      username:['',[Validators.required]],
      password:['',[Validators.required]],
      userType : [0,[Validators.required]]
    });

  }

  get getFormControl()
  {
    return this.createUserForm.controls;
  }

  onSubmit()
  {
    console.log(this.createUserForm.value);

    this.submitted = true;
    if(this.createUserForm.invalid) return;

    this._userService.createUser(this.createUserForm.value).subscribe({
      next: res =>{
        console.log(res);
        this._toastrService.success("User has been added successfully","Success")
        this.createUserForm.reset();
      },
      error: err =>{
        console.log(err)
      }
    })
  }

}
