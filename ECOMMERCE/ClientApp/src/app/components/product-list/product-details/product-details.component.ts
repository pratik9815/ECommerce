import { Component, Input, OnInit } from '@angular/core';
import { CategoryService } from 'src/app/service/category/category.service';
import { ProductService } from 'src/app/services/product/product.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {
  @Input("product-details") ProductDetails: any;

   @Input("product-id") productId : any;

  productData:any;
  constructor(private _productService:ProductService,private _categoryService:CategoryService) { }

  ngOnInit(): void {
    // console.log(this.productId)
    if(this.productId)
      this.getProduct()
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
