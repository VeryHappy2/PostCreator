import { IJwtClaims } from "../JwtClaimsResponse"

export interface ILogInResponse {
    flag?: boolean
    message?: string
    data?: IJwtClaims
}