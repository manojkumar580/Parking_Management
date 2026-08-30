import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ParkingSpace } from '../models/parking-space.model';

@Injectable({
  providedIn: 'root'
})
export class ParkingSpaceService {

  private readonly apiUrl =
    'https://localhost:7295/api/ParkingSpaces';

  constructor(private http: HttpClient) { }

  getAll(): Observable<ParkingSpace[]> {
    console.log(
      'Calling:',
      this.apiUrl
    );

    return this.http.get<ParkingSpace[]>(
      this.apiUrl
    );
  }
}
