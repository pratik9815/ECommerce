import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
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
      subCategory: this._formBuilder.array([
        this.newSubCategory() 
      ])
    });
  }

  subCategory():FormArray
  {
    return this.categoryForm.get("subCategory") as FormArray;
  }

  newSubCategory():FormGroup
  {
    return this._formBuilder.group({
      subCategoryName : ['',Validators.required],
      description: ['',[Validators.required]]
    });
  }

  removeSubCategory(i:any)
  {
    this.subCategory().removeAt(i);
  }
  addSubCategory()
  {
    this.subCategory().push(this.newSubCategory());
  }


  get getFormControl() {
    return this.categoryForm.controls;
  }



  
  onCreate()
  {
    console.log(this.categoryForm.value)
    this.submitted = true;
    if (this.categoryForm.invalid) return;

      this._categoryService.createCategoryWithSubCategory(this.categoryForm.value).subscribe({
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
