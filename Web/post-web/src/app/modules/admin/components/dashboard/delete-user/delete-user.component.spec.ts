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

describe('DeleteUserComponent', () => {
  let component: DeleteUserComponent;
  let fixture: ComponentFixture<DeleteUserComponent>;
  let modificationUserService: ModificationUserService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, MatAutocompleteModule, ReactiveFormsModule],
      declarations: [DeleteUserComponent, SearchUserAdminComponent],
      providers: [
        ModificationUserService,
        SearchService
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();

    fixture = TestBed.createComponent(DeleteUserComponent);
    component = fixture.componentInstance;
    component.searcherUserAdmin = TestBed.createComponent(SearchUserAdminComponent).componentInstance;
    modificationUserService = TestBed.inject(ModificationUserService)
    fixture.detectChanges();
  });

  it('should call deleteUserAsync with the correct request when delete is called', async () => {
    const mockUserName: string | null = 'testUser';
    spyOn(component.searcherUserAdmin, 'fetchUserNameData').and.returnValue(mockUserName);
    const mockResponse: IGeneralResponse<null> = { message: 'User deleted', flag: true, data: null };
    spyOn(modificationUserService, "deleteUserAsync").and.returnValue(Promise.resolve(mockResponse));

    await component["delete"]();

    expect(component.searcherUserAdmin.fetchUserNameData).toHaveBeenCalled();
    expect(modificationUserService.deleteUser).toHaveBeenCalledWith({ name: mockUserName });
    expect(component["check"]).toEqual(mockResponse);
  });

  it('should not call deleteUserAsync if fetchUserNameData returns null', async () => {
    spyOn(component.searcherUserAdmin, 'fetchUserNameData').and.returnValue(null);
    spyOn(modificationUserService, 'deleteUserAsync');
    spyOn(console, "warn")

    await component["delete"]();

    expect(component.searcherUserAdmin.fetchUserNameData).toHaveBeenCalled();
    expect(modificationUserService.deleteUser).not.toHaveBeenCalled();
    expect(component["check"]).toBeUndefined();
    expect(console.warn).toHaveBeenCalledOnceWith("User name data is invalid")
  });

  it('should handle unsuccessful deletion attempts correctly', async () => {
    const mockUserName = 'testUser';
    spyOn(component.searcherUserAdmin, 'fetchUserNameData').and.returnValue(mockUserName);

    const mockResponse: IGeneralResponse<null> = { message: 'Failed to delete user', flag: false, data: null };
    spyOn(modificationUserService, 'deleteUserAsync').and.returnValue(Promise.resolve(mockResponse));

    await component["delete"]();

    expect(modificationUserService.deleteUser).toHaveBeenCalledOnceWith({ name: mockUserName });
    expect(component["check"]).toEqual(mockResponse);
  });
});
