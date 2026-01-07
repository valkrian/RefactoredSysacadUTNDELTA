import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { ExamCall, ExamEnrollment } from '../models/exam.model';

@Injectable({ providedIn: 'root' })
export class ExamService {
  constructor(private readonly http: HttpClient) {}

  getExamCalls(filters?: { from?: string; to?: string; subjectId?: number }): Observable<ExamCall[]> {
    let params = new HttpParams();
    if (filters?.from) {
      params = params.set('from', filters.from);
    }
    if (filters?.to) {
      params = params.set('to', filters.to);
    }
    if (filters?.subjectId) {
      params = params.set('subjectId', filters.subjectId);
    }

    return this.http.get<ExamCall[]>(`${environment.apiBaseUrl}/exam-calls`, { params });
  }

  enrollExam(examCallId: number): Observable<void> {
    return this.http.post<void>(`${environment.apiBaseUrl}/students/me/exam-enrollments`, {
      examCallId,
    });
  }

  getMyExamEnrollments(): Observable<ExamEnrollment[]> {
    return this.http.get<ExamEnrollment[]>(
      `${environment.apiBaseUrl}/students/me/exam-enrollments`
    );
  }
}
