import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CategoryService } from 'src/app/service/category/category.service';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css']
})
export class AddCategoryComponent implements OnInit {

  
  @Output("callBack-category") callBack = new EventEmitter<object>();
  categoryForm:any;
  submitted: boolean = false;

  constructor(private _formBuilder:FormBuilder,
    private _categoryService:CategoryService,
    private _toastrService:ToastrService) { }

  ngOnInit(): void {

    this.categoryForm = this._formBuilder.group({

      categoryName: ['', [Validators.required]],
      description: ['', [Validators.required]],

    });
  }
  get getFormControl() {
    return this.categoryForm.controls;
  }
  onCreate()
  {
    this.submitted = true;
    if (this.categoryForm.invalid) return;

      this._categoryService.createCategory(this.categoryForm.value).subscribe({
        next: data => {
          this._toastrService.info("The category has been added", "Success");
          this.callBack.emit();
        },
        error: err => {
          this._toastrService.error("The category added failed", "Error");
        }
      });
  }

}
