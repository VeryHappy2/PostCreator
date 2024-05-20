import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from '../../../../../services/http.service';
import { postUrl } from '../../../../../urls';
import { PostItem } from '../../../../../models/enities/PostItem';
import { GeneralResponse } from '../../../../../models/reponses/GeneralResponse';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrl: './posts.component.scss'
})
export class PostsComponent implements OnInit {
  posts$?: Observable<GeneralResponse<Array<PostItem>>>

  constructor(private http: HttpService) { }

  ngOnInit(): void {
    this.posts$ = this.http.get<GeneralResponse<Array<PostItem>>>(`${postUrl}/postbff/getpostsbyownuserid`)
  }
}
