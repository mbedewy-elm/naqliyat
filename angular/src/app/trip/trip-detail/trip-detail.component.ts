import { Component, inject, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ConfigStateService } from '@abp/ng.core';
import { TripService } from '../../proxy/trips/trip.service';
import { TruckService } from '../../proxy/trucks/truck.service';
import { TripDto, BidDto, CreateBidDto, ConfirmArrivalDto } from '../../proxy/trips/models';
import { TruckDto } from '../../proxy/trucks/models';
import { TruckTypes } from '../../proxy/enums/truck-types.enum';
import { TripStatuses } from '../../proxy/enums/trip-statuses.enum';

declare const L: any;

// Saudi Arabia city coordinates for geocoding
const SAUDI_CITIES: { [key: string]: [number, number] } = {
  'riyadh': [24.7136, 46.6753],
  'jeddah': [21.4858, 39.1925],
  'mecca': [21.3891, 39.8579],
  'medina': [24.5247, 39.5692],
  'dammam': [26.4207, 50.0888],
  'khobar': [26.2172, 50.1971],
  'dhahran': [26.2361, 50.0393],
  'tabuk': [28.3838, 36.5550],
  'abha': [18.2164, 42.5053],
  'taif': [21.2703, 40.4158],
  'hail': [27.5114, 41.7208],
  'jubail': [27.0046, 49.6225],
  'yanbu': [24.0895, 38.0618],
  'najran': [17.4933, 44.1277],
  'jazan': [16.8892, 42.5511],
  'al kharj': [24.1556, 47.3122],
  'qatif': [26.5196, 50.0115],
  'hofuf': [25.3648, 49.5870],
  'buraidah': [26.3260, 43.9750],
  'sakaka': [29.9697, 40.2064],
  'arar': [30.9753, 41.0381],
};

@Component({
  selector: 'app-trip-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './trip-detail.component.html',
  styleUrls: ['./trip-detail.component.scss']
})
export class TripDetailComponent implements OnInit, AfterViewInit {
  private route = inject(ActivatedRoute);
  private tripService = inject(TripService);
  private truckService = inject(TruckService);
  private configState = inject(ConfigStateService);
  private fb = inject(FormBuilder);

  trip: TripDto | null = null;
  bids: BidDto[] = [];
  isLoading = true;
  errorMessage = '';
  successMessage = '';
  tripId = '';

  // Bid form for drivers
  bidForm!: FormGroup;
  showBidForm = false;
  isSubmittingBid = false;

  // Driver's truck
  driverTruck: TruckDto | null = null;
  isLoadingTruck = false;

  // Confirm arrival
  otpForm!: FormGroup;
  showOtpForm = false;
  isConfirmingArrival = false;

  // Leaflet Map
  @ViewChild('mapContainer') mapContainer!: ElementRef;
  map: any = null;
  routeControl: any = null;
  currentPositionMarker: any = null;
  mapLoaded = false;

  TruckTypes = TruckTypes;
  TripStatuses = TripStatuses;

  ngOnInit(): void {
    this.tripId = this.route.snapshot.paramMap.get('id') || '';
    this.initBidForm();
    this.initOtpForm();
    if (this.tripId) {
      this.loadTrip();
      this.loadBids();
    }
    // Always try to load driver truck - the API will handle authorization
    this.loadDriverTruck();
  }

  ngAfterViewInit(): void {
    // Map will be initialized after trip is loaded
  }

