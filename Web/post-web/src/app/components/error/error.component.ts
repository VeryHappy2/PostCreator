import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ErrorService } from '../../services/error/error.service';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrl: './error.component.scss'
})
export class ErrorComponent implements OnInit {
  protected error?: string;

  constructor(
    private router: Router,
    private errorService: ErrorService) {}

  ngOnInit(): void {
    this.error = this.errorService.error
  }
}
