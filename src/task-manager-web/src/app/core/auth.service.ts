import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { tap } from 'rxjs';
import { AuthResult } from './models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly tokenKey = 'task-manager.token';
  private readonly userKey = 'task-manager.user';
  readonly token = signal(sessionStorage.getItem(this.tokenKey));
  readonly user = signal<AuthResult | null>(this.readUser());
  readonly isAuthenticated = computed(() => Boolean(this.token()));

  login(email: string, password: string) {
    return this.http.post<AuthResult>('/api/auth/login', { email, password }).pipe(
      tap((result) => this.storeSession(result)),
    );
  }

  register(name: string, email: string, password: string) {
    return this.http.post<AuthResult>('/api/auth/register', { name, email, password }).pipe(
      tap((result) => this.storeSession(result)),
    );
  }

  logout(): void {
    sessionStorage.removeItem(this.tokenKey);
    sessionStorage.removeItem(this.userKey);
    this.token.set(null);
    this.user.set(null);
  }

  private storeSession(result: AuthResult): void {
    sessionStorage.setItem(this.tokenKey, result.accessToken);
    sessionStorage.setItem(this.userKey, JSON.stringify(result));
    this.token.set(result.accessToken);
    this.user.set(result);
  }

  private readUser(): AuthResult | null {
    const value = sessionStorage.getItem(this.userKey);
    if (!value) return null;
    try {
      return JSON.parse(value) as AuthResult;
    } catch {
      sessionStorage.removeItem(this.userKey);
      return null;
    }
  }
}
