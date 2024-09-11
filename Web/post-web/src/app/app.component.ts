import { Component, OnInit } from '@angular/core';
import { TokenStorageService } from './services/auth/token-storage.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  title = 'post-web';

  constructor(private token: TokenStorageService) { }

  ngOnInit(): void {
    this.token.refreshAccessToken();
  }
}
