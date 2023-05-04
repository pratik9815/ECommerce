import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-update-profile',
  templateUrl: './update-profile.component.html',
  styleUrls: ['./update-profile.component.css']
})
export class UpdateProfileComponent implements OnInit {

  @Input("update-profile") updateprofile : any;

  submitted:boolean = false;
  editForm:FormGroup;
  constructor(private _formBuilder:FormBuilder,
              private _authService:AuthService) { }

  ngOnInit(): void {
    console.log(this.updateprofile)
    this.editForm = this._formBuilder.group({
      fullName: [''],
      email: [''],
      phone: [''],
      address: ['']
    });
    this.editForm.patchValue(this.updateprofile)
  }


  saveChanges()
  {
    this.submitted = true;
    console.log(this.editForm.value)
 
  }

}
