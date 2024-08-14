import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  public error?: string;
  
  constructor() { }

  saveError(error: string) {
    this.error = error
  }
}
