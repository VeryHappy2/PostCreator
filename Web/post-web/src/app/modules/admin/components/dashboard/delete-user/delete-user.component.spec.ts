import { HttpClientTestingModule } from "@angular/common/http/testing";
import { CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";
import { ComponentFixture, TestBed } from "@angular/core/testing";
import { ModificationUserService } from "../../../services/modification-user.service";
import { DeleteUserComponent } from "./delete-user.component";
import { IGeneralResponse } from "../../../../../models/reponses/GeneralResponse";
import { SearchUserAdminComponent } from "../../search-user-admin/search-user-admin.component";
import { SearchService } from "../../../services/search.service";
import { MatAutocompleteModule } from "@angular/material/autocomplete";
import { ReactiveFormsModule } from "@angular/forms";
import { of } from "rxjs/internal/observable/of";
import { ResponseErrorHandlerService } from "../../../../../services/error/response-error-handler.service";
import { throwError } from "rxjs/internal/observable/throwError";
import { HttpErrorResponse } from "@angular/common/http";

describe('DeleteUserComponent', () => {
  let component: DeleteUserComponent;
  let fixture: ComponentFixture<DeleteUserComponent>;
  let modificationUserService: ModificationUserService;
  let errorHandler: ResponseErrorHandlerService

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, MatAutocompleteModule, ReactiveFormsModule],
      declarations: [DeleteUserComponent, SearchUserAdminComponent],
      providers: [
        ModificationUserService,
        SearchService,
        ResponseErrorHandlerService
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(DeleteUserComponent);
    component = fixture.componentInstance;
    component.searcherUserAdmin = TestBed.createComponent(SearchUserAdminComponent).componentInstance;

    modificationUserService = TestBed.inject(ModificationUserService)
    errorHandler = TestBed.inject(ResponseErrorHandlerService)
    fixture.detectChanges();
  });

  it('should call deleteUserAsync with the correct request when delete is called', async () => {
    const mockUserName: string | null = 'testUser';
    spyOn(component.searcherUserAdmin, 'fetchUserNameData').and.returnValue(mockUserName);
    
    const mockResponse: IGeneralResponse<null> = { message: 'User deleted', flag: true, data: null };
    spyOn(modificationUserService, "deleteUser").and.returnValue(of(mockResponse))

    component["delete"]();

    expect(component.searcherUserAdmin.fetchUserNameData).toHaveBeenCalled();
    expect(modificationUserService.deleteUser).toHaveBeenCalledWith({ name: mockUserName });
    expect(component["check"]).toEqual(mockResponse);
  });

  it('should not call deleteUserAsync if fetchUserNameData returns null', async () => {
    const check: IGeneralResponse<null> = {
      flag: false,
      message: "User name data is emtpy",
    } 

    spyOn(component.searcherUserAdmin, 'fetchUserNameData').and.returnValue(null);
    spyOn(modificationUserService, 'deleteUser');
    spyOn(console, "warn")

    component["delete"]();

    expect(component.searcherUserAdmin.fetchUserNameData).toHaveBeenCalled();
    expect(modificationUserService.deleteUser).not.toHaveBeenCalled();
    expect(component["check"]).toEqual(check);
    expect(console.warn).toHaveBeenCalledOnceWith("User name data is invalid")
  });

  it('should handle unsuccessful deletion attempts correctly', () => {
    const mockUserName = 'testUser';    
    const mockErrorResponse = {
      error: { 
        errors: { 
          name: ["User not found"] 
        } 
      },
    };

    const check = {
      flag: false,
      message: 'User not found',
      data: null
    }

    spyOn(component.searcherUserAdmin, 'fetchUserNameData').and.returnValue(mockUserName);
    spyOn(modificationUserService, 'deleteUser').and.returnValue(throwError(mockErrorResponse));
    spyOn(errorHandler, "GetMessageError").and.returnValue("User not found")

    component["delete"]();

    expect(modificationUserService.deleteUser).toHaveBeenCalledOnceWith({ name: mockUserName });
    expect(errorHandler.GetMessageError).toHaveBeenCalledOnceWith(mockErrorResponse.error)
    expect(component["check"]).toEqual(check)
  });
});
