import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class JwtService {

  constructor(private jwtHelper: JwtHelperService) { }

  decodeToken<T>(token: string): T | null {
    return this.jwtHelper.decodeToken(token)
  }
}
