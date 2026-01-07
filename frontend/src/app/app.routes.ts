import { Routes } from '@angular/router';
import { authGuard, redirectIfAuthenticatedGuard } from './guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/dashboard',
    pathMatch: 'full',
  },
  {
    path: 'login',
    canActivate: [redirectIfAuthenticatedGuard],
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
  {
  path: 'recuperar',
  loadComponent: () =>
    import('./components/recover/recover').then((m) => m.RecoverComponent),
},
{
  path: 'registro',
  loadComponent: () =>
    import('./components/register/register').then((m) => m.RegisterComponent),
},
{
  path: 'ayuda',
  loadComponent: () =>
    import('./components/help/help').then((m) => m.HelpComponent),
},
{
  path: 'perfil',
  canActivate: [authGuard],
  loadComponent: () =>
    import('./components/profile/profile').then((m) => m.ProfileComponent),
},

];
