import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { By } from '@angular/platform-browser';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { of } from 'rxjs/internal/observable/of';
import { ILogInResponse } from '../../../../models/reponses/LogInResponse';
import { throwError } from 'rxjs/internal/observable/throwError';
import { HttpErrorResponse } from '@angular/common/http';
import { IUserLoginRequest } from '../../../../models/requests/user/UserLoginRequest';


describe('LoginComponent', () => {
  let component: any;
  let fixture: ComponentFixture<LoginComponent>;
  let auth: AuthService

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, ReactiveFormsModule],
      declarations: [
        LoginComponent,
      ],
      providers: [
        AuthService,
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    })
    .compileComponents();
    
    auth = TestBed.inject(AuthService);
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance as any;

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it("should click on the button log in and call the login() method", () => {
    const logInButton = fixture.debugElement.query(By.css('button[mat-flat-button]')).nativeElement as HTMLElement
    spyOn(component, "logIn").and.callThrough()

    logInButton.click();

    expect(component.logIn).toHaveBeenCalled()
  });

  it("should to log in the user", () => {
    const resp: ILogInResponse = {
      flag: true,
      message: 'message',
      data: {
        id: "21",
        userName: "name",
        role: "User"
      }
    }
    component.userGroup.setValue({ userName: 'test@example.com', password: 'password123' });
    const request = {
      password: component.userGroup.value.password,
      email: component.userGroup.value.userName
    };
    

    spyOn(auth, "login").and.returnValue(of(resp))

    component["logIn"]();

    expect(auth.login).toHaveBeenCalledOnceWith(request)
  });

  it('should set `check` to error response on login failure', () => {
    const errorResponse = new HttpErrorResponse({
      error: { message: 'Invalid credentials' },
      status: 401
    });
    component.userGroup.setValue({ userName: 'test@example.com', password: 'password123' });
    const request: IUserLoginRequest = {
      email: "test@example.com",
      password: "password123"
    }

    spyOn(auth, "login").and.returnValue(throwError(() => errorResponse));

    component["logIn"]();

    expect(auth.login).toHaveBeenCalledOnceWith(request)
    expect(component.check).toEqual(errorResponse.error);
  });
});
