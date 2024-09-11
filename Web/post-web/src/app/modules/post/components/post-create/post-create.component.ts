import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpService } from '../../../../services/http.service';
import { postUrl } from '../../../../urls';
import { IPostItemRequest } from '../../../../models/requests/PostItemRequest';
import { IGeneralResponse } from '../../../../models/reponses/GeneralResponse';
import { IPostCategory } from '../../../../models/enities/PostCategory';
import { Router } from '@angular/router';
import { MatSelectChange } from '@angular/material/select';
import { take } from 'rxjs/internal/operators/take';
import { ManagementService } from '../../services/management.service';
import { HttpErrorResponse } from '@angular/common/http';

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

  public check?: IGeneralResponse<null>
  protected categories?: IGeneralResponse<Array<IPostCategory>>
  
  private selectedCategory!: number | null

  constructor(
    private http: HttpService,
    private managementService: ManagementService,
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
    const post: IPostItemRequest = {
      title: this.postGroup.value.title,
      content: this.postGroup.value.content,
      categoryId: this.selectedCategory
    }
    
    this.managementService.addPost(post)
      .pipe(
        take(1),
      )
      .subscribe({
        next: (response: IGeneralResponse<number>) => {
          if (response.data) {
            this.router.navigateByUrl('user/dashboard', { replaceUrl: true });
          }
        },
        error: (err: HttpErrorResponse) => {
          this.check = err.error
        }
      });
  }
}
