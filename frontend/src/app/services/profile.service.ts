import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

export interface Profile {
    studentId: number;
    fullName: string;
    email: string;
    legajo: string;
    }

    @Injectable({ providedIn: 'root' })
    export class ProfileService {
    constructor(private readonly http: HttpClient) {}

    getProfile(): Observable<Profile> {
        return this.http.get<Profile>(`${environment.apiBaseUrl}/me/profile`);
    }

    updateProfile(fullName: string, email: string): Observable<void> {
        return this.http.put<void>(`${environment.apiBaseUrl}/me/profile`, {
        fullName,
        email,
        });
    }

    changePassword(currentPassword: string, newPassword: string): Observable<void> {
        return this.http.put<void>(`${environment.apiBaseUrl}/me/password`, {
        currentPassword,
        newPassword,
        });
    }
}
