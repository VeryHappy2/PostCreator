import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { GeneralResponse } from '../../../../../general-models/reponses/GeneralResponse';
import { PostItem } from '../../../../../general-models/enities/PostItem';
import { HttpService } from '../../../../../services/http.service';
import { post } from '../../../../../urls';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrl: './posts.component.scss'
})
export class PostsComponent implements OnInit {
  posts?: Observable<GeneralResponse<Array<PostItem>>>

  constructor(private http: HttpService) { }

  ngOnInit(): void {
    this.posts = this.http.get<GeneralResponse<Array<PostItem>>>(`${post}/postbff/getpostsbyownuserid`)
  }
}
