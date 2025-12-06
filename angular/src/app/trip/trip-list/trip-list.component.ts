import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { LocalizationModule } from '@abp/ng.core';
import { TripService } from '../../proxy/trips/trip.service';
import { TripDto } from '../../proxy/trips/models';
import { TruckTypes } from '../../proxy/enums/truck-types.enum';
import { TripStatuses } from '../../proxy/enums/trip-statuses.enum';

@Component({
  selector: 'app-trip-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, LocalizationModule],
  templateUrl: './trip-list.component.html',
  styleUrls: ['./trip-list.component.scss']
})
export class TripListComponent implements OnInit {
  private tripService = inject(TripService);

  trips: TripDto[] = [];
  isLoading = true;
  errorMessage = '';
  locationFilter = '';

  TruckTypes = TruckTypes;
  TripStatuses = TripStatuses;

  ngOnInit(): void {
    this.loadTrips();
  }

  loadTrips(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.tripService.getOpenTrips(this.locationFilter || undefined).subscribe({
      next: (result) => {
        this.trips = result;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.error?.message || 'Failed to load trips';
        this.isLoading = false;
        console.error('Error loading trips:', err);
      }
    });
  }

  onSearch(): void {
    this.loadTrips();
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
