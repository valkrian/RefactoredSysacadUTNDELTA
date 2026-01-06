import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { CourseEnrollment, CourseOffer } from '../models/course.model';

@Injectable({ providedIn: 'root' })
export class CourseService {
  constructor(private readonly http: HttpClient) {}

  getCourseOffers(period?: string): Observable<CourseOffer[]> {
    let params = new HttpParams();
    if (period) {
      params = params.set('period', period);
    }
    return this.http.get<CourseOffer[]>(`${environment.apiBaseUrl}/course-offers`, { params });
  }

  enrollCourse(subjectId: number, period: string): Observable<void> {
    return this.http.post<void>(`${environment.apiBaseUrl}/students/me/course-enrollments`, {
      subjectId,
      period,
    });
  }

  getMyCourseEnrollments(): Observable<CourseEnrollment[]> {
    return this.http.get<CourseEnrollment[]>(
      `${environment.apiBaseUrl}/students/me/course-enrollments`
    );
  }
}
