import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Chart,   registerables } from 'node_modules/chart.js'
import { DashboardService } from 'src/app/service/dashboard/dashboard.service';
import { OrderStatus } from 'src/app/services/web-api-client';
import {ChartType,ChartData,ChartOptions} from 'chart.js'
import { Color } from 'ag-grid-community';


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


  blueColorRGBA: string = 'rgba(0, 123, 255, 0.6)';
  greenColorRGBA: string = 'rgba(40, 167, 69, 0.6)';
  redColorRGBA: string = 'rgba(220, 53, 69, 0.6)';
  orangeColorRGBA: string = 'rgba(253, 126, 20, 0.6)';
  purpleColorRGBA: string = 'rgba(111, 66, 193, 0.6)';

  hoverBlueColor: string = 'rgba(0, 123, 255, 1)';
  hoverGreenColor: string = 'rgba(40, 167, 69, 1)';
  hoverRedColor: string = 'rgba(220, 53, 69, 1)';
  hoverOrangeColor: string = 'rgba(253, 126, 20, 1)';
  hoverPurpleColor: string = 'rgba(111, 66, 193, 1)';
  


  //For ngOnDestory
  data: any;


  orderStatusData: any;
  orderStatus: any[] = [];
  orderedQuantity: number[] = [];
  todayData: any;
  revenueData: any;


barData:ChartData<'bar'>;
barOptions:ChartOptions<'bar'>;
pieOptions:ChartOptions<'pie'>;
pieData:ChartData<'pie'>;

barOrderData:ChartData<'bar'>;
barOrderOptions:ChartOptions<'bar'>;
    

  constructor(private _dashboardService: DashboardService) { }

  ngOnDestroy(): void {
    console.log("This method is destroyed");
    // this.data.unsubscribe();
  }

  ngOnInit(): void {
    this.getQuantityAndAmountForToday();
    this.getDashboardData();
    this.getTotalRevenue();
    this.getOrderStatusData();
  }
  
// BarChartRender()
// {
//   this.barData = {
//     labels: this.categoryName,
//     datasets: [
//       {
//         label: 'By category',
//         data: this.amount,
//         backgroundColor:[this.blueColorRGBA,this.greenColorRGBA,this.purpleColorRGBA,this.redColorRGBA,this.orangeColorRGBA],        
//         hoverBackgroundColor:[this.hoverBlueColor,this.hoverGreenColor,this.hoverPurpleColor,this.hoverRedColor,this.hoverOrangeColor],
//       }
//     ]
//   };

//   this.barOptions= {
//     scales: {
//       y: {
//         beginAtZero: true
//       }
//     }
//   }
// }

PieChartRender()
{
 this.pieData = {
  labels: this.categoryName,
  datasets: [{
    label:"By Category",
    data: this.amount,
    backgroundColor:[this.blueColorRGBA,this.greenColorRGBA,this.purpleColorRGBA,this.redColorRGBA,this.orangeColorRGBA,this.purpleColorRGBA],
    hoverBackgroundColor:[this.hoverBlueColor,this.hoverGreenColor,this.hoverPurpleColor,this.hoverRedColor,this.hoverOrangeColor,,this.hoverPurpleColor,],
    borderColor:[this.blueColorRGBA,this.greenColorRGBA,this.purpleColorRGBA,this.redColorRGBA,this.orangeColorRGBA,this.purpleColorRGBA],
    hoverBorderColor:[this.hoverBlueColor,this.hoverGreenColor,this.hoverPurpleColor,this.hoverRedColor,this.hoverOrangeColor,this.hoverPurpleColor,]
  }]
 };

 this.pieOptions = {
  radius:140,
  scales:{

  }
 }
}

RenderBarChartForOrders()
{
  this.barOrderData = {
    labels: this.orderStatus,
    datasets: [
      {
        label: 'Order Status',
        data: this.orderedQuantity,
        backgroundColor:[this.blueColorRGBA,this.greenColorRGBA,this.purpleColorRGBA,this.redColorRGBA,this.orangeColorRGBA,this.purpleColorRGBA],
        hoverBackgroundColor:[this.hoverBlueColor,this.hoverGreenColor,this.hoverPurpleColor,this.hoverRedColor,this.hoverOrangeColor,,this.hoverPurpleColor,],
        borderColor:[this.blueColorRGBA,this.greenColorRGBA,this.purpleColorRGBA,this.redColorRGBA,this.orangeColorRGBA,this.purpleColorRGBA],
        hoverBorderColor:[this.hoverBlueColor,this.hoverGreenColor,this.hoverPurpleColor,this.hoverRedColor,this.hoverOrangeColor,this.hoverPurpleColor,],

        maxBarThickness:90,
        borderWidth:1,
        barPercentage:1,
        minBarLength:2,
        categoryPercentage:0.8
        
      }
    ]
  };

  this.barOptions= {  
    responsive:true,
    maintainAspectRatio: false,
    
    plugins:{
      title:{
        display:true,
        text:"Bar Graph",
        font:{
          size:15
        },
        position:'top'
      }
    },
    scales: {
      y: {
        beginAtZero:true,
        ticks:{
          stepSize:0.4
        }
      },
      x: {
        beginAtZero:true,
        ticks:{
          stepSize:0.5
        }
      },
      
    },
    
  }
}

  getTotalRevenue()
  {
    this._dashboardService.getTotalRevenue().subscribe({
      next: res =>{
        console.log(res)
        this.revenueData = res;
      }
    })
  }
  getDashboardData() {
    this.data = this._dashboardService.getDashboardData().subscribe({
      next: res => {
        this.dashboardData = res;
        this.dashboardData.map((data: any) => {
          this.categoryName.push(data.categoryName)
          this.amount.push(data.amount)
        });
        // this.BarChartRender();
        this.PieChartRender();
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
        // this.RenderChart();
        this.RenderBarChartForOrders();
      }
    });
  }


  getQuantityAndAmountForToday()
  {
    this._dashboardService.getQuantityAndAmount().subscribe({
      next: res =>{
        console.log(res);
        this.todayData = res;
      }
    })
  }

}
