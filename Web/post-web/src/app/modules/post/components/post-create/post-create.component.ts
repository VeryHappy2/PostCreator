import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpService } from '../../../../services/http.service';
import { postUrl } from '../../../../urls';
import { IPostItemRequest } from '../../../../models/requests/PostItemRequest';
import { IGeneralResponse } from '../../../../models/reponses/GeneralResponse';
import { IPostCategory } from '../../../../models/enities/PostCategory';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { MatSelectChange } from '@angular/material/select';
import { take } from 'rxjs/internal/operators/take';

@Component({
  selector: 'app-post-create',
  templateUrl: './post-create.component.html',
  styleUrl: './post-create.component.scss'
})
export class PostCreateComponent implements OnInit {
  protected postGroup: FormGroup = new FormGroup ({
    title: new FormControl("", [Validators.required, Validators.maxLength(50)]),
    content: new FormControl("", [Validators.required, Validators.maxLength(3000)]),
  })

  protected errorMessage?: IGeneralResponse<string>
  protected categories?: IGeneralResponse<Array<IPostCategory>>
  
  private selectedCategory!: number | null

  constructor(
    private http: HttpService,
    private router: Router) { }

  ngOnInit(): void {
    this.http.get<IGeneralResponse<Array<IPostCategory>>>(`${postUrl}/postbff/getpostcategories`)
      .pipe(take(1))
      .subscribe(response => {
        this.categories = response
      })
  }

  protected onSelectChange(eventSelect: MatSelectChange): void {
    this.selectedCategory = eventSelect.value
  }

  protected createPost(): void {
    let post: IPostItemRequest = {
      title: this.postGroup.value.title,
      content: this.postGroup.value.content,
      categoryId: this.selectedCategory
    }
    
    this.http.post<IPostItemRequest, IGeneralResponse<number>>(`${postUrl}/postitem/add`, post).subscribe(
    (response: IGeneralResponse<number>) => {
      if (response.flag && response.data) {
        this.router.navigateByUrl('user/dashboard', { replaceUrl: true })
      }
    },  
    (error: HttpErrorResponse) => {
      this.errorMessage = error.error;    
    })
  }
}
