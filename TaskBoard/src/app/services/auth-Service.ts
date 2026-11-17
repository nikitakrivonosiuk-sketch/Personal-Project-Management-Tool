import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { LoginRequestDto, LoginResponseDto, RegisterRequestDto } from "../models/auth-models";
import { environment } from "../../environments/environment";
import { TokenStorageService } from "./token-storage-service";
import { tap } from "rxjs";

@Injectable ({
    providedIn: 'root'
})

export class AuthService {
    private http = inject(HttpClient);
    private tokenStorage = inject(TokenStorageService);
    apiUrl = environment.apiUrl;

    registerUser(email: string, password: string){
        const request: RegisterRequestDto = {
            Email: email,
            Password: password,
        }; 

        return this.http.post(`${this.apiUrl}/auth/register`, request);
    }

    loginUser(email: string, password: string){
        const request: LoginRequestDto = {
            Email: email,
            Password: password,
        }; 

        return this.http.post<LoginResponseDto>(`${this.apiUrl}/auth/login`, request)
        .pipe(
            tap(response => this.tokenStorage.setToken(response.accessToken)),
        );
    }
}