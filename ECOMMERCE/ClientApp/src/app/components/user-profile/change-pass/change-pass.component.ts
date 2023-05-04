import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-change-pass',
  templateUrl: './change-pass.component.html',
  styleUrls: ['./change-pass.component.css']
})
export class ChangePassComponent implements OnInit {

  passwordForm:FormGroup;
  isChange:boolean;
  constructor(private _formBuilder:FormBuilder) { }

  ngOnInit(): void {
    this.passwordForm = this._formBuilder.group({
      oldPassword: [''],
      newPassword: [''],
      confirmPassword: ['']
    })
  }
  close()
  {
    this.isChange = false;
    this.passwordForm.reset();
  }
  passwordChange()
  {
    console.log(this.passwordForm.value)
  }
}
