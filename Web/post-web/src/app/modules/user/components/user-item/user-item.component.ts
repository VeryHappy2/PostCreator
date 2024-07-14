import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { IUser } from '../../../../models/User';
import { HttpService } from '../../../../services/http.service';

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrl: './user-item.component.scss'
})
export class UserItemComponent implements OnInit {
  public user?: IUser
  constructor(
    private route: ActivatedRoute,
    private http: HttpService) { }

  ngOnInit(): void {
    this.route.params.subscribe((value: Params) => {
      let userName = value['name']

    })
  }

  // private loadUser() {
  //   this.http.
  // }
}
