import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpService } from '../../../../services/http.service';
import { postUrl } from '../../../../urls';
import { IPostItemRequest } from '../../../../models/requests/PostItemRequest';
import { IGeneralResponse } from '../../../../models/reponses/GeneralResponse';
import { IPostCategory } from '../../../../models/enities/PostCategory';
import { Router } from '@angular/router';
import { ResponseErrorHandlerService } from '../../../../services/response-error-handler.service';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { MatSelectChange } from '@angular/material/select';
import { TokenStorageService } from '../../../../services/auth/token-storage.service';

@Component({
  selector: 'app-post-create',
  templateUrl: './post-create.component.html',
  styleUrl: './post-create.component.scss'
})
export class PostCreateComponent implements OnInit {
  public postGroup: FormGroup = new FormGroup ({
    title: new FormControl("", [Validators.required, Validators.maxLength(50)]),
    content: new FormControl("", [Validators.required, Validators.maxLength(3000)]),
  })

  public errorMessage?: IGeneralResponse<string>
  public categories?: IGeneralResponse<Array<IPostCategory>>
  private selectedCategory!: number | null

  constructor(
    private http: HttpService,
    private router: Router,
    private tokenStorage: TokenStorageService) { }

  ngOnInit(): void {
    this.http.get<IGeneralResponse<Array<IPostCategory>>>(`${postUrl}/postbff/getpostcategories`)
      .subscribe(response => {
        this.categories = response
      },
    (error: IGeneralResponse<Array<IPostCategory>>) => console.log(JSON.stringify(error)))
  }

  public onSelectChange(eventSelect: MatSelectChange): void {
    this.selectedCategory = eventSelect.value
  }

  public createPost() {
    let post: IPostItemRequest = {
      title: this.postGroup.value.title,
      content: this.postGroup.value.content,
      categoryId: this.selectedCategory
    }
    
    this.http.post<IPostItemRequest, IGeneralResponse<number>>(`${postUrl}/postitem/add`, post).subscribe(
    (response: IGeneralResponse<number>) => {
      if (response.flag && response.data) {
        this.router.navigateByUrl('user/dashboard', {replaceUrl: true})
      }
    },  
    (error: HttpErrorResponse) => {
      this.errorMessage = error.error;    
    })
  }
}
