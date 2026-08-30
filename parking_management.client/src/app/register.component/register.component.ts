import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { RegisterRequest } from '../../models/auth.model';
import { AuthService } from '../../service/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
  standalone: false
})
export class RegisterComponent {

  registerRequest: RegisterRequest = {
    name: '',
    email: '',
    password: ''
  };

  confirmPassword = '';

  errorMessage = '';
  successMessage = '';
  isLoading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  onRegister(): void {
    try {
      this.errorMessage = '';
      this.successMessage = '';

      if (
        !this.registerRequest.name ||
        !this.registerRequest.email ||
        !this.registerRequest.password ||
        !this.confirmPassword
      ) {
        this.errorMessage = 'All fields are required.';
        return;
      }

      if (this.registerRequest.password !== this.confirmPassword) {
        this.errorMessage = 'Passwords do not match.';
        return;
      }

      this.isLoading = true;

      this.authService.register(this.registerRequest).subscribe({
        next: (response) => {
          this.isLoading = false;

          this.successMessage =
            response.message || 'Registration successful.';

          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 1500);
        },
        error: (error) => {
          this.isLoading = false;

          console.error('Registration error:', error);

          this.errorMessage =
            error?.error?.message ??
            'Registration failed. Please try again.';
        }
      });

    } catch (error) {
      this.isLoading = false;

      console.error(
        'Unexpected error during registration:',
        error
      );

      this.errorMessage =
        'An unexpected error occurred. Please try again.';
    }
  }

  goToLogin(): void {
    try {
      this.router.navigate(['/login']);
    } catch (error) {
      console.error('Error navigating to login:', error);
    }
  }
}
