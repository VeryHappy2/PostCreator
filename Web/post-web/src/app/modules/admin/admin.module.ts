import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuComponent } from './components/menu/menu.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AdminRoutingModule } from './admin-routing.module';
import { ChangeRoleComponent } from './components/dashboard/change-role/change-role.component';
import { DeleteUserComponent } from './components/dashboard/delete-user/delete-user.component';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { ReactiveFormsModule } from '@angular/forms';
import { SearchUserAdminComponent } from './components/search-user-admin/search-user-admin.component';
import { ModificationUserService } from './services/modification-user.service';
import { SearchService } from './services/search.service';



@NgModule({
  declarations: [
    MenuComponent,
    DashboardComponent,
    ChangeRoleComponent,
    DeleteUserComponent,
    SearchUserAdminComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    MatSelectModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
  ],
  providers: [
    ModificationUserService,
    SearchService
  ]
})
export class AdminModule { }
