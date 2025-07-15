import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CdbSimulationResponse, CdbSimulationRequest } from '../models/cdb.model';

@Injectable({
  providedIn: 'root'
})
export class CdbService {
  constructor(private readonly http: HttpClient) { }

  simulate(data: CdbSimulationRequest): Observable<CdbSimulationResponse> {
    const formData = new FormData();
    formData.append('Months', data.months.toString());
    formData.append('InitialValue', data.initialValue.toString());

    return this.http.post<CdbSimulationResponse>('/gateway/Cdb/Simulate', formData);
  }
}
