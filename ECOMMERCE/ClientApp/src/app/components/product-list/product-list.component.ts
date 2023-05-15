import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/services/product/product.service';
import {
  ConfirmBoxInitializer,
  DialogLayoutDisplay,
  DisappearanceAnimation,
  AppearanceAnimation
} from '@costlydeveloper/ngx-awesome-popup';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {

  //For AgGrid table
  defaultColDef:any;
  productColDef: any;
  productData: any;
  productDataClone: any;
  gridApi: any;
  gridColumnApi: any;
  domLayout: any;
  quickSearchValue: any;
  updateProductDetails: any;
  productId: any;
  ProductDetails: any;

  //for model popup
  addProductPopUpModal: boolean = false;
  updateProductPopUpModal: boolean = false;
  CategoryPopUpModal: boolean = false;
  productDetailsPopUpModal: boolean = false;

  constructor(private _productService: ProductService,
    private _toastrService: ToastrService) {
    this.domLayout = "autoHeight";

  }

  ngOnInit(): void {
    this.getProduct();
    this.productColDef = [
      { headerName: "S.N", valueGetter: 'node.rowIndex+1', width: 40, resizable: true },
      { headerName: "Name", field: 'name', sortable: true, resizable: true, filter: true, width: 100 },
      { headerName: "Description", field: 'description', sortable: true, resizable: true, width: 100 },
      { headerName: "Unit Price", field: 'price', sortable: true, resizable: true, width: 100 },
      { headerName: "Quantity", field: 'quantity', sortable: true, resizable: true, width: 100 },
      { headerName: "CreatedBy", field: 'createdBy', sortable: true, resizable: true, width: 100 },
      { headerName: "UpdateddBy", field: 'updatedBy', sortable: true, resizable: true, width: 100 },
      { headerName: "Actions", field: 'action', cellRenderer: this.actions(), pinned: 'right', resizable: true, width: 10 },
    ];
  }
  // <button type="button" data-action-type="Cateogry" class="btn ag-btn btn-secondary" 
  // data-toggle="tooltip" data-placement="bottom" title = "Category" ><i class="fa-solid fa-pen-nib"></i></button> &nbsp;
  public actions() {
    return function (params: any) {
      return ` 
          <button type="button" data-action-type="Edit" class="btn ag-btn"> <i data-action-type="Edit" class="fa-solid fa-pen-to-square"></i></button>
          <button type="button" data-action-type="Remove" class="btn ag-btn">  <i data-action-type="Remove" class="fas fa-trash"></i></button>
          <button type="button" data-action-type="Details" class="btn ag-btn"><i data-action-type="Details" class="fa-solid fa-eye"></i></button>`;
    }
  }

  getProduct() {
    this._productService.GetAllProductWithAllImage().subscribe({
      next: (res: any) => {
        this.productData = res;
        this.productDataClone = [...res];
        console.log(res);
      },
      error: err => {
        console.log(err);
      }
    });
  }

  onChange(e: any) {
    console.log(e.value)
  }



  onGridReady(params: any) {
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;
  }

  onRowClicked(e: any) {
    if (e.event.target) {
      let data = e.data;
      let actionType = e.event.target.getAttribute("data-action-type");

      switch (actionType) {
        case "Edit": {
          this.updateProductDetails = data;
          this.updateProductPopUpModal = true;
          break;
        }
        case "Remove": {
          return this.onOpenDialog(data);
        }
        case "Cateogry": {
          this.productId = data.id;
          this.CategoryPopUpModal = true;
          break;
        }
        case "Details": {

          this.ProductDetails = data;
          this.productDetailsPopUpModal = true;
          break;
        }
      }
    }
  }

  onFilterChanged(e: any) {
    e.api.refreshCells();
  }

  onQuickFilterChanged() {
    this.gridApi.setQuickFilter(this.quickSearchValue);
  }

  onModelUpdated() {
    setTimeout(() => { this.gridColumnApi.autoSizeAllColumns() });
  }

  onAddNewProductPopUp() {
    this.addProductPopUpModal = true;
  }

  close() {
    this.addProductPopUpModal = false;
    this.updateProductPopUpModal = false;
    this.CategoryPopUpModal = false;
    this.productDetailsPopUpModal = false;
  }

  onOpenDialog(row: any) {
    const newConfirmBox = new ConfirmBoxInitializer();

    newConfirmBox.setTitle('Warning!!!');
    newConfirmBox.setMessage('Are you sure you want to remove this product?');
    newConfirmBox.setButtonLabels('YES', 'NO');
    // Choose layout color type
    newConfirmBox.setConfig({
      layoutType: DialogLayoutDisplay.WARNING,// SUCCESS | INFO | NONE | DANGER | WARNING
      animationIn: AppearanceAnimation.BOUNCE_IN, // BOUNCE_IN | SWING | ZOOM_IN | ZOOM_IN_ROTATE | ELASTIC | JELLO | FADE_IN | SLIDE_IN_UP | SLIDE_IN_DOWN | SLIDE_IN_LEFT | SLIDE_IN_RIGHT | NONE
      animationOut: DisappearanceAnimation.BOUNCE_OUT, // BOUNCE_OUT | ZOOM_OUT | ZOOM_OUT_WIND | ZOOM_OUT_ROTATE | FLIP_OUT | SLIDE_OUT_UP | SLIDE_OUT_DOWN | SLIDE_OUT_LEFT | SLIDE_OUT_RIGHT | NONE
    });

    // Simply open the popup
    newConfirmBox.openConfirmBox$().subscribe((res: any) => {
      if (res.clickedButtonID === 'yes') {
        this._productService.DeleteProduct(row.id).subscribe({
          next: res => {
            this._toastrService.success('Product removed successfully.', 'Success!');
            this.getProduct();
          },
          error: err => {
            this._toastrService.error('Something went wrong! Please try again...', 'Error!');
          }
        });
      }
    });
  }

  callBack(): void {
    this.close();
    this.getProduct();
  }



}