  initMap(): void {
    if (!this.mapContainer || !this.trip || this.trip.statusId !== TripStatuses.OnItsWay) {
      return;
    }

    // Get coordinates for origin and destination
    const originCoords = this.getCityCoordinates(this.trip.fromLocation || '');
    const destCoords = this.getCityCoordinates(this.trip.toLocation || '');

    // Default center (Saudi Arabia) or midpoint between origin and destination
    const centerLat = (originCoords[0] + destCoords[0]) / 2;
    const centerLng = (originCoords[1] + destCoords[1]) / 2;

    // Initialize Leaflet map
    this.map = L.map(this.mapContainer.nativeElement).setView([centerLat, centerLng], 6);

    // Add OpenStreetMap tiles
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors'
    }).addTo(this.map);

    // Add origin marker (green)
    const originIcon = L.divIcon({
      className: 'custom-marker',
      html: '<div style="background: #10b981; width: 24px; height: 24px; border-radius: 50%; border: 3px solid white; box-shadow: 0 2px 5px rgba(0,0,0,0.3);"></div>',
      iconSize: [24, 24],
      iconAnchor: [12, 12]
    });
    L.marker(originCoords, { icon: originIcon })
      .addTo(this.map)
      .bindPopup(`<strong>Origin:</strong> ${this.trip.fromLocation}`);

    // Add destination marker (red)
    const destIcon = L.divIcon({
      className: 'custom-marker',
      html: '<div style="background: #ef4444; width: 24px; height: 24px; border-radius: 50%; border: 3px solid white; box-shadow: 0 2px 5px rgba(0,0,0,0.3);"></div>',
      iconSize: [24, 24],
      iconAnchor: [12, 12]
    });
    L.marker(destCoords, { icon: destIcon })
      .addTo(this.map)
      .bindPopup(`<strong>Destination:</strong> ${this.trip.toLocation}`);

    // Draw route line
    const routeLine = L.polyline([originCoords, destCoords], {
      color: '#f59e0b',
      weight: 4,
      opacity: 0.8,
      dashArray: '10, 10'
    }).addTo(this.map);

    // Add current position marker at random point along the route
    this.addCurrentPositionMarker(originCoords, destCoords);

    // Fit map to show all markers
    const bounds = L.latLngBounds([originCoords, destCoords]);
    this.map.fitBounds(bounds, { padding: [50, 50] });

    this.mapLoaded = true;
  }

  getCityCoordinates(location: string): [number, number] {
    const locationLower = location.toLowerCase().trim();
    
    // Check for exact match or partial match in city names
    for (const [city, coords] of Object.entries(SAUDI_CITIES)) {
      if (locationLower.includes(city) || city.includes(locationLower)) {
        return coords;
      }
    }
    
    // Default to Riyadh if city not found
    return SAUDI_CITIES['riyadh'];
  }

  addCurrentPositionMarker(originCoords: [number, number], destCoords: [number, number]): void {
    if (!this.map) return;

    // Get a random position between 20% and 80% of the route
    const randomProgress = 0.2 + Math.random() * 0.6;
    const currentLat = originCoords[0] + (destCoords[0] - originCoords[0]) * randomProgress;
    const currentLng = originCoords[1] + (destCoords[1] - originCoords[1]) * randomProgress;

    // Remove existing marker if any
    if (this.currentPositionMarker) {
      this.map.removeLayer(this.currentPositionMarker);
    }

    // Create truck icon marker
    const truckIcon = L.divIcon({
      className: 'truck-marker',
      html: `
        <div style="background: #f59e0b; width: 36px; height: 36px; border-radius: 50%; border: 3px solid #d97706; box-shadow: 0 2px 8px rgba(0,0,0,0.3); display: flex; align-items: center; justify-content: center;">
          <span style="font-size: 18px;">🚚</span>
        </div>
      `,
      iconSize: [36, 36],
      iconAnchor: [18, 18]
    });

    this.currentPositionMarker = L.marker([currentLat, currentLng], { icon: truckIcon })
      .addTo(this.map)
      .bindPopup(`
        <div style="padding: 4px; font-family: Arial, sans-serif;">
          <strong style="color: #f59e0b;">🚚 Truck in Transit</strong>
          <p style="margin: 4px 0 0 0; font-size: 12px; color: #666;">Currently on the way to destination</p>
        </div>
      `);
  }

  private initBidForm(): void {
    this.bidForm = this.fb.group({
      price: [null, [Validators.required, Validators.min(1)]],
      arrivalDate: ['', Validators.required]
    });
  }

  private initOtpForm(): void {
    this.otpForm = this.fb.group({
      otp: ['', [Validators.required, Validators.minLength(4)]]
    });
  }

  get isDriver(): boolean {
    const roles = this.configState.getOne('currentUser')?.roles || [];
    return roles.some((role: string) => role.toLowerCase() === 'driver');
  }

  get canPlaceBid(): boolean {
    return this.isDriver && this.trip?.statusId === TripStatuses.OpenForBidding && this.driverTruck !== null;
  }

  get canConfirmArrival(): boolean {
    return this.isDriver && this.trip?.statusId === TripStatuses.OnItsWay;
  }

  get currentUserId(): string {
    return this.configState.getDeep('currentUser.id') || '';
  }

  loadDriverTruck(): void {
    this.isLoadingTruck = true;
    const userId = this.currentUserId;
    console.log('Current User ID:', userId);
    
    if (!userId) {
      console.log('No user ID found');
      this.isLoadingTruck = false;
      return;
    }

    // First get driver by current user id
    this.truckService.getDriverByUserId(userId).subscribe({
      next: (driver) => {
        console.log('Driver found:', driver);
        if (driver?.id) {
          // Then get truck by driver id
          this.truckService.getTruckByDriverId(driver.id).subscribe({
            next: (truck) => {
              console.log('Truck found:', truck);
              this.driverTruck = truck;
              this.isLoadingTruck = false;
            },
            error: (err) => {
              console.error('Error getting truck:', err);
              this.driverTruck = null;
              this.isLoadingTruck = false;
            }
          });
        } else {
          console.log('No driver ID in response');
          this.driverTruck = null;
          this.isLoadingTruck = false;
        }
      },
      error: (err) => {
        console.error('Error getting driver:', err);
        this.driverTruck = null;
        this.isLoadingTruck = false;
      }
    });
  }

  loadTrip(): void {
    this.isLoading = true;
    this.tripService.getById(this.tripId).subscribe({
      next: (result) => {
        this.trip = result;
        this.isLoading = false;
        // Initialize map after trip is loaded and view is ready
        if (this.trip?.statusId === TripStatuses.OnItsWay) {
          setTimeout(() => this.initMap(), 100);
        }
      },
      error: (err) => {
        this.errorMessage = err?.error?.error?.message || 'Failed to load trip details';
        this.isLoading = false;
        console.error('Error loading trip:', err);
      }
    });
  }

  loadBids(): void {
    this.tripService.getNewBids(this.tripId).subscribe({
      next: (result) => {
        this.bids = result;
      },
      error: (err) => {
        console.error('Error loading bids:', err);
      }
    });
  }

  approveBid(bidId: string): void {
    this.tripService.approveBid(bidId).subscribe({
      next: () => {
        this.loadTrip();
        this.loadBids();
      },
      error: (err) => {
        console.error('Error approving bid:', err);
      }
    });
  }

  rejectBid(bidId: string): void {
    this.tripService.rejectBid(bidId).subscribe({
      next: () => {
        this.loadBids();
      },
      error: (err) => {
        console.error('Error rejecting bid:', err);
      }
    });
  }

  // Driver bid methods
  toggleBidForm(): void {
    this.showBidForm = !this.showBidForm;
    if (!this.showBidForm) {
      this.bidForm.reset();
      this.errorMessage = '';
    }
  }

  submitBid(): void {
    if (this.bidForm.invalid) {
      this.bidForm.markAllAsTouched();
      return;
    }

    this.isSubmittingBid = true;
    this.errorMessage = '';
    this.successMessage = '';

    const formValue = this.bidForm.value;
    const bidData: CreateBidDto = {
      truckId: this.driverTruck?.id,
      price: formValue.price,
      arrivalDate: new Date(formValue.arrivalDate).toISOString()
    };

    this.tripService.placeBid(this.tripId, bidData).subscribe({
      next: () => {
        this.isSubmittingBid = false;
        this.successMessage = 'Your bid has been submitted successfully!';
        this.showBidForm = false;
        this.bidForm.reset();
      },
      error: (err) => {
        this.isSubmittingBid = false;
        this.errorMessage = err?.error?.error?.message || 'Failed to submit bid. Please try again.';
      }
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.bidForm.get(fieldName);
    return field ? field.invalid && field.touched : false;
  }

  isOtpFieldInvalid(fieldName: string): boolean {
    const field = this.otpForm.get(fieldName);
    return field ? field.invalid && field.touched : false;
  }

  // Confirm arrival methods
  toggleOtpForm(): void {
    this.showOtpForm = !this.showOtpForm;
    if (!this.showOtpForm) {
      this.otpForm.reset();
      this.errorMessage = '';
    }
  }

  confirmArrival(): void {
    if (this.otpForm.invalid) {
      this.otpForm.markAllAsTouched();
      return;
    }

    this.isConfirmingArrival = true;
    this.errorMessage = '';
    this.successMessage = '';

    const confirmData: ConfirmArrivalDto = {
      otp: this.otpForm.value.otp
    };

    this.tripService.confirmArrival(this.tripId, confirmData).subscribe({
      next: () => {
        this.isConfirmingArrival = false;
        this.successMessage = 'Arrival confirmed successfully!';
        this.showOtpForm = false;
        this.otpForm.reset();
        this.loadTrip(); // Reload trip to update status
      },
      error: (err) => {
        this.isConfirmingArrival = false;
        this.errorMessage = err?.error?.error?.message || 'Failed to confirm arrival. Please check the OTP and try again.';
      }
    });
  }

  getTruckTypeName(type: TruckTypes | undefined): string {
    if (type === undefined) return 'Unknown';
    return TruckTypes[type] || 'Unknown';
  }

  getStatusName(status: TripStatuses | undefined): string {
    if (status === undefined) return 'Unknown';
    return TripStatuses[status] || 'Unknown';
  }

  getStatusClass(status: TripStatuses | undefined): string {
    switch (status) {
      case TripStatuses.OpenForBidding: return 'status-open';
      case TripStatuses.WaitingBidAgreement: return 'status-waiting';
      case TripStatuses.WaitingPayment: return 'status-payment';
      case TripStatuses.OnItsWay: return 'status-progress';
      case TripStatuses.Arrived: return 'status-completed';
      default: return '';
    }
  }
}
