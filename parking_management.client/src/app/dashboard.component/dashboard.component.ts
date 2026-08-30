import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
  standalone: false
})
export class DashboardComponent implements OnInit {

  userName = '';
  userEmail = '';

  constructor(
    private router: Router
  ) { }

  ngOnInit(): void {

    try {

      const userJson =
        localStorage.getItem('parking_user');

      if (!userJson) {

        this.router.navigate(['/login']);

        return;
      }

      const user =
        JSON.parse(userJson);

      this.userName =
        user.name ?? '';

      this.userEmail =
        user.email ?? '';

    } catch (error) {

      console.error(
        'Error loading dashboard:',
        error
      );

      localStorage.removeItem(
        'parking_auth_token'
      );

      localStorage.removeItem(
        'parking_user'
      );

      this.router.navigate(['/login']);
    }
  }

}
