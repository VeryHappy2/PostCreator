import { HttpClientTestingModule } from "@angular/common/http/testing";
import { CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";
import { ComponentFixture, TestBed } from "@angular/core/testing";
import { DashboardComponent } from "./dashboard.component";
import { TokenStorageService } from "../../../../services/auth/token-storage.service";
import { Subscription } from "rxjs/internal/Subscription";
import { IUser } from "../../../../models/User";
import { of } from "rxjs/internal/observable/of";

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;
  let tokenStorageService: TokenStorageService

  beforeEach(async () => {
    const tokenStorageSpy = jasmine.createSpyObj('TokenStorageService', ['user$']);

    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [
        DashboardComponent,
      ],
      providers: [TokenStorageService],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    })
    .compileComponents();
    
    tokenStorageService = TestBed.inject(TokenStorageService);
    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should subscribe to user$ on initialization', () => {
    const mockUser: IUser = {
      id: "1",
      role: "user",
      name: "name"
    };
    
    tokenStorageService.user$ = of(mockUser);

    fixture.detectChanges();
    component.ngOnInit()

    expect(component['user']).toEqual(mockUser);
  });

  it('should unsubscribe from user$ on destroy', () => {
    const mockUser = { id: "1", name: 'Test User', role: "user" };
    tokenStorageService.user$ = of(mockUser);

    fixture.detectChanges();
    component.ngOnDestroy();

    expect(component['userSub']?.closed).toBe(true);
  });
});
