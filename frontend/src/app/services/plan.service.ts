import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Plan } from '../models/plan.model';
import { Observable } from 'rxjs';

const API_BASE_URL = 'http://localhost:5018';

@Injectable({ providedIn: 'root' })
export class PlanService {
  constructor(private readonly http: HttpClient) {}

  getMyPlan(): Observable<Plan> {
    return this.http.get<Plan>(`${API_BASE_URL}/students/me/plan`);
  }
}
