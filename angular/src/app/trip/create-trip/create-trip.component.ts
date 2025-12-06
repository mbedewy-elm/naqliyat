import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { LocalizationModule } from '@abp/ng.core';
import { TripService } from '../../proxy/trips/trip.service';
import { CreateTripDto } from '../../proxy/trips/models';
import { TruckTypes, truckTypesOptions } from '../../proxy/enums/truck-types.enum';

@Component({
  selector: 'app-create-trip',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, LocalizationModule],
  templateUrl: './create-trip.component.html',
  styleUrls: ['./create-trip.component.scss']
})
export class CreateTripComponent implements OnInit {
  private fb = inject(FormBuilder);
  private tripService = inject(TripService);
  private router = inject(Router);

  tripForm!: FormGroup;
  truckTypes = truckTypesOptions;
  isSubmitting = false;
  errorMessage = '';

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.tripForm = this.fb.group({
      fromLocation: ['', [Validators.required, Validators.minLength(3)]],
      toLocation: ['', [Validators.required, Validators.minLength(3)]],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      goodsWeight: [null, [Validators.required, Validators.min(1)]],
      goodsDimensions: [''],
      truckTypeId: [TruckTypes.Unknown, Validators.required],
      goodsType: ['', Validators.required],
      notes: [''],
      pictureIds: [[]]
    });
  }

  onSubmit(): void {
    if (this.tripForm.invalid) {
      this.tripForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    const formValue = this.tripForm.value;
    const tripData: CreateTripDto = {
      fromLocation: formValue.fromLocation,
      toLocation: formValue.toLocation,
      startDate: new Date(formValue.startDate).toISOString(),
      endDate: new Date(formValue.endDate).toISOString(),
      goodsWeight: formValue.goodsWeight,
      goodsDimensions: formValue.goodsDimensions,
      truckTypeId: formValue.truckTypeId,
      goodsType: formValue.goodsType,
      notes: formValue.notes,
      pictureIds: formValue.pictureIds || []
    };

    this.tripService.create(tripData).subscribe({
      next: (result) => {
        this.isSubmitting = false;
        this.router.navigate(['/trip', result.id]);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err?.error?.error?.message || 'Failed to create trip. Please try again.';
        console.error('Error creating trip:', err);
      }
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.tripForm.get(fieldName);
    return field ? field.invalid && field.touched : false;
  }

  getFieldError(fieldName: string): string {
    const field = this.tripForm.get(fieldName);
    if (!field || !field.errors) return '';

    if (field.errors['required']) return `${fieldName} is required`;
    if (field.errors['minlength']) return `${fieldName} must be at least ${field.errors['minlength'].requiredLength} characters`;
    if (field.errors['min']) return `${fieldName} must be greater than ${field.errors['min'].min}`;

    return 'Invalid value';
  }
}
