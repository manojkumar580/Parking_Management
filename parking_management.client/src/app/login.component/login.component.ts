import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../../service/auth.service';
import { LoginRequest } from '../../models/auth.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  standalone:false
})
export class LoginComponent {

  loginRequest: LoginRequest = {
    email: '',
    password: ''
  };

  errorMessage = '';
  isLoading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  onLogin(): void {
    try {
      this.errorMessage = '';

      if (!this.loginRequest.email || !this.loginRequest.password) {
        this.errorMessage = 'Email and password are required.';
        return;
      }

      this.isLoading = true;

      this.authService.login(this.loginRequest).subscribe({
        next: (response) => {
          console.log('LOGIN SUCCESS:', response);

          this.isLoading = false;

          this.router.navigate(['/dashboard']);
        },
        error: (error) => {
          console.error('LOGIN ERROR:', error);

          this.isLoading = false;

          this.errorMessage =
            error?.error?.message ??
            'Login failed. Please check your credentials.';
        }
      });
    } catch (error) {
      this.isLoading = false;
      console.error('Unexpected error during login:', error);
      this.errorMessage =
        'An unexpected error occurred. Please try again.';
    }
  }

  goToRegister(): void {
    try {
      this.router.navigate(['/register']);
    } catch (error) {
      console.error('Error navigating to register:', error);
    }
  }
}
