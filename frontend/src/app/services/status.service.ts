import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { StudentStatus } from '../models/status.model';

@Injectable({ providedIn: 'root' })
export class StatusService {
  constructor(private readonly http: HttpClient) {}

  getMyStatus(): Observable<StudentStatus> {
    return this.http.get<StudentStatus>(`${environment.apiBaseUrl}/students/me/status`);
  }
}
