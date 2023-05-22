import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { AgGridModule } from 'ag-grid-angular';
import {
  NgxAwesomePopupModule,
  DialogConfigModule,
  ConfirmBoxConfigModule,
  ToastNotificationConfigModule
} from '@costlydeveloper/ngx-awesome-popup';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/shared/navbar/navbar.component';
import { LoginComponent } from './components/login/login.component';
import { SidebarComponent } from './components/shared/sidebar/sidebar.component';
import { AuthService } from './services/auth/auth.service';
import { TokenInterceptor } from './interceptor/token.interceptor';
import { AuthGuard } from './guard/auth.guard';
import { ToastrModule } from 'ngx-toastr';
import { ProductService } from './services/product/product.service';
import { ProductListComponent } from './components/product-list/product-list.component';
import { CategoryListComponent } from './components/category-list/category-list.component';
import { AddproductComponent } from './components/product-list/addproduct/addproduct.component';
import { EditproductComponent } from './components/product-list/editproduct/editproduct.component';

import { UpdateCategoryComponent } from './components/category-list/update-category/update-category.component';
import { AddCategoryComponent } from './components/category-list/add-category/add-category.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ProductDetailsComponent } from './components/product-list/product-details/product-details.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { UpdateProfileComponent } from './components/user-profile/update-profile/update-profile.component';
import { ChangePassComponent } from './components/user-profile/change-pass/change-pass.component';
import { PasswordChangeSuccessComponent } from './components/password-change-success/password-change-success.component';
import { CreateUserComponent } from './components/user-profile/create-user/create-user.component';
import { UserListComponent } from './components/user-profile/user-list/user-list.component';
import { ProductWithCategoryComponent } from './components/product-list/product-with-category/product-with-category.component';



@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    LoginComponent,
    SidebarComponent,
    ProductListComponent,
    CategoryListComponent,
    AddproductComponent,
    EditproductComponent,
    AddCategoryComponent,
    UpdateCategoryComponent,
    DashboardComponent,
    ProductDetailsComponent,
    UserProfileComponent,
    UpdateProfileComponent,
    ChangePassComponent,
    PasswordChangeSuccessComponent,
    CreateUserComponent,
    UserListComponent,
    ProductWithCategoryComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    CommonModule,
    AgGridModule,
    FormsModule,
    ToastrModule.forRoot({
      timeOut: 4000,
      // closeButton: true,
      progressBar: true,
      tapToDismiss: true,
      preventDuplicates: true,
      countDuplicates: false,
      easeTime: 800,
      positionClass: 'toast-bottom-right'
    }),
    NgxAwesomePopupModule.forRoot(), // Essential, mandatory main module.
    DialogConfigModule.forRoot(), // Needed for instantiating dynamic components.
    ConfirmBoxConfigModule.forRoot(), // Needed for instantiating confirm boxes.
    ToastNotificationConfigModule.forRoot(), NgbModule, // Needed for instantiating toast notifications.
    NgSelectModule

    ],
  providers: [
    AuthService,
    AuthGuard,
    {provide: HTTP_INTERCEPTORS, useClass:TokenInterceptor,multi:true}, 
    ProductService  


  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
