import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LocalizationModule } from '@abp/ng.core';
import { TripService } from '../../proxy/trips/trip.service';
import { TripDto } from '../../proxy/trips/models';
import { TruckTypes } from '../../proxy/enums/truck-types.enum';
import { TripStatuses } from '../../proxy/enums/trip-statuses.enum';

@Component({
  selector: 'app-my-trips',
  standalone: true,
  imports: [CommonModule, RouterModule, LocalizationModule],
  templateUrl: './my-trips.component.html',
  styleUrls: ['./my-trips.component.scss']
})
export class MyTripsComponent implements OnInit {
  private tripService = inject(TripService);

  trips: TripDto[] = [];
  filteredTrips: TripDto[] = [];
  isLoading = true;
  errorMessage = '';
  searchTerm = '';
  selectedStatus: string = 'all';

  TruckTypes = TruckTypes;
  TripStatuses = TripStatuses;

  ngOnInit(): void {
    this.loadTrips();
  }

  loadTrips(): void {
    this.isLoading = true;
    this.tripService.getDriverTrips().subscribe({
      next: (result) => {
        this.trips = result;
        this.applyFilters();
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.error?.message || 'Failed to load your trips';
        this.isLoading = false;
      }
    });
  }

  onSearch(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.searchTerm = target.value.toLowerCase();
    this.applyFilters();
  }

  onStatusFilter(status: string): void {
    this.selectedStatus = status;
    this.applyFilters();
  }

  applyFilters(): void {
    let filtered = [...this.trips];

    // Apply search filter
    if (this.searchTerm) {
      filtered = filtered.filter(trip =>
        trip.fromLocation?.toLowerCase().includes(this.searchTerm) ||
        trip.toLocation?.toLowerCase().includes(this.searchTerm) ||
        trip.goodsType?.toLowerCase().includes(this.searchTerm)
      );
    }

    // Apply status filter
    if (this.selectedStatus !== 'all') {
      const statusValue = parseInt(this.selectedStatus, 10);
      filtered = filtered.filter(trip => trip.statusId === statusValue);
    }

    this.filteredTrips = filtered;
  }

  getTruckTypeName(type: TruckTypes | undefined): string {
    if (type === undefined) return 'Unknown';
    return TruckTypes[type]?.replace(/([A-Z])/g, ' $1').trim() || 'Unknown';
  }

  getStatusName(status: TripStatuses | undefined): string {
    if (status === undefined) return 'Unknown';
    const statusMap: { [key: number]: string } = {
      [TripStatuses.OpenForBidding]: 'Open for Bidding',
      [TripStatuses.WaitingBidAgreement]: 'Waiting Agreement',
      [TripStatuses.WaitingPayment]: 'Waiting Payment',
      [TripStatuses.OnItsWay]: 'On Its Way',
      [TripStatuses.Arrived]: 'Arrived'
    };
    return statusMap[status] || 'Unknown';
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

  formatDate(dateString: string | undefined): string {
    if (!dateString) return 'N/A';
    return new Date(dateString).toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric'
    });
  }
}
