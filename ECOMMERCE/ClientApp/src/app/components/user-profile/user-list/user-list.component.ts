import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/service/user/user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  createUserPopUpModal:boolean = false;

  superAdminUserData:any;
  adminUserData: any;


  userColDef:any;
  domLayout:any;
  gridApi:any;
  gridColumnApi:any;
  constructor(private _userService:UserService,private _toastrService:ToastrService) {
    this.domLayout = "autoHeight";
   }

  ngOnInit(): void {
    this.getSuperAdminUser();
    this.getAdminUser();
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

  getSuperAdminUser()
  {
    this._userService.getSuperAdminUser().subscribe({
      next: res =>{
        this.superAdminUserData = res;
      },
      error: err =>{
        this._toastrService.error("Something went wrong","Error");
        console.log(err);
      }
    });
  }
  getAdminUser()
  {
    this._userService.getAdminUser().subscribe({
      next: res =>{
        this.adminUserData = res;
      },
      error: err =>{
        this._toastrService.error("Something went wrong","Error");
        console.log(err);
      }
    });
  }

  onCreateUser()
  {
    this.createUserPopUpModal = true;
  }
  close()
  {
    this.createUserPopUpModal = false;
  }
  callback()
  {
    this.createUserPopUpModal = false;
    this.getSuperAdminUser();
  }

}
