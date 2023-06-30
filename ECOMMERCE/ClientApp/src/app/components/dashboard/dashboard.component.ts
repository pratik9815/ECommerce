import { Component, OnDestroy, OnInit } from '@angular/core';
import { Chart, registerables } from 'node_modules/chart.js'
import { DashboardService } from 'src/app/service/dashboard/dashboard.service';
import { OrderStatus } from 'src/app/services/web-api-client';


Chart.register(...registerables);
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, OnDestroy {

  dashboardData: any;
  amount: number[] = [];
  categoryName: string[] = [];


  firstColor: string = '#32FF62'
  secondColor: string = '#FF3333'
  thirdColor: string = '#E9EF39'
  fourthColor: string = 'rgb(255, 138, 102)'
  fifthColor: string = 'aqua'


  firstBorderColor: string = '#32FF62'
  secondBorderColor: string = '#FF3333'
  thirdBorderColor: string = '#E9EF39'
  fourthBorderColor: string = 'rgba(255, 138, 0,1)'
  fifthBorderColor: string = 'aqua'
  //For ngOnDestory
  data: any;

  orderStatusData: any;
  orderStatus: any[] = [];
  orderedQuantity: number[] = [];
  constructor(private _dashboardService: DashboardService) { }

  ngOnDestroy(): void {
    console.log("This method is destroyed");
    // this.data.unsubscribe();
  }

  ngOnInit(): void {
    this.getDashboardData();
  }

  getDashboardData() {
    this.data = this._dashboardService.getDashboardData().subscribe({
      next: res => {
        this.dashboardData = res;
        this.dashboardData.map((data: any) => {
          this.categoryName.push(data.categoryName)
          this.amount.push(data.amount)
        });
        this.getOrderStatusData()
      }
    });
  }

  getOrderStatusData() {
    this._dashboardService.getOrderStatus().subscribe({
      next: (res: any) => {
        this.orderStatusData = res;
        this.orderStatusData.map((data: any) => {
          this.orderedQuantity.push(data.orderedQuantity)
          this.orderStatus.push(OrderStatus[data.orderStatus])
        })
        this.RenderChart();
      }
    })
  }

  RenderChart() {
    const data = {
      labels: this.categoryName,
      datasets: [{
        label: 'Performances by category',
        data: this.amount,
        backgroundColor: [this.fifthColor],
        borderColor: [this.fifthBorderColor],
        borderWidth: 1,
        hoverOffset: 4,
        outerStart: 20,
        outerEnd: 20,
        innerStart: 20,
        innerEnd: 20,
      }]
    };

    const orderStatusData = {
      labels: this.orderStatus,
      datasets: [{
        label: 'Orders',
        data: this.orderedQuantity,
        backgroundColor: [this.firstColor],
        borderColor: [this.firstBorderColor],
        borderWidth: 1,
        hoverOffset: 4,
        outerStart: 20,
        outerEnd: 20,
        innerStart: 20,
        innerEnd: 20,
      }]
    };

    for (var i = 0; i < this.dashboardData.length; i++) {
      if (i === 0) {
        data.datasets[0].backgroundColor.push(this.firstColor);
        data.datasets[0].borderColor.push(this.firstBorderColor);
      }
      else if (i % 3 === 0) {
        data.datasets[0].backgroundColor.push(this.fourthColor);  
        data.datasets[0].borderColor.push(this.fourthBorderColor);
      }
      else if (i % 2 === 0) {
        data.datasets[0].backgroundColor.push(this.secondColor);
        data.datasets[0].borderColor.push(this.secondBorderColor);
      }
      else {
        data.datasets[0].backgroundColor.push(this.thirdColor);
        data.datasets[0].borderColor.push(this.thirdBorderColor);
      }
    }


    for (var i = 0; i < this.orderStatusData.length; i++) {
      if (i === 0) {
        orderStatusData.datasets[0].backgroundColor.push(this.secondColor);
        orderStatusData.datasets[0].borderColor.push(this.secondBorderColor);
      }
      else if (i % 3 === 0) {
        orderStatusData.datasets[0].backgroundColor.push(this.fourthColor);
        orderStatusData.datasets[0].borderColor.push(this.fourthBorderColor);
      }
      else if (i % 2 === 0) {
        orderStatusData.datasets[0].backgroundColor.push(this.secondColor);
        orderStatusData.datasets[0].borderColor.push(this.secondBorderColor);
      }
      else {
        orderStatusData.datasets[0].backgroundColor.push(this.thirdColor);
        orderStatusData.datasets[0].borderColor.push(this.thirdBorderColor);
      }
    }


    // const barGraph = new Chart("bargraph",
    //   {
    //     type: 'bar',
    //     data: data,
    //     options: {
    //       scales: {
    //         y: {
    //           beginAtZero: true
    //         }
    //       }
    //     },
    //   });

    const pieChart = new Chart("piechart", {
      type: 'pie',
      data: data,
      options: {
        plugins: {
          tooltip: {
            enabled: true,
          },
        },
        radius: 140,
        elements: {
          arc: {
            borderWidth: 10,
          },
        },
        layout: {
          padding: {
            left: 5,
            right: 5,
            top: 5,
            bottom: 5
          }
        },
      },
    });


    const lineChart = new Chart("lineChart", {
      type: 'line',
      data: data,

    });


    const barGraphForOrderStatus = new Chart("barGraphForOrderStatus",
      {
        type: 'bar',
        data: orderStatusData,
        options: {
          scales: {
            y: {
              beginAtZero: true
            }
          }
        },
      });
  }
}
