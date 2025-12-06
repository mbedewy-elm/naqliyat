import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LocalizationModule } from '@abp/ng.core';
import { TruckService } from '../../proxy/trucks/truck.service';
import { DriverDto } from '../../proxy/trucks/models';

@Component({
  selector: 'app-driver-list',
  standalone: true,
  imports: [CommonModule, RouterModule, LocalizationModule],
  templateUrl: './driver-list.component.html',
  styleUrls: ['./driver-list.component.scss']
})
export class DriverListComponent implements OnInit {
  private truckService = inject(TruckService);

  drivers: DriverDto[] = [];
  filteredDrivers: DriverDto[] = [];
  isLoading = true;
  errorMessage = '';
  searchTerm = '';

  ngOnInit(): void {
    this.loadDrivers();
  }

  loadDrivers(): void {
    this.isLoading = true;
    this.truckService.getDriverList().subscribe({
      next: (result) => {
        this.drivers = result;
        this.filteredDrivers = [...this.drivers];
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.error?.message || 'Failed to load drivers';
        this.isLoading = false;
      }
    });
  }

  onSearch(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.searchTerm = target.value.toLowerCase();
    this.filteredDrivers = this.drivers.filter(driver =>
      (driver.name?.toLowerCase().includes(this.searchTerm) ||
       driver.phone?.toLowerCase().includes(this.searchTerm) ||
       driver.licenseNumber?.toLowerCase().includes(this.searchTerm) ||
       driver.identification?.toLowerCase().includes(this.searchTerm))
    );
  }
}
