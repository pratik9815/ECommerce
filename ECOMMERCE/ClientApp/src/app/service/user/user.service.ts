import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  apiUrl: string = "https://localhost:7069/api/User/";
  apiApplication:string = "https://localhost:7069/api/ApplicationUser/create-user";

  constructor(private _httpClient:HttpClient) { }

  getUser()
  {
    return this._httpClient.get(this.apiUrl+"get-user");
  }



  getSuperAdminUser()
  {
    return this._httpClient.get(this.apiUrl+"get-superadmin-user");
  }
  getAdminUser()
  {
    return this._httpClient.get(this.apiUrl+"get-admin-user");
  }

  createUser(body:any)
  {
    return this._httpClient.post(this.apiApplication,body,
      {responseType: 'text'});
  }
  getCustomer()
  {
    return this._httpClient.get(this.apiUrl+"get-customer-user");
  }
}
