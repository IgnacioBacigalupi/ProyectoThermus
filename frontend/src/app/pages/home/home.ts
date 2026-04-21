import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
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

  readingsByDevice: UltimaLecturaPorDispositivo[] = [];
  isLoading = true;
  errorMessage = '';

  ngOnInit(): void {
    this.loadReadings();
  }

  loadReadings(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.readingService.getLatestReadingsByDevice().subscribe({
      next: (data) => {
        console.log('Lecturas recibidas:', data);
        this.readingsByDevice = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error cargando lecturas:', err);
        this.errorMessage = 'No pude cargar las lecturas de los dispositivos.';
        this.isLoading = false;
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