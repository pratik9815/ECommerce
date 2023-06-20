import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CategoryService } from 'src/app/service/category/category.service';

@Component({
  selector: 'app-update-category',
  templateUrl: './update-category.component.html',
  styleUrls: ['./update-category.component.css']
})
export class UpdateCategoryComponent implements OnInit {

  @Input("update-category-details") updateCategoryDetails: any;

  @Output("callBack-category") callBack = new EventEmitter<object>();

  categoryForm:any;
  submitted: boolean = false;
  constructor(private _categoryService:CategoryService,
              private _toastrService:ToastrService,
              private _formBuilder:FormBuilder) { }

  ngOnInit(): void {
    this.categoryForm = this._formBuilder.group({
      id: [null],
      categoryName: ['',[Validators.required]],
      description: ['',[Validators.required]],

    })
  this.categoryForm.patchValue(this.updateCategoryDetails);
  }
  get getFormControl(){
    return this.categoryForm.controls;
  }
  onUpdateCategory()
  {
    // console.log(this.categoryForm.value)
    this.submitted = true;
    if(this.categoryForm.invalid) return;

      this._categoryService.updateCategory(this.categoryForm.value).subscribe({
        next: data =>{
          this._toastrService.info("The category has been added", "Success");
          this.callBack.emit();
        },
        error: err => {
          this._toastrService.error("The category added failed", "Error");
        }
      })
  }
}
