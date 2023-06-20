import { Component, OnInit } from '@angular/core';
import {Chart , registerables} from 'node_modules/chart.js'
import { first } from 'rxjs';
import { DashboardService } from 'src/app/service/dashboard/dashboard.service';


Chart.register(...registerables);
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  dashboardData:any;
  amount:number[] = [];
  categoryName:string[] = [];
  popularProduct:any;

  firstColor: string = 'rgb(255,0,0)'
  secondColor:string = 'rgb(178, 255, 102)'
  thirdColor:string = 'rgb(255, 255, 102)'
  fourthColor:string = 'rgb(255, 138, 102)'
  fifthColor:string = 'rgb(51, 51, 255)'


  firstBorderColor:string = 'rgba(255, 0, 0,1)'
  secondBorderColor:string = 'rgba(0, 255, 0,1)'
  thirdBorderColor:string = 'rgba(255, 255, 0,1)'
  fourthBorderColor:string = 'rgba(255, 138, 0,1)'
  fifthBorderColor:string = 'rgba(51, 51, 255,1)'

  constructor(private _dashboardService:DashboardService) { }

  ngOnInit(): void {
    this.getDashboardData();  
    this.getProductData();
  }

  getProductData()
  {
    this._dashboardService.getPopularProduct().subscribe({
      next: res =>{
        this.popularProduct = res;
        console.log(this.popularProduct)
      }
    })
  }

  getDashboardData()
  {
    this._dashboardService.getDashboardData().subscribe({
      next: res =>{
        this.dashboardData =  res;
        this.dashboardData.map((data:any) =>{
          this.categoryName.push(data.categoryName)
          this.amount.push(data.amount)
        });
        this.RenderChart();
      }
    })
  }

  RenderChart()
  {
    const data = {
      labels:this.categoryName,
      datasets: [{
        label: 'Performances by category',
        data: this.amount,
        // backgroundColor: [
        //   'rgb(255,0,0)',
        //   'rgb(178, 255, 102)',
        //   'rgb(255, 255, 102)',
        //   'rgb(255, 138, 102)',   
        // ],
        backgroundColor:[this.fifthColor],
        borderColor: [this.fifthBorderColor
          // 'rgba(255, 0, 0,1)',
          // 'rgba(0, 255, 0,1)',
          // 'rgba(255, 255, 0,1)',
          // 'rgba(255, 138, 0,1)',      
        ],
        borderWidth:1,
        hoverOffset: 4,
        outerStart:20,
        outerEnd:20,
        innerStart:20,
        innerEnd:20,
      }]
    };

    for(var i=0; i< this.dashboardData.length; i++)
    {
      if(i === 0)
      {
        data.datasets[0].backgroundColor.push(this.firstColor);
        data.datasets[0].borderColor.push(this.firstBorderColor);
      }
      else if(i % 3 === 0)
      {
        data.datasets[0].backgroundColor.push(this.fourthColor);
        data.datasets[0].borderColor.push(this.fourthBorderColor);
      }
      else if(i % 2 === 0)
      {
        data.datasets[0].backgroundColor.push(this.secondColor);
        data.datasets[0].borderColor.push(this.secondBorderColor);
      }
      else
      {
        data.datasets[0].backgroundColor.push(this.thirdColor);
        data.datasets[0].borderColor.push(this.thirdBorderColor);
      // }
      // else if(i === 3)
      // {
      //   data.datasets[0].backgroundColor.push(this.fourthColor);
      //   data.datasets[0].borderColor.push(this.fourthBorderColor);
      // }
      // else if(i === 4)
      // {
      //   data.datasets[0].backgroundColor.push(this.fifthColor);
      //   data.datasets[0].borderColor.push(this.fifthBorderColor);
      }
    }

    const barGraph = new Chart("bargraph",
    {
      type: 'bar',
      data: data,
      options: {
        scales: {
          y: {
            beginAtZero: true
          }
        }
      },
    });

    const pieChart = new Chart("piechart",{
      type: 'pie',
      data: data,
      options:{
        plugins: {
          tooltip: {
            enabled: true,
          },
        },
        radius:160,
        elements:{
          arc: {
            borderWidth:10,
          },
        },
        layout:{
          padding:{
            left:5,
            right:5,
            top:5,
            bottom:5
          }
        },
      },
    });


    const lineChart = new Chart("lineChart",{
    
        type: 'line',
        data: data,
      
    })
  }

}
