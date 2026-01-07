import { Injectable, inject, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { LoginResponse, MeResponse } from '../models/auth.model';
import { Observable, tap } from 'rxjs';

const TOKEN_KEY = 'autogestion_token';

@Injectable({ providedIn: 'root' })
export class AuthService {
  readonly me = signal<MeResponse | null>(null);
  readonly initialized = signal(false);
  private readonly platformId = inject(PLATFORM_ID);

  constructor(private readonly http: HttpClient) {
    if (isPlatformBrowser(this.platformId)) {
      this.initialized.set(true);
    }
  }

  get token(): string | null {
    if (!isPlatformBrowser(this.platformId)) {
      return null;
    }

    return localStorage.getItem(TOKEN_KEY);
  }

  isAuthenticated(): boolean {
    return !!this.token;
  }

  login(email: string, password: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${environment.apiBaseUrl}/auth/login`, {
        email,
        password,
      })
      .pipe(
        tap((response) => {
          if (isPlatformBrowser(this.platformId)) {
            localStorage.setItem(TOKEN_KEY, response.token);
          }
        })
      );
  }

  loadMe(): Observable<MeResponse> {
    return this.http.get<MeResponse>(`${environment.apiBaseUrl}/me`).pipe(
      tap((response) => {
        this.me.set(response);
      })
    );
  }

  logout(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem(TOKEN_KEY);
    }
    this.me.set(null);
  }
}
