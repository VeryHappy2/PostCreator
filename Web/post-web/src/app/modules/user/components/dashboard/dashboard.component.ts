import { Component, OnInit } from '@angular/core';
import { TokenStorageService } from '../../../../services/auth/token-storage.service';
import { IUser } from '../../../../models/User';
import { HttpErrorResponse } from '@angular/common/http';
import { IPostItem } from '../../../../models/enities/PostItem';
import { IGeneralResponse } from '../../../../models/reponses/GeneralResponse';
import { postUrl } from '../../../../urls';
import { HttpService } from '../../../../services/http.service';
import { IByIdRequest } from '../../../../models/requests/ByIdRequest';

@Component({
  selector: 'app-main',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  public user?: IUser
  public posts?: IGeneralResponse<Array<IPostItem>>
  constructor(
    private tokenStorage: TokenStorageService,
    private http: HttpService
  ) { }

  ngOnInit(): void {
    this.user = {
      name: this.tokenStorage.getUsername(),
      role: this.tokenStorage.getAuthorities(),
      token: null
    }

    let userId: IByIdRequest<string | null> = {
      id: this.tokenStorage.getId()
    }

    this.http.post<IByIdRequest<string | null>, IGeneralResponse<Array<IPostItem>>>(`${postUrl}/postbff/getpostsbyuserid`, userId).subscribe(
      (response: IGeneralResponse<Array<IPostItem>>) => this.posts = response,
      (error: HttpErrorResponse) => this.posts = error.error)
    console.log(JSON.stringify)
  }
}


