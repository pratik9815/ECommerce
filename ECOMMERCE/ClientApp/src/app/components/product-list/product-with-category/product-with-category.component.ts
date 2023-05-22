import { Component, OnInit } from '@angular/core';
import { CategoryService } from 'src/app/service/category/category.service';
import { ProductService } from 'src/app/services/product/product.service';

@Component({
  selector: 'app-product-with-category',
  templateUrl: './product-with-category.component.html',
  styleUrls: ['./product-with-category.component.css']
})
export class ProductWithCategoryComponent implements OnInit {

  categoryList:any;
  categoryId:any;
  productList:any;
  isItemSelected:boolean;
  productId:any;
  productDetailsPopUpModal:boolean = false;
  constructor(private _categoryService:CategoryService,
              private _productService:ProductService) { }

  ngOnInit(): void {
    this.getCategory();
  }

  getCategory()
  {
    this._categoryService.getAllCategory().subscribe({
      next: res =>{
        console.log(res);
        this.categoryList = res;
      },
      error: err => {
        console.log(err);
      }
    })
  }

  getProductWithCategory()
  {
    this._productService.getProductWithCategory(this.categoryId).subscribe({
      next: res =>{
        console.log(res);
        this.productList = res;
      }
    })
  }
  

  onChange(e:any)
  {
    this.isItemSelected = !!e;
    this.categoryId = e.id;
    console.log(this.categoryId);
    this.getProductWithCategory();
  }

  getId(productid:any)
  {
    this.productDetailsPopUpModal = true;
    this.productId = productid;
  }

  close()
  {
    this.productDetailsPopUpModal = false;
  }
}
