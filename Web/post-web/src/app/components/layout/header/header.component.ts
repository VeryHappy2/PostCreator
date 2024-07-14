import { Component, OnInit } from '@angular/core';
import { TokenStorageService } from '../../../services/auth/token-storage.service';
import { HttpService } from '../../../services/http.service';
import { Router } from '@angular/router';
import { IUser } from '../../../models/User';
import { AuthService } from '../../../services/auth/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent { } 
