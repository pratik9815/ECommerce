import { Component, Input, OnInit } from '@angular/core';
import { CategoryService } from 'src/app/service/category/category.service';
import { ProductService } from 'src/app/services/product/product.service';
import { ProductStatus } from 'src/app/services/web-api-client';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {
  @Input("product-details") ProductDetails: any;

   @Input("product-id") productId : any;
  productStatus:string;
  productData:any;
  constructor(private _productService:ProductService,private _categoryService:CategoryService) { }

  ngOnInit(): void {
    // console.log(this.productId)
    if(this.productId)
      this.getProduct()

    // console.log(ProductStatus[this.ProductDetails.productStatus]); This gives the enum value from the integer value returned from the asp.net
    this.productStatus = ProductStatus[this.ProductDetails.productStatus]
  }

  getProduct()
  {
    this._productService.GetProductWithId(this.productId).subscribe({
      next: (res:any) =>{
        console.log(res);
        this.productData = res;
      },
      error: err =>{
        console.log(err);
      }
    });
  }
}
