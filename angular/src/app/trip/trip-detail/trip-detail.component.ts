import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { TripService } from '../../proxy/trips/trip.service';
import { TripDto, BidDto } from '../../proxy/trips/models';
import { TruckTypes } from '../../proxy/enums/truck-types.enum';
import { TripStatuses } from '../../proxy/enums/trip-statuses.enum';

@Component({
  selector: 'app-trip-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './trip-detail.component.html',
  styleUrls: ['./trip-detail.component.scss']
})
export class TripDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private tripService = inject(TripService);

  trip: TripDto | null = null;
  bids: BidDto[] = [];
  isLoading = true;
  errorMessage = '';
  tripId = '';

  TruckTypes = TruckTypes;
  TripStatuses = TripStatuses;

  ngOnInit(): void {
    this.tripId = this.route.snapshot.paramMap.get('id') || '';
    if (this.tripId) {
      this.loadTrip();
      this.loadBids();
    }
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
