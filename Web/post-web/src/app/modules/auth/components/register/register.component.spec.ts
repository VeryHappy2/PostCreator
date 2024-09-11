import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RegisterComponent } from './register.component';
import { By } from '@angular/platform-browser';
import { AuthService } from '../../services/auth.service';
import { of } from 'rxjs/internal/observable/of';
import { IGeneralResponse } from '../../../../models/reponses/GeneralResponse';
import { IUserRegisterRequest } from '../../../../models/requests/user/UserRegisterRequest';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { throwError } from 'rxjs/internal/observable/throwError';
import { IUserLoginRequest } from '../../../../models/requests/user/UserLoginRequest';
import { HttpErrorResponse } from '@angular/common/http';


describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;
  let auth: AuthService

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, ReactiveFormsModule],
      declarations: [RegisterComponent],
      providers: [AuthService],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    })
    .compileComponents();
    
    auth = TestBed.inject(AuthService);
    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should click the register button and have been called the register method', () => {
    const registerButton: HTMLElement = fixture.debugElement.query(By.css('button[mat-flat-button]')).nativeElement
    const signUp = spyOn(component, "signUp").and.callThrough()

    registerButton.click();

    expect(signUp).toHaveBeenCalled();
  });

  it("should signed up", () => {
    component.userGroup.setValue({
      name: "name",
      password: "password",
      confirmPassword: "confirmpassword",
      email: "email"
    })
    const resp: IGeneralResponse<null> = {
      flag: true,
      message: "message",
      data: null
    }
    const userRegister: IUserRegisterRequest = {
      name: component.userGroup.value.name!,
      email: component.userGroup.value.email!,
      password: component.userGroup.value.password!,
      confirmPassword: component.userGroup.value.confirmPassword!
    };
    spyOn(auth, "register").and.returnValue(of(resp));

    component.signUp();

    expect(auth.register).toHaveBeenCalledOnceWith(userRegister);
  });
  it('should set `check` to error response on login failure', () => {
    const errorResponse = new HttpErrorResponse({
      error: { message: 'Invalid credentials' },
      status: 401
    });
    component.userGroup.setValue({
      name: "name",
      password: "password",
      confirmPassword: "confirmpassword",
      email: "email"
    })
    const request: IUserRegisterRequest = {
      name: component.userGroup.value.name!,
      email: component.userGroup.value.email!,
      password: component.userGroup.value.password!,
      confirmPassword: component.userGroup.value.confirmPassword!
    };

    spyOn(auth, "register").and.returnValue(throwError(() => errorResponse));

    component["signUp"]();

    expect(auth.register).toHaveBeenCalledOnceWith(request)
    expect(component.check).toEqual(errorResponse.error);
  });
});
