import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { LocalizationModule } from '@abp/ng.core';
import { TruckService } from '../../proxy/trucks/truck.service';
import { TruckDto } from '../../proxy/trucks/models';
import { TruckTypes } from '../../proxy/enums/truck-types.enum';
import { TemperatureRequirements } from '../../proxy/enums/temperature-requirements.enum';

@Component({
  selector: 'app-truck-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, LocalizationModule],
  templateUrl: './truck-detail.component.html',
  styleUrls: ['./truck-detail.component.scss']
})
export class TruckDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private truckService = inject(TruckService);

  truck: TruckDto | null = null;
  isLoading = true;
  errorMessage = '';
  truckId = '';

  ngOnInit(): void {
    this.truckId = this.route.snapshot.paramMap.get('id') || '';
    if (this.truckId) {
      this.loadTruck();
    }
  }

  loadTruck(): void {
    this.isLoading = true;
    // Since there's no getById, we'll get the list and find the truck
    this.truckService.getList().subscribe({
      next: (result) => {
        this.truck = result.find(t => t.id === this.truckId) || null;
        this.isLoading = false;
        if (!this.truck) {
          this.errorMessage = 'Truck not found';
        }
      },
      error: (err) => {
        this.errorMessage = err?.error?.error?.message || 'Failed to load truck details';
        this.isLoading = false;
      }
    });
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
