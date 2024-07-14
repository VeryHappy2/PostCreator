import { Component, Input, OnInit, input } from '@angular/core';
import { HttpService } from '../../../../../services/http.service';
import { postUrl } from '../../../../../urls';
import { IPostItem } from '../../../../../models/enities/PostItem';
import { IGeneralResponse } from '../../../../../models/reponses/GeneralResponse';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-user-post',
  templateUrl: './user-post.component.html',
  styleUrl: './user-post.component.scss'
})

export class PostsComponent {
  @Input() post?: IPostItem;

  constructor() { }
}
