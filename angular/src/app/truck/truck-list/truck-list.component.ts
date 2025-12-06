import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LocalizationModule } from '@abp/ng.core';
import { TruckService } from '../../proxy/trucks/truck.service';
import { TruckDto } from '../../proxy/trucks/models';
import { TruckTypes } from '../../proxy/enums/truck-types.enum';
import { TemperatureRequirements } from '../../proxy/enums/temperature-requirements.enum';

@Component({
  selector: 'app-truck-list',
  standalone: true,
  imports: [CommonModule, RouterModule, LocalizationModule],
  templateUrl: './truck-list.component.html',
  styleUrls: ['./truck-list.component.scss']
})
export class TruckListComponent implements OnInit {
  private truckService = inject(TruckService);

  trucks: TruckDto[] = [];
  filteredTrucks: TruckDto[] = [];
  isLoading = true;
  errorMessage = '';
  searchTerm = '';

  TruckTypes = TruckTypes;

  ngOnInit(): void {
    this.loadTrucks();
  }

  loadTrucks(): void {
    this.isLoading = true;
    this.truckService.getList().subscribe({
      next: (result) => {
        this.trucks = result;
        this.filteredTrucks = [...this.trucks];
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.error?.message || 'Failed to load trucks';
        this.isLoading = false;
      }
    });
  }

  onSearch(event: Event): void {
    const target = event.target as HTMLInputElement;
    this.searchTerm = target.value.toLowerCase();
    this.filteredTrucks = this.trucks.filter(truck =>
      (truck.make?.toLowerCase().includes(this.searchTerm) ||
       truck.model?.toLowerCase().includes(this.searchTerm) ||
       truck.licenseNumber?.toLowerCase().includes(this.searchTerm) ||
       truck.registrationNumber?.toLowerCase().includes(this.searchTerm))
    );
  }

  getTruckTypeName(type: TruckTypes | undefined): string {
    if (type === undefined) return 'Unknown';
    return TruckTypes[type]?.replace(/([A-Z])/g, ' $1').trim() || 'Unknown';
  }

  getTemperatureName(temp: TemperatureRequirements | undefined): string {
    if (temp === undefined) return 'N/A';
    return TemperatureRequirements[temp] || 'N/A';
  }
}
