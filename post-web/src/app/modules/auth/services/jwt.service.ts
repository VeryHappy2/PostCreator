import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class JwtService {
  private jwtHelper: JwtHelperService = new JwtHelperService()
  
  constructor() { }

  decodeToken<T>(token: string): T | null {
    return this.jwtHelper.decodeToken(token)
  }
}
