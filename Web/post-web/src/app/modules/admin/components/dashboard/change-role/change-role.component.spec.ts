import { HttpClientTestingModule } from "@angular/common/http/testing";
import { CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";
import { ComponentFixture, TestBed } from "@angular/core/testing";
import { ChangeRoleComponent } from "./change-role.component";
import { HttpService } from "../../../../../services/http.service";
import { ModificationUserService } from "../../../services/modification-user.service";
import { IGeneralResponse } from "../../../../../models/responses/GeneralResponse";
import { of } from "rxjs";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { identityServerUrl } from "../../../../../../env/urls";
import { By } from "@angular/platform-browser";
import { MatSelectModule } from "@angular/material/select";
import { IChangeRoleRequest } from "../../../../../models/requests/user/ChangeRoleRequest";
import { ResponseErrorHandlerService } from "../../../../../services/error/response-error-handler.service";

describe('ChangeRoleComponent', () => {
  let component: ChangeRoleComponent;
  let fixture: ComponentFixture<ChangeRoleComponent>;
  let http: HttpService
  let modificationService: ModificationUserService
  const childComponent = jasmine.createSpyObj("SearchUserAdminComponent", [
    "fetchUserNameData"
  ])

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        MatSelectModule,
        BrowserAnimationsModule,
      ],
      declarations: [
        ChangeRoleComponent,
        ResponseErrorHandlerService
      ],
      providers: [
        HttpService,
        ModificationUserService
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    })
    .compileComponents();
    
    modificationService = TestBed.inject(ModificationUserService)
    http = TestBed.inject(HttpService);
    fixture = TestBed.createComponent(ChangeRoleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it("should have to change the role", () => {
    const response: IGeneralResponse<null>  = {
      flag: true, 
      message: "message",
      data: null
    };
    
    component["selectedRole"] = "user";
  
    const mockUserName = "data";
    const expectedRequest: IChangeRoleRequest = {
      role: component['selectedRole'],
      userName: mockUserName
    };
    component.searcherUserAdmin = childComponent

    
    spyOn(modificationService, "changeRole").and.returnValue(of(response));
    fixture.detectChanges();

    component.changeRole();

    expect(component.searcherUserAdmin.fetchUserNameData).toHaveBeenCalled()
    expect(component["selectedRole"]).toEqual("user");    
  
    expect(modificationService.changeRole).toHaveBeenCalledWith(expectedRequest);
    expect(component["check"]).toEqual(response)
  });
  

  it("should get the roles", (done: DoneFn) => {
    const resp: IGeneralResponse<Array<string>> = {
      flag: true,
      data: [
        "User"
      ],
      message: "message"
    }

    spyOn(http, "get").and.returnValue(of(resp))

    component['ngOnInit']()

    component.roles$!.subscribe((result) => {
      expect(result).toEqual(resp);
      done()
    });
    expect(http.get).toHaveBeenCalledOnceWith(`${identityServerUrl}/account/getroles`)
  })

  it('should call onSelectChange with the correct value when a selection is made', () => {
    const resp: IGeneralResponse<Array<string>> = {
      flag: true,
      data: [
        "User"
      ],
      message: "message"
    }

    component.roles$ = of(resp)
    
    spyOn(component, 'onSelectChange').and.callThrough();

    fixture.detectChanges();

    const matSelect = fixture.debugElement.query(By.css("mat-select"));
    matSelect.triggerEventHandler('selectionChange', { value: 'admin' });

    expect(component['onSelectChange']).toHaveBeenCalled();
    expect(component['onSelectChange']).toHaveBeenCalledWith(jasmine.objectContaining({ value: 'admin' }));
    expect(component['selectedRole']).toEqual('admin');
  });
});
