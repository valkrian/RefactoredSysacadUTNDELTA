import { Routes } from '@angular/router';
//defines routes for the app
export const routes: Routes = [
    {
        path: '',
        redirectTo: '/dashboard',
        pathMatch: 'full',
    },
    {
        //dashboard route
        path: 'dashboard',
        //load the comp
        loadComponent: () => import('./components/dashboard/dashboard').then(m => m.Dashboard)
    }
];
