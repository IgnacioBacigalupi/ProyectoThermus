import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { ReadingService } from '../../services/reading.service';
import { UltimaLecturaPorDispositivo } from '../../models/UltimaLecturaPorDispositivo';

@Component({
  selector: 'app-home',
  imports: [CommonModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {

  private readingService = inject(ReadingService);
  private cdr = inject(ChangeDetectorRef);

  readingsByDevice: UltimaLecturaPorDispositivo[] = [];
  isLoading = true;
  errorMessage = '';

  ngOnInit(): void {
    console.log('Entro a Home');
    this.loadReadings();
  }

  loadReadings(): void {
    console.log('Entro a loadReadings');

    this.isLoading = true;
    this.errorMessage = '';

    this.readingService.getLatestReadingsByDevice().subscribe({
      next: (data: any) => {
        console.log('Lecturas recibidas:', data);
        console.log('Es array:', Array.isArray(data));
        console.log('Cantidad:', data?.length);

        this.readingsByDevice = Array.isArray(data) ? [...data] : [];
        this.isLoading = false;
        this.errorMessage = '';

        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error cargando lecturas:', err);

        this.readingsByDevice = [];
        this.isLoading = false;
        this.errorMessage = 'No pude cargar las lecturas de los dispositivos.';

        this.cdr.detectChanges();
      }
    });
  }

  getCardTitle(device: UltimaLecturaPorDispositivo): string {
    return device.location || device.name || device.externalId;
  }

  formatTime(dateUtc: string): string {
    return new Date(dateUtc).toLocaleString('es-ES');
  }
}

