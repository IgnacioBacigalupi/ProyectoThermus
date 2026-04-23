import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, DestroyRef, inject, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { interval } from 'rxjs';
import { ReadingHistoryItem } from '../../models/reading-history-item';
import { UltimaLecturaPorDispositivo } from '../../models/UltimaLecturaPorDispositivo';
import { ReadingService } from '../../services/reading.service';

@Component({
  selector: 'app-home',
  imports: [CommonModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  private readingService = inject(ReadingService);
  private cdr = inject(ChangeDetectorRef);
  private destroyRef = inject(DestroyRef);
  private readonly refreshIntervalMs = 10000;

  readingsByDevice: UltimaLecturaPorDispositivo[] = [];
  readingHistoryByDevice: Record<number, ReadingHistoryItem[]> = {};
  flippedCards: Record<number, boolean> = {};
  historyLoadingByDevice: Record<number, boolean> = {};
  historyErrorByDevice: Record<number, string> = {};
  isLoading = true;
  errorMessage = '';

  ngOnInit(): void {
    console.log('Entro a Home');
    this.loadReadings();

    interval(this.refreshIntervalMs)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(() => this.loadReadings(false));
  }

  loadReadings(showLoadingState = true): void {
    console.log('Entro a loadReadings');

    if (showLoadingState) {
      this.isLoading = true;
    }

    this.errorMessage = '';

    this.readingService.getLatestReadingsByDevice().subscribe({
      next: (data: unknown) => {
        console.log('Lecturas recibidas:', data);
        console.log('Es array:', Array.isArray(data));

        this.readingsByDevice = Array.isArray(data) ? [...data] : [];
        this.isLoading = false;
        this.errorMessage = '';

        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error cargando lecturas:', err);

        if (showLoadingState) {
          this.readingsByDevice = [];
        }

        this.isLoading = false;
        this.errorMessage = 'No pude cargar las lecturas de los dispositivos.';

        this.cdr.detectChanges();
      }
    });
  }

  getCardTitle(device: UltimaLecturaPorDispositivo): string {
    return device.location || device.name || device.externalId;
  }

  toggleCard(device: UltimaLecturaPorDispositivo): void {
    const isFlipped = this.flippedCards[device.deviceId] ?? false;

    this.flippedCards = {
      ...this.flippedCards,
      [device.deviceId]: !isFlipped,
    };

    if (!isFlipped && !this.readingHistoryByDevice[device.deviceId] && !this.historyLoadingByDevice[device.deviceId]) {
      this.loadHistory(device.deviceId);
    }
  }

  loadHistory(deviceId: number): void {
    this.historyLoadingByDevice = {
      ...this.historyLoadingByDevice,
      [deviceId]: true,
    };

    this.historyErrorByDevice = {
      ...this.historyErrorByDevice,
      [deviceId]: '',
    };

    this.readingService.getLatestHistoryByDevice(deviceId).subscribe({
      next: (history) => {
        this.readingHistoryByDevice = {
          ...this.readingHistoryByDevice,
          [deviceId]: history,
        };

        this.historyLoadingByDevice = {
          ...this.historyLoadingByDevice,
          [deviceId]: false,
        };

        this.cdr.detectChanges();
      },
      error: () => {
        this.historyLoadingByDevice = {
          ...this.historyLoadingByDevice,
          [deviceId]: false,
        };

        this.historyErrorByDevice = {
          ...this.historyErrorByDevice,
          [deviceId]: 'No pude cargar el historial del sensor.',
        };

        this.cdr.detectChanges();
      }
    });
  }

  isCardFlipped(deviceId: number): boolean {
    return this.flippedCards[deviceId] ?? false;
  }

  getHistory(deviceId: number): ReadingHistoryItem[] {
    return this.readingHistoryByDevice[deviceId] ?? [];
  }

  formatTime(dateUtc: string): string {
    return new Date(dateUtc).toLocaleString('es-ES');
  }
}
