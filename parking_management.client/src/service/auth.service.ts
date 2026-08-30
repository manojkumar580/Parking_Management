import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginRequest, LoginResponse, RegisterRequest, RegisterResponse } from '../models/auth.model';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly apiUrl = 'https://localhost:7295/api/Auth';
  private readonly tokenKey = 'parking_auth_token';
  private readonly userKey = 'parking_user';

  constructor(private http: HttpClient) { }

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(
        `${this.apiUrl}/login`,
        request
      )
      .pipe(
        tap((response:any) => {
          localStorage.setItem(
            this.tokenKey,
            response.token
          );

          localStorage.setItem(
            this.userKey,
            JSON.stringify({
              userId: response.userId,
              name: response.name,
              email: response.email
            })
          );
        })
      );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getUser(): {
    userId: string;
    name: string;
    email: string;
  } | null {
    const user = localStorage.getItem(this.userKey);

    if (!user) {
      return null;
    }

    try {
      return JSON.parse(user);
    } catch (error) {
      console.error('Unable to read stored user information.', error);
      return null;
    }
  }

  register(request: RegisterRequest): Observable<RegisterResponse> {
    try {
      return this.http.post<RegisterResponse>(
        `${this.apiUrl}/register`,
        request
      );
    } catch (error) {
      console.error('Error while registering user:', error);
      throw error;
    }
  }
}
