import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Lectura } from '../models/lectura';

@Injectable({
  providedIn: 'root'
})
export class ReadingService {

  constructor(private http: HttpClient) { }

  getlatestReading():Observable<Lectura>{
    return this.http.get()
  }
  
}
