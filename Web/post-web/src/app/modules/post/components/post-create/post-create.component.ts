import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpService } from '../../../../services/http.service';
import { postUrl } from '../../../../urls';
import { PostItemRequest } from '../../../../models/requests/PostItemRequest';
import { GeneralResponse } from '../../../../models/reponses/GeneralResponse';
import { PostCategory } from '../../../../models/enities/PostCategory';
import { Router } from '@angular/router';

@Component({
  selector: 'app-post-create',
  templateUrl: './post-create.component.html',
  styleUrl: './post-create.component.scss'
})
export class PostCreateComponent implements OnInit {
  public postGroup: FormGroup = new FormGroup ({
    title: new FormControl("", [Validators.required, Validators.maxLength(50)]),
    categoryId: new FormControl("", [Validators.required]),
    content: new FormControl("", [Validators.required]),
  })
  public errorMessage?: string
  public categories?: GeneralResponse<Array<PostCategory>>
  constructor(
    private http: HttpService,
    private router: Router) { }

  ngOnInit(): void {
    this.http.get<GeneralResponse<Array<PostCategory>>>(`${postUrl}/postbff/getpostcategories`)
      .subscribe(response => {
        this.categories = response
      },
    (error: GeneralResponse<Array<PostCategory>>) => console.log(JSON.stringify(error)))
  }

  public createPost() {
    let post: PostItemRequest = {
      title: this.postGroup.value.title,
      content: this.postGroup.value.content,
      categoryId: this.postGroup.value.categoryId,
    }

    this.http.post<PostItemRequest, GeneralResponse<number>>(`${postUrl}/postitem/add`, post).subscribe(
      (response: GeneralResponse<number>) => {
        if (response.flag && response.data) {
          this.router.navigateByUrl('post/post-create', {replaceUrl: true})
        }
      },
      (error: GeneralResponse<number>) => {
        this.errorMessage = error.message;    
      }
    )
  }
}
