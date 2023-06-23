import { Component, OnInit } from '@angular/core';
import { DashboardService } from 'src/app/service/dashboard/dashboard.service';

@Component({
  selector: 'app-popular-product-list',
  templateUrl: './popular-product-list.component.html',
  styleUrls: ['./popular-product-list.component.css']
})
export class PopularProductListComponent implements OnInit {
  popularProduct: any;
  popularProductAmount: any;
  price: any[] = [];

  constructor(private _dashboardService: DashboardService) { }

  ngOnInit(): void {
    this.getPopularProduct();
  }

  getPopularProduct() {
    this._dashboardService.getPopularProduct().subscribe({
      next: res => {
        this.popularProduct = res;
        console.log(this.popularProduct);

        this.popularProductAmount= JSON.parse(JSON.stringify(this.popularProduct));
        this.popularProductAmount.sort((a: any, b: any) =>
          b.totalPrice - a.totalPrice
        );

        console.log(this.popularProductAmount)
   
      },
      error: err => {
        console.log(err);
      }
    });
  }

}
