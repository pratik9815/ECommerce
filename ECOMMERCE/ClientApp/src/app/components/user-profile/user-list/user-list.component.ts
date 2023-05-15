import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/service/user/user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  userData:any;

  userColDef:any;
  domLayout:any;
  gridApi:any;
  gridColumnApi:any;
  constructor(private _userService:UserService,private _toastrService:ToastrService) {
    this.domLayout = "autoHeight";
   }

  ngOnInit(): void {
    this.getUsers();
    this.userColDef = [
      { headerName: "S.N", valueGetter: 'node.rowIndex+1', width: 70, resizable: true },
      { headerName: "Name", field: 'fullName', sortable: true, resizable: true, filter: true, width: 150 },
      { headerName: "Address", field: 'address', sortable: true, resizable: true, width: 100 },
      { headerName: "Email", field: 'email', sortable: true, resizable: true, width: 150 },
      { headerName: "UserType", field: 'userType', sortable: true, resizable: true, width: 150 },
      { headerName: "Phone", field: 'phoneNumber', sortable: true, resizable: true, width: 150 },];
  }


  onGridReady(params: any) {
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;
  }

  onFilterChanged(e: any) {
    e.api.refreshCells();
  }

  getUsers()
  {
    this._userService.getUser().subscribe({
      next: res =>{
        console.log(res)
        this.userData = res;
      },
      error: err =>{
        this._toastrService.error("Something went wrong","Error");
        console.log(err);
      }
    })
  }

}
