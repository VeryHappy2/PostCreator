import { Component, OnInit } from '@angular/core';
import { ErrorService } from '../../services/error/error.service';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrl: './error.component.scss'
})
export class ErrorComponent implements OnInit {
  protected error?: string;

  constructor(
    private errorService: ErrorService) {}

  ngOnInit(): void {
    this.error = this.errorService.error
  }
}
