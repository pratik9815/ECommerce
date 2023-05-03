import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  apiUrl: string = "https://localhost:7069/api/Category/" ;

  constructor(private _httpClient:HttpClient) { }

  getAllCategory()
  {
    return this._httpClient.get(this.apiUrl+'get-category');
  }
  createCategory(category:any)
  {
     return this._httpClient.post(this.apiUrl+'create-category',category);
  }
  updateCategory(category:any)
  {
    return this._httpClient.put(this.apiUrl+'update-category',category);
  }
  removeCategory(id:any)
  {
    return this._httpClient.delete(this.apiUrl+"delete-category/"+id);
  }
}
