import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService, LocalizationModule } from '@abp/ng.core';
import { UserService } from '../../proxy/users/user.service';
import { RegisterAsOwnerDto } from '../../proxy/users/models';

@Component({
  selector: 'app-register-owner',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, LocalizationModule],
  templateUrl: './register-owner.component.html',
  styleUrls: ['./register-owner.component.scss']
})
export class RegisterOwnerComponent implements OnInit {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private authService = inject(AuthService);
  private userService = inject(UserService);

  form!: FormGroup;
  isLoading = false;
  error = '';

  // Saudi Arabia nationality ID (you may need to adjust this)
  nationalities = [
    { id: 1, name: 'Saudi Arabia' },
    { id: 2, name: 'United Arab Emirates' },
    { id: 3, name: 'Kuwait' },
    { id: 4, name: 'Qatar' },
    { id: 5, name: 'Bahrain' },
    { id: 6, name: 'Oman' },
    { id: 7, name: 'Egypt' },
    { id: 8, name: 'Jordan' },
    { id: 9, name: 'Other' }
  ];

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      phone: ['', [Validators.required, Validators.pattern(/^[0-9+\-\s]{10,15}$/)]],
      identification: ['', [Validators.required, Validators.minLength(5)]],
      entityNumber: ['', [Validators.required, Validators.minLength(5)]],
      nationalityId: [1, [Validators.required]]
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.error = '';

    const dto: RegisterAsOwnerDto = this.form.value;

    this.userService.registerAsOwner(dto).subscribe({
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
        window.location.href = '/';
      },
      error: () => {
        window.location.href = '/';
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/registration']);
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.form.get(fieldName);
    return field ? field.invalid && field.touched : false;
  }
}
