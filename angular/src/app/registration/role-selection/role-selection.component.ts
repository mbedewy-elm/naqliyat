import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService, LocalizationModule } from '@abp/ng.core';
import { UserService } from '../../proxy/users/user.service';

@Component({
  selector: 'app-role-selection',
  standalone: true,
  imports: [CommonModule, LocalizationModule],
  templateUrl: './role-selection.component.html',
  styleUrls: ['./role-selection.component.scss']
})
export class RoleSelectionComponent {
  private router = inject(Router);
  private authService = inject(AuthService);
  private userService = inject(UserService);

  isLoading = false;
  error = '';

  roles = [
    {
      id: 'requester',
      title: 'Service Requester',
      description: 'I need to ship goods and want to find trucks and drivers for my shipments.',
      icon: 'fa-box',
      color: '#3b82f6'
    },
    {
      id: 'owner',
      title: 'Truck Owner',
      description: 'I own trucks and want to offer transportation services to requesters.',
      icon: 'fa-truck',
      color: '#f59e0b'
    },
    {
      id: 'driver',
      title: 'Driver',
      description: 'I am a professional driver looking for trips to transport goods.',
      icon: 'fa-id-card',
      color: '#10b981'
    }
  ];

  selectRole(roleId: string): void {
    if (roleId === 'requester') {
      this.registerAsRequester();
    } else if (roleId === 'owner') {
      this.router.navigate(['/registration/owner']);
    } else if (roleId === 'driver') {
      this.router.navigate(['/registration/driver']);
    }
  }

  private registerAsRequester(): void {
    this.isLoading = true;
    this.error = '';

    this.userService.registerAsRequester().subscribe({
      next: (result) => {
        this.isLoading = false;
        if (result.success) {
          this.logoutAndRedirect();
        } else {
          this.error = result.message || 'Registration failed. Please try again.';
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.error = 'An error occurred during registration. Please try again.';
        console.error('Registration error:', err);
      }
    });
  }

  private logoutAndRedirect(): void {
    this.authService.logout().subscribe({
      next: () => {
        // Redirect to home after logout
        window.location.href = '/';
      },
      error: () => {
        window.location.href = '/';
      }
    });
  }
}
