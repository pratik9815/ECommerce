import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  apiUrl: string = 'https://localhost:7069/api/Order/'
  constructor(private _httpClient: HttpClient) { }


  getAllOrders() {
    return this._httpClient.get(this.apiUrl + 'get-orders');
  }
  
  getOrderWithStatus(orderStatus:any) {
    return this._httpClient.get(this.apiUrl + 'get-order-with-status/' + orderStatus)
  }
  changeOrderStatus(orderId:any, orderStatus:any)
  {
    return this._httpClient.post(this.apiUrl+'change-order-status/'+orderId,orderStatus);
  }
}
