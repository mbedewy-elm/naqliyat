import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ConfigStateService, LocalizationModule } from '@abp/ng.core';
import { TripService } from '../../proxy/trips/trip.service';
import { TruckService } from '../../proxy/trucks/truck.service';
import { TripDto, CreateBidDto } from '../../proxy/trips/models';
import { TruckDto } from '../../proxy/trucks/models';
import { TruckTypes } from '../../proxy/enums/truck-types.enum';

@Component({
  selector: 'app-place-bid',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, LocalizationModule],
  templateUrl: './place-bid.component.html',
  styleUrls: ['./place-bid.component.scss']
})
export class PlaceBidComponent implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private tripService = inject(TripService);
  private truckService = inject(TruckService);
  private configState = inject(ConfigStateService);

  trip: TripDto | null = null;
  bidForm!: FormGroup;
  isLoading = true;
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';
  tripId = '';

  // Driver's truck
  driverTruck: TruckDto | null = null;
  isLoadingTruck = false;

  TruckTypes = TruckTypes;

  ngOnInit(): void {
    this.tripId = this.route.snapshot.paramMap.get('id') || '';
    this.initForm();
    if (this.tripId) {
      this.loadTrip();
    }
    this.loadDriverTruck();
  }

  get currentUserId(): string {
    return this.configState.getDeep('currentUser.id') || '';
  }

  loadDriverTruck(): void {
    this.isLoadingTruck = true;
    const userId = this.currentUserId;
    console.log('PlaceBid - Current User ID:', userId);
    
    if (!userId) {
      console.log('PlaceBid - No user ID found');
      this.isLoadingTruck = false;
      return;
    }

    // First get driver by current user id
    this.truckService.getDriverByUserId(userId).subscribe({
      next: (driver) => {
        console.log('PlaceBid - Driver found:', driver);
        if (driver?.id) {
          // Then get truck by driver id
          this.truckService.getTruckByDriverId(driver.id).subscribe({
            next: (truck) => {
              console.log('PlaceBid - Truck found:', truck);
              this.driverTruck = truck;
              this.isLoadingTruck = false;
            },
            error: (err) => {
              console.error('PlaceBid - Error getting truck:', err);
              this.driverTruck = null;
              this.isLoadingTruck = false;
            }
          });
        } else {
          console.log('PlaceBid - No driver ID in response');
          this.driverTruck = null;
          this.isLoadingTruck = false;
        }
      },
      error: (err) => {
        console.error('PlaceBid - Error getting driver:', err);
        this.driverTruck = null;
        this.isLoadingTruck = false;
      }
    });
  }

  private initForm(): void {
    this.bidForm = this.fb.group({
      price: [null, [Validators.required, Validators.min(1)]],
      arrivalDate: ['', Validators.required],
      truckId: [''] // Optional - driver can select from their trucks
    });
  }

  loadTrip(): void {
    this.isLoading = true;
    this.tripService.getById(this.tripId).subscribe({
      next: (result) => {
        this.trip = result;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.error?.message || 'Failed to load trip details';
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.bidForm.invalid) {
      this.bidForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    const formValue = this.bidForm.value;
    const bidData: CreateBidDto = {
      price: formValue.price,
      arrivalDate: new Date(formValue.arrivalDate).toISOString(),
      truckId: this.driverTruck?.id
    };

    this.tripService.placeBid(this.tripId, bidData).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.successMessage = 'Your bid has been submitted successfully!';
        setTimeout(() => {
          this.router.navigate(['/trip/available']);
        }, 2000);
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err?.error?.error?.message || 'Failed to submit bid. Please try again.';
      }
    });
  }

  getTruckTypeName(type: TruckTypes | undefined): string {
    if (type === undefined) return 'Unknown';
    return TruckTypes[type]?.replace(/([A-Z])/g, ' $1').trim() || 'Unknown';
  }

  formatDate(dateString: string | undefined): string {
    if (!dateString) return 'N/A';
    return new Date(dateString).toLocaleDateString('en-US', {
      weekday: 'short',
      month: 'short',
      day: 'numeric',
      year: 'numeric'
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.bidForm.get(fieldName);
    return field ? field.invalid && field.touched : false;
  }
}
