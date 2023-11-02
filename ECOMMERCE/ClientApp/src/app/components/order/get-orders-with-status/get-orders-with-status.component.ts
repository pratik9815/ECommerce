import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { OrderService } from 'src/app/service/order/order.service';
import { OrderStatus } from 'src/app/services/web-api-client';

@Component({
  selector: 'app-get-orders-with-status',
  templateUrl: './get-orders-with-status.component.html',
  styleUrls: ['./get-orders-with-status.component.css']
})
export class GetOrdersWithStatusComponent implements OnInit {
  orderList: any;
  grandTotal: number = 0;
  openDetails: boolean;
  orderDetails: any;
  orderStatus: any;
  changeStatus: boolean = true;
  nextStatus: any;
  changeStatusClicked: boolean = false;
  orderId: any;

  constructor(private _orderService: OrderService, private _router: Router, private _activatedRoute: ActivatedRoute, private _toastrService: ToastrService) {
    this._router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    this.orderStatus = this._activatedRoute.snapshot.paramMap.get('orderStatus');
    if (OrderStatus[this.orderStatus] == OrderStatus[3]) {
      this.changeStatus = false;
    }
    this.getOrder(this.orderStatus);
  }



  // Create a method to get the status name
  // getStatusName(statusCode: any): string {
  //   switch (statusCode) {
  //     case OrderStatus.Pending:
  //       return 'Pending';
  //     case OrderStatus.OrderRejected:
  //       return 'Order Rejected';
  //     case OrderStatus.OrderProcessing:
  //       return 'Order Processing';
  //     case OrderStatus.OrderDelivered:
  //       return 'Order Delivered';
  //     default:
  //       return 'Unknown';
  //   }
  // }



  getOrder(orderStatus: any) {
    this._orderService.getOrderWithStatus(orderStatus).subscribe({
      next: res => {
        this.orderList = res;
        this.orderList.forEach((element: any) => {
          this.grandTotal += element.amount;
        });
      },
      error: err => {
        console.log(err);
      }
    });
  }
  openDetailsButton(orderDetails: any) {
    this.openDetails = true;
    this.orderDetails = orderDetails;
  }

  onClose() {
    this.openDetails = false;
  }

  onChangeStatus(orderId: any) {
    this.orderId = orderId;
    if (OrderStatus[this.orderStatus] === OrderStatus[0]) {
      this.nextStatus = OrderStatus.OrderProcessing;
    }
    else if (OrderStatus[this.orderStatus] === OrderStatus[2]) {
      this.nextStatus = OrderStatus.OrderDelivered;
    }

    this.changeStatusClicked = true;
  }

  saveStatusChange()
  {
    this._orderService.changeOrderStatus(this.orderId,this.nextStatus).subscribe({
      next: res =>{
        this._toastrService.success("Order status changed to "+ OrderStatus[this.nextStatus],"Success");
        this.getOrder(this.orderStatus);
     },
     error: err =>{
      this._toastrService.error("Something went wrong!!","Error")
     }
    });

  }

}
