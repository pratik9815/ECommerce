import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  apiUrl:string = 'https://localhost:7069/api/Dashboard/'
  constructor(private _httpClient:HttpClient) { }

  getDashboardData()
  {
   return this._httpClient.get(this.apiUrl+'get-dashboard-data')
  }
  getPopularProduct()
  {
    return this._httpClient.get(this.apiUrl + 'get-popular-product');
  }
  getOrderStatus()
  {
    return this._httpClient.get(this.apiUrl+'get-order-status-for-dashboard');
  }
}
