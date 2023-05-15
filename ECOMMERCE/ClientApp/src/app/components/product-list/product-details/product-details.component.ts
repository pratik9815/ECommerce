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

   

  productData:any;
  productDataClone:any;
  categoryList:any;
  constructor(private _productService:ProductService,private _categoryService:CategoryService) { }

  ngOnInit(): void {
    this.getProduct()
    console.log(this.ProductDetails);
  
  }

  getProduct()
  {
    this._productService.GetAllProduct().subscribe({
      next: (res:any) =>{
        this.productData = res;
        this.productDataClone = [...res];

      },
      error: err =>{
        console.log(err);
      }
    });
  }
}