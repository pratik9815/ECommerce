import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
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

  categoryForm: any;
  submitted: boolean = false;
  constructor(private _categoryService: CategoryService,
    private _toastrService: ToastrService,
    private _formBuilder: FormBuilder) { }

  ngOnInit(): void {
    console.log(this.updateCategoryDetails)
    this.categoryForm = this._formBuilder.group({
      id: [null],
      categoryName: ['', [Validators.required]],
      description: ['', [Validators.required]],
      subCategory: this._formBuilder.array([
      ])
    });
    this.editSubCategory(this.updateCategoryDetails)
    // this.categoryForm.patchValue(this.updateCategoryDetails);

    console.log(this.updateCategoryDetails.subCategories)
  }

  get SubCategories(): FormArray {
    return this.categoryForm.get('subCategory') as FormArray;
  }

  get getFormControl() {
    return this.categoryForm.controls;
  }


  setExistingValues(data: any[]): FormArray {
    const formArray = new FormArray([]);
    data.forEach(d => {

      formArray.push(this._formBuilder.group({
        id: d.id,
        subCategoryName: d.subCategoryName,
        subCategoryDescription: d.subCategoryDescription
      }));
    });
    // console.log(formArray)
    return formArray;
  }
  // 
  subCategory(): FormArray {
    return this.categoryForm.get("subCategory") as FormArray;
  }
  newSubCategory(): FormGroup {
    return this._formBuilder.group({
      id: [null],
      subCategoryName: ['', Validators.required],
      subCategoryDescription: ['', [Validators.required]]
    });
  }

  removeSubCategory(i: any) {
    this.subCategory().removeAt(i);
  }
  addSubCategory() {
    this.subCategory().push(this.newSubCategory());
  }

  editSubCategory(categoryDetails:any)
  {
    this.categoryForm.patchValue({
      id:categoryDetails.id,
      categoryName: categoryDetails.categoryName,
      description:categoryDetails.description
    })
    this.categoryForm.setControl('subCategory', this.setExistingValues(categoryDetails.subCategories))
  }


  onUpdateCategory() {
    console.log(this.categoryForm.value)
    this.submitted = true;

    if (this.categoryForm.invalid) return;

    this._categoryService.updateCategoryWithSubCategory(this.categoryForm.value).subscribe({
      next: data => {
        this._toastrService.info("The category has been added", "Success");
        this.callBack.emit();
      },
      error: err => {
        this._toastrService.error("The category added failed", "Error");
      }
    })
  }
}
