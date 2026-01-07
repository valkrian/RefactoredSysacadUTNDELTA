import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Plan } from '../models/plan.model';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class PlanService {
  constructor(private readonly http: HttpClient) {}

  getMyPlan(): Observable<Plan> {
    return this.http.get<Plan>(`${environment.apiBaseUrl}/students/me/plan`);
  }
}
