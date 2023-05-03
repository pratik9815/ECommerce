import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  apiUrl : string = "https://localhost:7069/api/Product/";
  constructor(private _httpClient:HttpClient) { }

  GetAllProduct()
  {
    return this._httpClient.get(this.apiUrl+"get-product");
  }
  GetAllProductWithImage()
  {
    return this._httpClient.get(this.apiUrl+'get-product-with-image')
  }
  GetAllProductWithAllImage()
  {
    return this._httpClient.get(this.apiUrl+'get-images-with-all-images')
  }

  CreateProduct(product : any)
  {
    return this._httpClient.post(this.apiUrl+"create-product", product);
  }

  CreateProductWithMultipleImages(product : any)
  {
    return this._httpClient.post(this.apiUrl+'create-product-with-multiple-image',product)
  }
  
  UpdateProduct(product:any) 
  {
    return this._httpClient.put(this.apiUrl+"update-product",product);
  }
  DeleteProduct(id:any)
  {
    return this._httpClient.put(this.apiUrl+"delete-product/"+id,null);
  }
}
