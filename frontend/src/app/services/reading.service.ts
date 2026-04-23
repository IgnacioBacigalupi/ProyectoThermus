import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Lectura } from '../models/lectura';
import { UltimaLecturaPorDispositivo } from '../models/UltimaLecturaPorDispositivo';
import { ReadingHistoryItem } from '../models/reading-history-item';


@Injectable({
  providedIn: 'root'
})
export class ReadingService {

  constructor(private http: HttpClient) { }

  getlatestReading():Observable<Lectura>{
    return this.http.get<Lectura>('/api/readings/ultima');
  }

    getLatestReadingsByDevice(): Observable<UltimaLecturaPorDispositivo[]> {
    return this.http.get<UltimaLecturaPorDispositivo[]>('/api/readings/ultimas-por-dispositivo');
  }

  getLatestHistoryByDevice(deviceId: number): Observable<ReadingHistoryItem[]> {
    return this.http.get<ReadingHistoryItem[]>(`/api/readings/dispositivo/${deviceId}/ultimas`);
  }
  
}
