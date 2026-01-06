import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/dashboard',
    pathMatch: 'full',
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./components/login/login').then((m) => m.LoginComponent),
  },
  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./components/dashboard/dashboard').then((m) => m.Dashboard),
  },
  {
    path: 'plan',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./components/plan/plan').then((m) => m.PlanComponent),
  },
  {
    path: 'estado',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./components/status/status').then((m) => m.StatusComponent),
  },
  {
    path: 'examenes',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./components/exams/exams').then((m) => m.ExamsComponent),
  },
  {
    path: 'cursado',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./components/courses/courses').then((m) => m.CoursesComponent),
  },
];
