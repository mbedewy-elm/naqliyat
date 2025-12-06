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
  selector: 'app-driver-trips',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, LocalizationModule],
  templateUrl: './driver-trips.component.html',
  styleUrls: ['./driver-trips.component.scss']
})
export class DriverTripsComponent implements OnInit {
  private tripService = inject(TripService);

  trips: TripDto[] = [];
  filteredTrips: TripDto[] = [];
  isLoading = true;
  errorMessage = '';
  locationFilter = '';

  TruckTypes = TruckTypes;
  TripStatuses = TripStatuses;

  ngOnInit(): void {
    this.loadOpenTrips();
  }

  loadOpenTrips(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.tripService.getOpenTrips(this.locationFilter || undefined).subscribe({
      next: (result) => {
        // Filter only trips with OpenForBidding status
        this.trips = result.filter(trip => trip.statusId === TripStatuses.OpenForBidding);
        this.filteredTrips = [...this.trips];
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.error?.message || 'Failed to load available trips';
        this.isLoading = false;
        console.error('Error loading trips:', err);
      }
    });
  }

  onSearch(): void {
    this.loadOpenTrips();
  }

  getTruckTypeName(type: TruckTypes | undefined): string {
    if (type === undefined) return 'Unknown';
    return TruckTypes[type]?.replace(/([A-Z])/g, ' $1').trim() || 'Unknown';
  }

  formatDate(dateString: string | undefined): string {
    if (!dateString) return 'N/A';
    return new Date(dateString).toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric'
    });
  }

  getTimeAgo(dateString: string | undefined): string {
    if (!dateString) return '';
    const date = new Date(dateString);
    const now = new Date();
    const diffMs = now.getTime() - date.getTime();
    const diffHours = Math.floor(diffMs / (1000 * 60 * 60));
    const diffDays = Math.floor(diffHours / 24);

    if (diffDays > 0) return `${diffDays}d ago`;
    if (diffHours > 0) return `${diffHours}h ago`;
    return 'Just now';
  }
}
