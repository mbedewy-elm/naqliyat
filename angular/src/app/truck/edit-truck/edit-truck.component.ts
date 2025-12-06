import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { LocalizationModule } from '@abp/ng.core';
import { TruckService } from '../../proxy/trucks/truck.service';
import { TruckDto } from '../../proxy/trucks/models';
import { TruckTypes, truckTypesOptions } from '../../proxy/enums/truck-types.enum';
import { TemperatureRequirements, temperatureRequirementsOptions } from '../../proxy/enums/temperature-requirements.enum';

@Component({
  selector: 'app-edit-truck',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, LocalizationModule],
  templateUrl: './edit-truck.component.html',
  styleUrls: ['./edit-truck.component.scss']
})
export class EditTruckComponent implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private truckService = inject(TruckService);

  truck: TruckDto | null = null;
  truckForm!: FormGroup;
  isLoading = true;
  isSubmitting = false;
  errorMessage = '';
  truckId = '';

  truckTypeOptions = truckTypesOptions;
  temperatureOptions = temperatureRequirementsOptions;
  yearOptions: string[] = [];

  ngOnInit(): void {
    this.truckId = this.route.snapshot.paramMap.get('id') || '';
    this.initForm();
    this.generateYearOptions();
    if (this.truckId) {
      this.loadTruck();
    }
  }

  private initForm(): void {
    this.truckForm = this.fb.group({
      make: ['', Validators.required],
      model: ['', Validators.required],
      year: ['', Validators.required],
      licenseNumber: ['', Validators.required],
      registrationNumber: [''],
      truckTypeId: [TruckTypes.Unknown, Validators.required],
      temperatureRequirement: [TemperatureRequirements.Ambient],
      maxLoad: [null, [Validators.min(0)]]
    });
  }

  private generateYearOptions(): void {
    const currentYear = new Date().getFullYear();
    for (let year = currentYear; year >= currentYear - 30; year--) {
      this.yearOptions.push(year.toString());
    }
  }

  loadTruck(): void {
    this.isLoading = true;
    this.truckService.getList().subscribe({
      next: (result) => {
        this.truck = result.find(t => t.id === this.truckId) || null;
        if (this.truck) {
          this.truckForm.patchValue({
            make: this.truck.make,
            model: this.truck.model,
            year: this.truck.year,
            licenseNumber: this.truck.licenseNumber,
            registrationNumber: this.truck.registrationNumber,
            truckTypeId: this.truck.truckTypeId,
            temperatureRequirement: this.truck.temperatureRequirement,
            maxLoad: this.truck.maxLoad
          });
        } else {
          this.errorMessage = 'Truck not found';
        }
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.error?.message || 'Failed to load truck';
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.truckForm.invalid) {
      this.truckForm.markAllAsTouched();
      return;
    }

    // Note: The API only has create and getList methods
    // Edit functionality would require an update endpoint
    this.errorMessage = 'Edit functionality is not yet available in the API';
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.truckForm.get(fieldName);
    return field ? field.invalid && field.touched : false;
  }
}
