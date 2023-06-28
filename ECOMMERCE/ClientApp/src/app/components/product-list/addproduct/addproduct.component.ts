import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { ToastrService } from 'ngx-toastr';
import { CategoryService } from 'src/app/service/category/category.service';
import { ProductService } from 'src/app/services/product/product.service';
import { ProductStatus } from 'src/app/services/web-api-client';

@Component({
  selector: 'app-addproduct',
  templateUrl: './addproduct.component.html',
  styleUrls: ['./addproduct.component.css']
})
export class AddproductComponent implements OnInit {

  //this is for the preview of the image
  @ViewChild('fileInput', { static: false }) fileInput: ElementRef;

  @Output("callBack-product") callBack = new EventEmitter<object>();
  productForm: any;
  productData: any;
  submitted: boolean = false;
  // isCreated: boolean;
  categoryList:any;
  subCategoryList:any[] = [];

  imageSource: any;


  productStatus: any = [{
    id: ProductStatus.InStock,
    name: "InStock"
  },
  {
    id: ProductStatus.OutOfStock,
    name: "OutOfStock"
  },
  {
    id: ProductStatus.Damaged,
    name: "Damaged"
  },
]


  constructor(private _formBuilder: FormBuilder,
    private _productService: ProductService,
    private _toastrService: ToastrService,
    private _categoryService:CategoryService) { }

  ngOnInit(): void {
    this.getCategory();

    this.productForm = this._formBuilder.group({
      name: ['', [Validators.required]],
      price: ['', [Validators.required]],
      quantity: ['', [Validators.required]],
      description: ['', [Validators.required]],
      categoryId:[null,[Validators.required]],
      productStatus:[0,[Validators.required]],
      subCategoryId:[null,[Validators.required]]
    });
    console.log(this.productForm.value);
  }
  get getFormControl() {
    return this.productForm.controls;
  }
  onCreate() {
    this.submitted = true;
        if (this.productForm.invalid) return;
    

    let input = new FormData();
    // input.append('img', this.fileInput.nativeElement.files[0]);
    for (let i = 0; i < this.imageSource.length; i++) {
      input.append('img', this.fileInput.nativeElement.files[i]);
    }
    input.append('name', this.productForm.value.name);
    input.append('price', this.productForm.value.price);
    input.append('description', this.productForm.value.description);
    input.append('quantity', this.productForm.value.quantity);
    
  input.append('categoryId',this.productForm.value.categoryId);
  // const categoryIds = this.productForm.get('categoryId').value;   
  // for (let i = 0; i < categoryIds.length; i++) {
  //   input.append('categoryId', categoryIds[i]);
  // }
 
    const subCategoryIds = this.productForm.get('subCategoryId').value; 
    for(let i = 0; i < subCategoryIds.length; i++)
    {
      input.append('subCategoryId',subCategoryIds[i]);
    }
  


  // input.forEach((value, key) => {
  //   console.log(key + ': ' + value);
  // });

    this._productService.CreateProduct(input).subscribe({
      next: data => {
        this._toastrService.info("The product has been added", "Success");
        this.callBack.emit();
      },
      error: err => {
        this._toastrService.error("The product added failed", "Error");
      }
    });
  }

  getCategory()
  {
    this._categoryService.getCategoryWithSubCategory().subscribe({
      next: res =>{
        console.log(res);
        this.categoryList = res;
      },
      error: err => {
        console.log(err);
      }
    })
  }
  // imageUpload(event: any) {
  //   var file = event.target.file[0];
  //   const formData: FormData = new FormData();
  //   formData.append('file', file, file.Name);
  //   this._httpClient.post(this.apiUrl, formData);
  // }


//For single image we do this
  // onFileChange(event:any) {
  //   const file = event.target.files[0];
  //   const reader = new FileReader();
  //   reader.readAsDataURL(file);
  //   reader.onload = () => {
  //     this.imageSrc = reader.result as string;
  //   };
  // }

  //This one is for multiple image
  onFileChange(event: any) {
    // let file = event.target.files[0];
    const files = event.target.files;
    this.imageSource = [];
    for (let i = 0; i < files.length; i++) {
      const file = files[i];
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.imageSource.push(reader.result as string);
      };
    }
    console.log(files)
    console.log(this.imageSource) 
  }
  onChange(e:any)
  {
    this.subCategoryList = e.subCategories;
    this.productForm.value.categoryId = e.id;
  }

  onChangeSubCategory(e:any)
  {
    if(this.subCategoryList.length > 0)
    console.log(e)
    this.productForm.value.subcategoryId = e.id;
  }



}