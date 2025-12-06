import { Component, inject, OnInit } from '@angular/core';
import { ConfigStateService, LocalizationModule } from '@abp/ng.core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TruckService } from '../../proxy/trucks/truck.service';
import { CreateTruckDto, DriverDto } from '../../proxy/trucks/models';
import { TruckTypes, truckTypesOptions } from '../../proxy/enums/truck-types.enum';
import { TemperatureRequirements, temperatureRequirementsOptions } from '../../proxy/enums/temperature-requirements.enum';

@Component({
  selector: 'app-add-truck',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, LocalizationModule],
  templateUrl: './add-truck.component.html',
  styleUrls: ['./add-truck.component.scss']
})
export class AddTruckComponent implements OnInit {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private truckService = inject(TruckService);
  private configState = inject(ConfigStateService);

  truckForm!: FormGroup;
  isSubmitting = false;
  errorMessage = '';

  truckTypeOptions = truckTypesOptions;
  temperatureOptions = temperatureRequirementsOptions;

  // Generate year options (last 30 years)
  yearOptions: string[] = [];

  // Drivers list
  drivers: DriverDto[] = [];
  isLoadingDrivers = false;

  ngOnInit(): void {
    this.initForm();
    this.generateYearOptions();
    this.loadDrivers();
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
      maxLoad: [null, [Validators.min(0)]],
      driverId: [null]
    });
  }

  private loadDrivers(): void {
    this.isLoadingDrivers = true;
    this.truckService.getDriverList().subscribe({
      next: (result) => {
        this.drivers = result;
        this.isLoadingDrivers = false;
      },
      error: () => {
        this.isLoadingDrivers = false;
      }
    });
  }

  private generateYearOptions(): void {
    const currentYear = new Date().getFullYear();
    for (let year = currentYear; year >= currentYear - 30; year--) {
      this.yearOptions.push(year.toString());
    }
  }

  onSubmit(): void {
    if (this.truckForm.invalid) {
      this.truckForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    const formValue = this.truckForm.value;
    const truckData: CreateTruckDto = {
      make: formValue.make,
      model: formValue.model,
      year: formValue.year,
      licenseNumber: formValue.licenseNumber,
      registrationNumber: formValue.registrationNumber || undefined,
      truckTypeId: formValue.truckTypeId,
      temperatureRequirement: formValue.temperatureRequirement,
      maxLoad: formValue.maxLoad || undefined,
      driverId: formValue.driverId || undefined,
      ownerId: this.configState.getDeep('currentUser.id'),
      pictureIds: []
    };

    this.truckService.create(truckData).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/truck/list']);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err?.error?.error?.message || 'Failed to add truck. Please try again.';
      }
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.truckForm.get(fieldName);
    return field ? field.invalid && field.touched : false;
  }
}
