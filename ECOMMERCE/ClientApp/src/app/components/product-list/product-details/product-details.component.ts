import { Component, Input, OnInit } from '@angular/core';
import { CategoryService } from 'src/app/service/category/category.service';
import { ProductService } from 'src/app/services/product/product.service';
import { ProductStatus } from 'src/app/services/web-api-client';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {
  @Input("product-id") productId: any;

  productStatus:string;
  productData:any;
  imageUrl = environment.imageUrl;
  constructor(private _productService:ProductService,private _categoryService:CategoryService) { }

  ngOnInit(): void {
    // console.log(this.ProductDetails )
   
      this.getProduct()

  }

  getProduct()
  {
    this._productService.GetProductWithId(this.productId).subscribe({
      next: (res:any) =>{
        console.log(res);
        this.productData = res;
        this.productStatus = ProductStatus[this.productData.productStatus]
      },
      error: err =>{
        console.log(err);
      }
    });
  }
}
