import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { AuthGuard } from './guard/auth.guard';
import { CategoryListComponent } from './components/category-list/category-list.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { PasswordChangeSuccessComponent } from './components/password-change-success/password-change-success.component';
import { CreateUserComponent } from './components/user-profile/create-user/create-user.component';
import { UserListComponent } from './components/user-profile/user-list/user-list.component';

const routes: Routes = [
  {
    path:"",
    redirectTo:"/",
    pathMatch:"full",
  },
  {
    path:"product-list",
    component:ProductListComponent,
    canActivate: [AuthGuard]
  },
  {
    path:"category-list",
    component:CategoryListComponent,
    canActivate: [AuthGuard]
  },
  {
    path:"dashboard",
    component:DashboardComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "profile",
    component: UserProfileComponent, 
    canActivate: [AuthGuard]
  },
  {
    path: "password-change-success",
    component: PasswordChangeSuccessComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "create-user",
    component: CreateUserComponent,
    canActivate: [AuthGuard]
  },
  {
    path: "user-list",
    component: UserListComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
