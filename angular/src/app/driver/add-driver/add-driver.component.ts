import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { LocalizationModule } from '@abp/ng.core';
import { TruckService } from '../../proxy/trucks/truck.service';
import { CreateDriverDto } from '../../proxy/trucks/models';

@Component({
  selector: 'app-add-driver',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, LocalizationModule],
  templateUrl: './add-driver.component.html',
  styleUrls: ['./add-driver.component.scss']
})
export class AddDriverComponent implements OnInit {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private truckService = inject(TruckService);

  driverForm!: FormGroup;
  isSubmitting = false;
  errorMessage = '';

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.driverForm = this.fb.group({
      name: ['', Validators.required],
      phone: ['', Validators.required],
      identification: ['', Validators.required],
      licenseNumber: ['', Validators.required],
      nationalityId: [1, Validators.required]
    });
  }

  onSubmit(): void {
    if (this.driverForm.invalid) {
      this.driverForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    const formValue = this.driverForm.value;
    const driverData: CreateDriverDto = {
      name: formValue.name,
      phone: formValue.phone,
      identification: formValue.identification,
      licenseNumber: formValue.licenseNumber,
      nationalityId: formValue.nationalityId
    };

    this.truckService.createDriver(driverData).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/driver/list']);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err?.error?.error?.message || 'Failed to add driver. Please try again.';
      }
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.driverForm.get(fieldName);
    return field ? field.invalid && field.touched : false;
  }
}
