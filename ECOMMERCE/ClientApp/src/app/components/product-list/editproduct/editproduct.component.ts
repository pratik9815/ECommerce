import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CategoryService } from 'src/app/service/category/category.service';
import { ProductService } from 'src/app/services/product/product.service';
import { ProductStatus } from 'src/app/services/web-api-client';

@Component({
  selector: 'app-editproduct',
  templateUrl: './editproduct.component.html',
  styleUrls: ['./editproduct.component.css']
})
export class EditproductComponent implements OnInit {
  // Getting values from parent component
  @Input("update-product-details") updateProductDetails: any;

  //Emitting values to parent component
  @Output("callBack-product") callBack = new EventEmitter<object>();

  productForm:any;
  submitted: boolean = false;


  //category
  categoryList:any;

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

  constructor(private _productService:ProductService,
              private _formBuilder:FormBuilder,
              private _toastrService:ToastrService,
              private _categoryService:CategoryService) { }

  ngOnInit(): void {
    this.getCategory();
        this.productForm = this._formBuilder.group({
          id: [null],
          name: ['',[Validators.required]],
          price: ['',[Validators.required]],
          description: ['',[Validators.required]],
          quantity: ['',[Validators.required]],
          categories:[null,[Validators.required]],
          productStatus:[null,[Validators.required]]
        })
      this.productForm.patchValue({
        id: this.updateProductDetails.id,
        name: this.updateProductDetails.name,
        price: this.updateProductDetails.price,
        description: this.updateProductDetails.description,
        quantity : this.updateProductDetails.quantity,
        categories: this.updateProductDetails.categories.map((obj:any) =>
          obj.categoryId
        ),
        productStatus: this.updateProductDetails.productStatus
      });
      
  }

  get getFormControl(){
    return this.productForm.controls;
  }
  onUpdateProduct()
  {
    console.log(this.productForm.value) 
    this.submitted = true;
    if(this.productForm.invalid) return;

      this._productService.UpdateProduct(this.productForm.value).subscribe({
        next: data =>{
          this._toastrService.info("The product has been added", "Success");
          this.callBack.emit();
        },
        error: err => {
          this._toastrService.error("The product added failed", "Error");
        }
      });
  }
  onChange(event:any)
  {
    this.productForm.value.categoryId = event.id;
  }
  getCategory()
  {
    this._categoryService.getAllCategory().subscribe({
      next: res =>{
        // console.log(res)
        this.categoryList = res;
      },
      error: err => {
        console.log(err);
      }
    });
  }



}
