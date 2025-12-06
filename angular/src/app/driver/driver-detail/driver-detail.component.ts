import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { LocalizationModule } from '@abp/ng.core';
import { TruckService } from '../../proxy/trucks/truck.service';
import { DriverDto } from '../../proxy/trucks/models';

@Component({
  selector: 'app-driver-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, LocalizationModule],
  templateUrl: './driver-detail.component.html',
  styleUrls: ['./driver-detail.component.scss']
})
export class DriverDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private truckService = inject(TruckService);

  driver: DriverDto | null = null;
  isLoading = true;
  errorMessage = '';
  driverId = '';

  ngOnInit(): void {
    this.driverId = this.route.snapshot.paramMap.get('id') || '';
    if (this.driverId) {
      this.loadDriver();
    }
  }

  loadDriver(): void {
    this.isLoading = true;
    this.truckService.getDriverById(this.driverId).subscribe({
      next: (result) => {
        this.driver = result;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.error?.message || 'Failed to load driver details';
        this.isLoading = false;
      }
    });
  }
}
