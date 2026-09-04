import { Routes } from '@angular/router';
import { authGuard } from './core/auth.guard';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./features/auth/login.component').then((c) => c.LoginComponent) },
  { path: 'register', loadComponent: () => import('./features/auth/register.component').then((c) => c.RegisterComponent) },
  { path: 'tasks', canActivate: [authGuard], loadComponent: () => import('./features/tasks/tasks.component').then((c) => c.TasksComponent) },
  { path: '', pathMatch: 'full', redirectTo: 'tasks' },
  { path: '**', redirectTo: 'tasks' },
];
