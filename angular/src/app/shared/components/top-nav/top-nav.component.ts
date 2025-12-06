import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService, ConfigStateService } from '@abp/ng.core';

interface NavLink {
  path: string;
  label: string;
  icon: string;
  roles?: string[];
  requiresAuth?: boolean;
}

@Component({
  selector: 'app-top-nav',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './top-nav.component.html',
  styleUrls: ['./top-nav.component.scss']
})
export class TopNavComponent implements OnInit {
  private authService = inject(AuthService);
  private configState = inject(ConfigStateService);
  private router = inject(Router);

  navLinks: NavLink[] = [
    { path: '/', label: 'Home', icon: 'fa-home', requiresAuth: false },
    // Driver links
    { path: '/trip/available', label: 'Available Trips', icon: 'fa-route', roles: ['driver'], requiresAuth: true },
    { path: '/trip/my-trips', label: 'My Trips', icon: 'fa-truck-loading', roles: ['driver'], requiresAuth: true },
    // Service Requester links
    { path: '/trip/create', label: 'Create Trip', icon: 'fa-plus-circle', roles: ['requester', 'admin'], requiresAuth: true },
    { path: '/trip/list', label: 'My Requests', icon: 'fa-list-alt', roles: ['requester', 'admin'], requiresAuth: true },
    // Truck Owner links
    { path: '/truck/list', label: 'Trucks', icon: 'fa-truck', roles: ['owner', 'admin'], requiresAuth: true },
    { path: '/truck/add', label: 'Add Truck', icon: 'fa-plus', roles: ['owner', 'admin'], requiresAuth: true },
    { path: '/driver/list', label: 'Drivers', icon: 'fa-id-card', roles: ['owner', 'admin'], requiresAuth: true },
    { path: '/driver/add', label: 'Add Driver', icon: 'fa-user-plus', roles: ['owner', 'admin'], requiresAuth: true },
  ];

  ngOnInit(): void {
    this.checkUserRoles();
  }

  private checkUserRoles(): void {
    // Only check if user is authenticated
    if (!this.authService.isAuthenticated) {
      return;
    }

    // Skip check if already on registration page
    if (this.router.url.startsWith('/registration')) {
      return;
    }

    // Check if user has any roles
    const currentUser = this.configState.getOne('currentUser');
    const roles = currentUser?.roles || [];

    // If user has no roles, redirect to registration
    if (roles.length === 0) {
      this.router.navigate(['/registration']);
    }
  }

  get isAuthenticated(): boolean {
    return this.authService.isAuthenticated;
  }

  get hasRoles(): boolean {
    return this.currentUserRoles.length > 0;
  }

  get currentUserRoles(): string[] {
    const roles = this.configState.getOne('currentUser')?.roles || [];
    return roles.map((r: string) => r.toLowerCase());
  }

  get currentUserName(): string {
    return this.configState.getOne('currentUser')?.userName || '';
  }

  isLinkVisible(link: NavLink): boolean {
    // If link doesn't require auth, show it
    if (!link.requiresAuth) {
      return true;
    }

    // If not authenticated, hide auth-required links
    if (!this.isAuthenticated) {
      return false;
    }

    // If no specific roles required, show to all authenticated users
    if (!link.roles || link.roles.length === 0) {
      return true;
    }

    // Check if user has any of the required roles
    return link.roles.some(role => this.currentUserRoles.includes(role));
  }

  login(): void {
    this.authService.navigateToLogin();
  }

  logout(): void {
    this.authService.logout().subscribe();
  }
}
