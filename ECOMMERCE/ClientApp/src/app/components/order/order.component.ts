import { Component, OnInit } from '@angular/core';
import { OrderService } from 'src/app/service/order/order.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {

  openDetails:boolean = false;
  orderList:any;
  orderDetails:any;
  grandTotal:number = 0;
  constructor(private _orderService:OrderService) { }

  ngOnInit(): void {
    this.getOrder();
  }

  getOrder()
  {
    this._orderService.getAllOrders().subscribe({
      next: res =>{
        this.orderList = res;
        this.orderList.forEach((element:any) => {
          this.grandTotal += element.amount;
        });
      },
      error: err => {
        console.log(err);
      }
    });
  }

  openDetailsButton(orderDetails:any)
  {
    this.openDetails = true;
    this.orderDetails = orderDetails;
  }
  onClose()
  {
    this.openDetails = false;
  }

}
