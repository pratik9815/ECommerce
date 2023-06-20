import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/service/user/user.service';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.css']
})
export class CustomerListComponent implements OnInit {

  customerData:any;
  customerColDef:any;
  domLayout:any;
  defaultColDef:any;
  quickSearchValue:any;
  gridApi: any;
  gridColumnApi: any;
  constructor(private _userService:UserService) {
    this.domLayout = "autoHeight";
   }

  ngOnInit(): void {
    this.getCustomer();
    this.customerColDef = [
      {headerName: "S.N", valueGetter: 'node.rowIndex+1',width: 30 , resizable: false},
      {headerName: "Name",field: "fullName",sortable: true, resizable: true, filter: true, width: 100},
      {headerName: "Address",field: "address",sortable: true, resizable: true, filter: true, width: 100},
      {headerName: "Email",field: "email",sortable: true, resizable: true, filter: true, width: 100},
      {headerName: "Gender",field: "gender",sortable: true, resizable: true, filter: true, width: 100},
      {headerName: "Username",field: "userName",sortable: true, resizable: true, filter: true, width: 100},

    ]


  }
  onFilterChanged(e:any)
  {
    e.api.refreshCells();
  }
  onModelUpdated()
  {
    setTimeout(() => { this.gridColumnApi.autoSizeAllColumns() });
  }
  onGridReady(params:any)
  {
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;
  }
  onRowClicked(e:any)
  {

  }
  onQuickFilterChanged()
  {
    this.gridApi.setQuickFilter(this.quickSearchValue);
  }
  getCustomer()
  {
    this._userService.getCustomer().subscribe({
      next: res =>{
        this.customerData = res;
        console.log(this.customerData)
      }
    })
  }

}
